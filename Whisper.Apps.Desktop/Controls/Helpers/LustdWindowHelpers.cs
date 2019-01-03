using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Whisper.Apps.Desktop.Controls.Helpers
{
    public static class LustdWindowHelpers
    {
        private static readonly Dictionary<IntPtr, Window> windows = new Dictionary<IntPtr, Window>();

        public static void OnSourceInitialized(Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;

            if (windows.ContainsKey(handle) || handle == IntPtr.Zero)
                return;

            windows.Add(handle, window);

            window.SizeChanged += Window_SizeChanged;

            EventHandler handler = null;
            handler = (sender, e) =>
            {
                window.Closed -= handler;

                windows.Remove(handle);
            };
            window.Closed += handler;

            var hwndSource = HwndSource.FromHwnd(handle);

            if (hwndSource == null)
                return;

            hwndSource.AddHook(WindowProc);
        }

        private static void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // TODO: Do something less shit than this.
            var w = (Window)sender;

            if (e.NewSize.Width < w.MinWidth)
                w.Width = w.MinWidth;

            if (e.NewSize.Height < w.MinHeight)
                w.Height = w.MinHeight;
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Fix maximise issue
                case WindowMessages.GetMinMaxInfo:

                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;

                    break;

                // Fix min size issue
                case WindowMessages.WindowPosChanging:

                    var pos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

                    if ((pos.flags & SetWindowPositionFlags.NoMove) != 0)
                    {
                        return IntPtr.Zero;
                    }

                    var wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;
                    if (wnd == null)
                    {
                        return IntPtr.Zero;
                    }

                    var changedPos = false;

                    // Note: pos.cx and pos.cy are 'device pixels'
                    if (windows.ContainsKey(hwnd))
                    {
                        var window = windows[hwnd];

                        Matrix transformToDevice;

                        using (var source = new HwndSource(new HwndSourceParameters()))
                        {
                            transformToDevice = source.CompositionTarget.TransformToDevice;
                        }

                        var s = transformToDevice.Transform((Vector)new Size(window.MinWidth, window.MinHeight));

                        if (pos.cx < s.X) { pos.cx = (int)s.X; changedPos = true; }
                        if (pos.cy < s.Y) { pos.cy = (int)s.Y; changedPos = true; }
                    }

                    if (changedPos)
                    {
                        Marshal.StructureToPtr(pos, lParam, true);
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            const int MONITOR_DEFAULTTONEAREST = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        
        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            public RECT rcMonitor = new RECT();

            public RECT rcWork = new RECT();

            public int dwFlags = 0;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;


            public static readonly RECT Empty;


            public int Width => Math.Abs(right - left);

            public int Height => bottom - top;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }

            public bool IsEmpty => left >= right || top >= bottom;

            public override string ToString()
            {
                if (this == Empty) return "RECT {Empty}";
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) return false;
                return this == (RECT)obj;
            }

            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }

            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
            }

            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }
    }
}
