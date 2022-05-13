using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        //ovdje ide logika za queue stepova koji idu na izvrsavanje
        //zamisljeno je kao lista lista
        //stepovi koji se izvrsavaju paralelno idu u jednu listu
        //za svaki step koji se izvrsava sekvencijalno pravi se lista koja ima jedan element
        //odavde se koristi step executor
        //queue u sebi ima step statuse
    }
}
