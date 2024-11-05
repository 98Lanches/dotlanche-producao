using Microsoft.AspNetCore.Mvc;

namespace Dotlanche.Producao.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmptyController : ControllerBase
    {
        public EmptyController()
        {
        }

        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EmptyAction()
        {
            throw new NotImplementedException();
        }
    }
}