using Gruppe6_Kartverket.Mvc.Models.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruppe6_Kartverket.Mvc.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomValueController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var random = new Random();
            var value = random.Next(0, 100);
            return Ok(value);
        }

        public IActionResult Post(ApiModel model)
        {
            model.Message = $"Hello from the server! {model.RandomNumber}";
            return Ok(model);
        }
    }


}
