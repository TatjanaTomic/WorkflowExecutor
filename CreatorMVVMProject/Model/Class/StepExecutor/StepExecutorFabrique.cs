using CreatorMVVMProject.Model.Class.Exceptions;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using Type = CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.Type;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public static class StepExecutorFabrique
    {
        public static AbstractExecutor CreateExecutor(Step step)
        {
            return step.Type switch
            {
                Type.Executable => new ScriptExecutor(step),
                Type.Upload => new UploadExecutor(step),
                Type.Download => new DownloadExecutor(step),
                _ => throw new WrongDefinitionException("Step type must be defined.")
            };
        }
    }
}
