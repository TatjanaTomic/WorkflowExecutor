using Autofac;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Interface.DialogService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.DIBuilder
{
    public class ServiceContainer
    {
        private static ServiceContainer? instance;
        private readonly ContainerBuilder builder = new();
        private readonly ILifetimeScope scope;

        private ServiceContainer()
        {
            builder.RegisterType<WorkflowService.WorkflowService>().As<IWorkflowService>().SingleInstance();
            builder.RegisterType<WorkflowService.WorkflowRepository.WorkflowRepository>().As<WorkflowService.WorkflowRepository.IWorkflowRepository>().SingleInstance();
            builder.RegisterType<StatusReportService.StatusReportService>().As<IStatusReportService>().SingleInstance();
            builder.RegisterType<ExecutionService.ExecutionService>().As<IExecutionService>().SingleInstance();
            builder.RegisterType<DialogService.MessageService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<MainModel>().SingleInstance();
            IContainer containerBuilder = builder.Build();
            scope = containerBuilder.BeginLifetimeScope();
        }

        public static ServiceContainer Instance => instance ??= new ServiceContainer();

        public static T Resolve<T>() where T : class
        {
            return Instance.scope.Resolve<T>();
        }
    }
}
