using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagesController : ControllerBase
    {
        private readonly IManageUserService _manageUser;
        public ManagesController(IManageUserService manageUser)
        {
            _manageUser = manageUser;
        }

        [HttpGet("users")]

        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _manageUser.GetUsersAsync();
            return Ok(users);
        }

        [HttpPatch("block/{id}")]

        public async Task<IActionResult> BlockedUserAsync([FromRoute] string id)
        {
            var result = await _manageUser.BlockedUserAsync(id);
            return Ok(result);
        }

        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnBlockedUserAsync([FromRoute] string id) =>
            Ok(await _manageUser.UnBlockedUserAsync(id));

        [HttpPatch("change-role")]
        [Authorize(Roles = "superAdmin")]
        public async Task<IActionResult> ChangeUserRoleAsync([FromBody] ChangeUserRoleRequest request)
         => Ok(await _manageUser.ChangeUserRoleAsync(request));
    }
}
