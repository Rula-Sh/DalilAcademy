using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // anyone can login
    public class AuthController(IAuthService auth) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto) =>
            Ok(await auth.LoginAsync(dto));

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> RegisterStudent(RegisterStudentDto dto)
        {
            var created = await auth.RegisterStudentAsync(dto);
            return Ok(created);
        }
    }
}
