namespace Whisper.Apps.Common.Models.Initialisation
{
    public class InitialisationUpdate
    {
        public InitialisationUpdate(InitialisationStages stage, double progress = 0)
        {
            Stage = stage;
            ProgressPercentage = progress;
        }

        public double ProgressPercentage { get; }
        public InitialisationStages Stage { get; }
    }
}
