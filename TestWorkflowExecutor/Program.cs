using ExecutionEngine.Executor;
using ExecutionEngine.Xml;
using ExecutionEngine.Xml.StageListBuilder;
using System.Xml.Serialization;

namespace TestExecutionEngine
{
    public class Program
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");
        private static readonly string STEP_PATH = Path.Combine(BASE_PATH, "testStep.xml");
        private static readonly string STAGE_PATH = Path.Combine(BASE_PATH, "testStage.xml");
        private static readonly string CONFIG_PATH = Path.Combine(BASE_PATH, "WorkflowConfig.xml");
       
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello");
            var stageList = ReadWorkflowConfig();
            Console.WriteLine();
            
            ExecuteAllSteps(stageList);

            Thread.Sleep(5000);
        }

        private static StageList? ReadWorkflowConfig()
        {

            var stageList = StageListBuilder.GetStageList(CONFIG_PATH);

            if (stageList != null && stageList.Stages != null)
            {
                foreach (var stage in stageList.Stages)
                {
                    Console.WriteLine("Stage: " + stage.Id);
                    if (stage.Steps != null)
                    {
                        foreach (var step in stage.Steps)
                        {
                            Console.WriteLine("  Step: " + step.Id);
                            Console.WriteLine("    Executable path: " + step.ExecutablePath);
                            Console.WriteLine("    File: " + step.File);
                            Console.WriteLine("    Type: " + step.Type);
                            Console.WriteLine("    Parallel: " + step.CanBeExecutedInParallel);
                            Console.WriteLine("    Description: " + step.Description);
                            if (step.Dependencies != null)
                            {
                                foreach (var dependency in step.Dependencies)
                                {
                                    Console.WriteLine("    Dependency: " + dependency.DependencyStep);
                                }
                            }
                            if (step.Parameters != null)
                            {
                                foreach (var parameter in step.Parameters)
                                {
                                    Console.WriteLine("    Parameters: " + parameter.KeyWord + " " + parameter.Value);
                                }
                            }
                        }
                    }
                }
            }

            return stageList;
        }

        private static void ExecuteAllSteps(StageList? stageList)
        {
            Console.WriteLine("Executing...");
            if(stageList != null && stageList.Stages != null)
            {
                foreach(var stage in stageList.Stages)
                {
                    //Console.WriteLine("  Stage: " + stage.Id);
                    if (stage.Steps != null)
                    {
                        foreach (var step in stage.Steps)
                        {
                            //Console.WriteLine("    Step: " + step.Id);
                            //ScriptExecutorService test = new(step);
                            //_ = test.Start();
                            AbstractExecutor? ex = ExecutorFabrique.Instance.CreateExecutor(step);
                            ex.Start();
                        }
                    }
                }
            }
        }

        private static void ReadStep()
        {
            using (var fileStream = File.Open(STEP_PATH, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Step));
                var myDocument = (Step)serializer.Deserialize(fileStream);
                Console.WriteLine($"Id : {myDocument.Id}");
                Console.WriteLine($"Type: {myDocument.Type}");
                foreach (var item in myDocument.Dependencies)
                {
                    Console.WriteLine(item.DependencyStep);
                }
            }
        }

        private static void ReadStage()
        {
            using (var fileStream = File.Open(STAGE_PATH, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stage));
                var myDocument = (Stage)serializer.Deserialize(fileStream);
                Console.WriteLine($"Stage: {myDocument.Id}");
                foreach (var item in myDocument.Steps)
                {
                    Console.WriteLine("Step: " + item.Id);
                    Console.WriteLine("    Executable path: " + item.ExecutablePath);
                    Console.WriteLine("    File: " + item.File);
                    Console.WriteLine("    Type: " + item.Type);
                    Console.WriteLine("    Parallel: " + item.CanBeExecutedInParallel);
                    Console.WriteLine("    Description: " + item.Description);
                    foreach(var dependency in item.Dependencies)
                    {
                        Console.WriteLine("    Dependency: " + dependency.DependencyStep);
                    }
                    foreach(var parameter in item.Parameters)
                    {
                        Console.WriteLine("    Parameters: " + parameter.KeyWord + " " + parameter.Value);
                    }
                }
            }
        }

        
    }
}