using Gruppe6_Kartverket.Mvc.Models.Database;  // Import the namespace where CaseRecord is defined

namespace Gruppe6_Kartverket.Mvc.Models.ViewModels
{
    public class CaseWorkerPageV2ViewModel
    {
        public List<CaseRecord> CaseRecords { get; set; }
    }
}