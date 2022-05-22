using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public enum Status
    {
        Disabled,
        NotStarted,
        Waiting,
        InProgress,
        Success,
        Failed,
        Obsolete,
    }

    /*
     * Disabled - ne može se startati jer se njegov zavisni korak nije izvršio
     * NotStarted - može se startati
     * Waiting - stavljen u red, ceka na izvrsavanje
     * InProgress - trenutno se izvršava
     * Success - uspješno izvršen
     * Failed - neuspješno izvršavanje
     * Obsolete - primjer: Step3 zavisi od Step2, a Step2 zavisi od Step 1 i svi su uspješno izvršeni.
     *                     Ukoliko korinisk ponovi izvrši Step1, Step2 i Step3, tj. svi zavisni koraci, prelaze u ovo stanje
     * */
}
