using Autofac;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.DIBuilder
{
    public class ServiceContainer
    {
        private static ServiceContainer? instance;
        private readonly ContainerBuilder builder = new();
        private readonly IContainer containerBuilder;
        private readonly ILifetimeScope scope;
        private ServiceContainer()
        {
            builder.RegisterType<WorkflowService.WorkflowService>().As<IWorkflowService>().SingleInstance();
            builder.RegisterType<WorkflowService.WorkflowRepository.WorkflowRepository>().As<WorkflowService.WorkflowRepository.IWorkflowRepository>().SingleInstance();
            builder.RegisterType<StatusReportService.StatusReportService>().As<IStatusReportService>().SingleInstance();
            builder.RegisterType<ExecutionService.ExecutionService>().As<IExecutionService>().SingleInstance();
            builder.RegisterType<MainModel>().SingleInstance();
            this.containerBuilder = builder.Build();
            this.scope = this.containerBuilder.BeginLifetimeScope();
        }
        public static T Resolve<T>() where T: class
        {
            return Instance.scope.Resolve<T>();
        }
        public static ServiceContainer Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ServiceContainer();
                }
                return instance;
            }
        }
        
    }
}
