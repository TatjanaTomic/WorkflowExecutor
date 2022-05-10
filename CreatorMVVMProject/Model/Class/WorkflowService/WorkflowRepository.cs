using ExecutionEngine.Exceptions;
using ExecutionEngine.Xml;
using ExecutionEngine.Xml.StageListBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");
        private readonly string CONFIG_PATH = Path.Combine(BASE_PATH, "WorkflowConfig.xml");

        IList<Stage> IWorkflowRepository.GetAllStages()
        {
            var stagesList = StageListBuilder.GetStageList(CONFIG_PATH);

            IList<Stage> stages = new List<Stage>();
            if (stagesList != null && stagesList.Stages != null)
                stages = stagesList.Stages;

            return stages;
        }

    }
}
