namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StatusChangedEventArgs
    {
        public StatusChangedEventArgs(Status status, string stepId)
        {
            Status = status;
            StepId = stepId;
        }

        public Status Status { get; set; }

        public string StepId { get; set; }
    }
}
