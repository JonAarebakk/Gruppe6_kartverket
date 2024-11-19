using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Gruppe6_Kartverket.Repository; 
using Gruppe6_Kartverket.Model.Database; 

namespace Gruppe6_Kartverket.Controllers
{
    public class UserController : Controller
    {
        private readonly DapperRepository _dapperRepository;

        public UserController(DapperRepository dapperRepository)
        {
            _dapperRepository = dapperRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userInfos = await _dapperRepository.GetUserInfosAsync();
            return View(userInfos);
        }
    }
}