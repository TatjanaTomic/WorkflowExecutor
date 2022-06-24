using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class ExecutionCompletedEventArgs
    {
        private Step step;
        private bool isSuccessful;
        private string message;

        public ExecutionCompletedEventArgs(Step step, bool isSuccessful, string message)
        {
            this.step = step;
            this.isSuccessful = isSuccessful;
            this.message = message;
        }

        public Step Step { get => step; set => step = value; }
        public bool IsSuccessful { get => isSuccessful; set => isSuccessful = value; }
        public string Message { get => message; set => message = value; }
    }
}
