using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Controllers;

public class CaseWorkerController : Controller
{
    // GET
    public IActionResult CaseWorkerPage()
    {
        return View("CaseWorkerPage-V2");
    }

    //// POST: Apply Filters
    //[HttpPost]
    //public IActionResult ApplyFilters(string category, bool option1, bool option2, bool option3, int range)
    //{
    //    // Logic to apply filters and fetch filtered data
    //    // For example, you might query the database with the provided filters

    //    // Return the filtered data to the view
    //    // This is just a placeholder; you would replace it with your actual logic
    //    var filteredData = new List<string>(); // Replace with actual data fetching logic

    //    return View("CaseWorkerPage", filteredData);
    //}
}