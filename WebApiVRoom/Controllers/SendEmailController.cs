using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Helpers;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : Controller
    {
        [HttpGet("sendemail/{username}/{useremail}/{text}")]
        public async Task<ActionResult> SendlMessage(string username, string useremail,string text)
        {
            SendEmailHelper.SendHelpMessage(username, useremail, text);
            return Ok();
        }
    }
}
