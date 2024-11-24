using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;

namespace Gruppe6_Kartverket.Mvc.Controllers
{
    public class CaseInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CaseInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CaseInfo(int caseRecordId)
        {
            var caseRecord = await _context.CaseRecords
                .Include(cr => cr.CaseLocation)
                .Include(cr => cr.User)
                .FirstOrDefaultAsync(cr => cr.CaseRecordId == caseRecordId);

            if (caseRecord == null)
            {
                return NotFound();
            }

            var viewmodel = new CaseDetailsViewModel
            {
                CaseRecordId = caseRecord.CaseRecordId,
                CaseDate = caseRecord.CaseDate,
                CaseTitle = caseRecord.CaseTitle,
                CaseIssueType = caseRecord.CaseIssueType,
                CaseDescription = caseRecord.CaseDescription,
                CaseStatus = caseRecord.CaseStatus,
                LocationId = caseRecord.LocationId,
                GeoJSON = caseRecord.CaseLocation?.GeoJSON,
                CaseLocation = caseRecord.CaseLocation,
                User = caseRecord.User
            };
            return View(viewmodel);
        }
    }
}