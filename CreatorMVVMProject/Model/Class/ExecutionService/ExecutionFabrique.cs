using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionFabrique
    {
        private static ExecutionFabrique? instance;

        private ExecutionFabrique()
        {

        }

        public static ExecutionFabrique Instance
        {
            get
            {
                return instance ??= new ExecutionFabrique();
            }
        }

    }
}
