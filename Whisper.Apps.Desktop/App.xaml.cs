using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using Splat;
using Whisper.Apps.Desktop.ViewModels;
using Whisper.Apps.Desktop.Views;

namespace Whisper.Apps.Desktop
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            Locator.CurrentMutable.Register(() => new HistoryListItemView(), typeof(IViewFor<HistoryListItemGuidViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListItemView(), typeof(IViewFor<HistoryListItemPasswordViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryListView(), typeof(IViewFor<HistoryListViewModel>));

            var mainWindowViewModel = new MainWindowViewModel(new HistoryListViewModel());
            var mainWindow = new MainWindow()
            {
                ViewModel = mainWindowViewModel
            };

            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
