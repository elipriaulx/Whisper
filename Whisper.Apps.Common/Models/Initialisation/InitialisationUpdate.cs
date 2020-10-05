namespace Whisper.Apps.Common.Models.Initialisation
{
    public class InitialisationUpdate
    {
        public InitialisationUpdate(InitialisationStages stage, string message)
        {
            Stage = stage;
            Message = message;
        }

        public InitialisationStages Stage { get; }
        public string Message { get; }
    }
}
