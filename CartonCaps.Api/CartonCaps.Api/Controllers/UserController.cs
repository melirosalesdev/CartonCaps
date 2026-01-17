using CartonCaps.Model;
using Microsoft.AspNetCore.Mvc;
using CartonCaps.Api.Service.Interface;

namespace CartonCaps.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets a user by email.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/users/maria@email.com
        ///
        /// Sample response:
        /// {
        ///   "id": "user_123",
        ///   "name": "Maria",
        ///   "lastName": "Gomez",
        ///   "email": "maria@email.com",
        ///   "referred": false,
        ///   "birthday": "1990-04-12",
        ///   "zipCode": "90210"
        /// }
        /// </remarks>
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult<User> GetByEmail(string email)
        {
            var user = _userService.GetByEmail(email);

            return Ok(user);
        }
    }
}
