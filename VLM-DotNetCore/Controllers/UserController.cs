using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLM.Core.Auth;
using VLM.Core.Entities;
using VLM.Core.Helpers;
using VLM.Core.Models;
using VLM.Data;

namespace VLM_DotNetCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly LinkGenerator link;

        public UserController(IMapper mapper, IUserService userService, LinkGenerator link)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.link = link;
        }

        // POST: authenticate user
        [AllowAnonymous]
        [HttpPost("Auth")]
        public async Task<ActionResult<AuthResponse>> Auth(AuthRequest req)
        {
            if ((req.Role == Roles.User) || (req.Role == Roles.Admin))
            {
                string checkCredentials = await userService.CheckUserCredentials(req);
                if(checkCredentials == "SUCCESS")
                {
                var response = await userService.AuthenticateAsync(req);
                return Ok(response);
                }
                return BadRequest(new { message = checkCredentials });
            }
            return Unauthorized();
        }

        // GET: all users
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IList<UserDTO>>> GetAll(bool showDeleted = false)
        {
            try
            {
                var users = await userService.GetAllUsersAsync(showDeleted);
                if (users == null) return NotFound(new { message = "NO_USERS" });
                var userModel = mapper.Map<IList<UserDTO>>(users);
                return Ok(userModel);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // GET: user with username
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetByUsername(string username)
        {
            try
            {
                var user = await userService.GetUserByUsernameAsync(username);
                if (user == null) return NotFound( new { message = "USER_NOT_EXIST" });
                var userModel = mapper.Map<UserDTO>(user);
                return Ok(userModel);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // POST: add new user
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuthResponse>> AddNewUser(Users newUserData)
        {
            try
            {
                var addedUserToken = await userService.AddUserAsync(newUserData);

                if (addedUserToken == null)
                {
                    return BadRequest(new { message = "USER_ALREADY_EXIST" });
                }

                if ((await userService.CommitAsync()) > 0)
                {
                    return Ok(addedUserToken);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }


        // PUT: edit user with username
        [HttpPut("{username}")]
        public async Task<ActionResult<UserDTO>> EditUser(UserDTO editUserData)
        {
            try
            {
                var userExist = await userService.GetUserByUsernameAsync(editUserData.UserName);
                if (userExist == null)
                {
                    return BadRequest(new { message = "USERNAME_OCCUPIED" });
                }

                var p = mapper.Map(editUserData,userExist);
                
                userService.EditUserAsync(userExist);
                
                var userModel = mapper.Map<UserDTO>(editUserData);

                if ((await userService.CommitAsync()) > 0) return Ok(userModel);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }


        // DELETE: delete user with username
        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            try
            {
                var userDeleted = await userService.DeleteUserAsync(username);
                if (userDeleted == false)
                {
                    return NotFound(new { message = "USER_NOT_EXIST" });
                }

                if ((await userService.CommitAsync()) > 0)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }
    }
}
