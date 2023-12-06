using Microsoft.AspNetCore.Mvc;
using WebApi.CustomValidator.Validators;


namespace WebApiRouting.Controllers;

[ApiController]
[Route("[controller]")]
public class RoutingController : ControllerBase
{

    public class UserPin
    {
        [PinLengthRange(4)]
        public int Pin { get; set; }
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult PostUserPin([FromBody] UserPin request)
    {
        if(request == null)
        {
            return BadRequest("Invalid request.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(new { Message = "User PIN is valid.", request.Pin });
    }
}