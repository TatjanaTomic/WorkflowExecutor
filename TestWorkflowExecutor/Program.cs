using System.Xml.Serialization;

namespace TestExecutionEngine
{
    public class Program
    {
        private static readonly string BASE_PATH = @"C:\Users\EC\Desktop\WorkflowExecutorTest";
        private static readonly string TEST_PATH = Path.Combine(BASE_PATH, "testStep.xml");
        public static void Main(string[] args)
        {
            using (var fileStream = File.Open(TEST_PATH, FileMode.Open))
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
    }
}