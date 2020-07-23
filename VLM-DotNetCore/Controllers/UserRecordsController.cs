using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VLM.Core.Entities;
using VLM.Core.Helpers;
using VLM.Core.Models;
using VLM.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VLM_DotNetCore.Controllers
{
    [Authorize]
    [Route("api/user/{username}/records")]
    [ApiController]
    public class UserRecordsController : ControllerBase
    {
        private readonly IRecordsService recordsService;
        private readonly LinkGenerator link;
        private readonly IMapper mapper;

        public UserRecordsController(IRecordsService recordsService, LinkGenerator link, IMapper mapper)
        {
            this.recordsService = recordsService;
            this.link = link;
            this.mapper = mapper;
        }

        //// GET: api/user/{username}/records
        // get all users records by username
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRecordDTO>>> GetUserRecords(string username)
        {
            try
            {
                var userRecords = await recordsService.GetRecordsByUsernameAsync(username);
                if(userRecords == null)
                    return NotFound(new { message = "NO_USERRECORDS" });
                var userRecordsDTO = mapper.Map<IEnumerable<UserRecordDTO>>(userRecords);
                return Ok(userRecordsDTO);
            }
            catch(Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        //// GET: api/user/{username}/records/{recordId}
        // get user record by recordId
        [HttpGet("{recordId:int}")]
        public async Task<ActionResult<UserRecordDTO>> GetUserRecordsById(string username, int recordId)
        {
            try
            {
                var userRecords = await recordsService.GetUserRecordsByIdAsync(username,recordId);
                if(userRecords == null)
                    return NotFound(new { message = "USERRECORD_NOT_EXIST" });
                var userRecordsDTO = mapper.Map<UserRecordDTO>(userRecords);
                return Ok(userRecordsDTO);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        //// POST: api/user/{username}/records
        // Add new record 
        [HttpPost]
        public async Task<ActionResult<RecordsDTO>> AddRecordEntry(string username,RecordEntryModel addRecord)
        {
            try
            {
                if(username != addRecord.Username) return BadRequest();

                Records record = await recordsService.AddRecordAsync(addRecord);
                if(record == null)
                    return BadRequest();

                var location = link.GetPathByAction("GetUserRecordsById", "UserRecords", new { username = record.User.UserName, recordId = record.RecordsId });
                if(string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest();
                }
                //var recordDTO = mapper.Map<RecordsDTO>(record);

                if((await recordsService.CommitAsync()) > 0)
                {
                    return Created(location, record);
                }
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

        // DELETE : Record
       [HttpDelete("{recordId:int}")]
        public async Task<ActionResult<RecordsDTO>> DeleteRecord(string username, int recordId)
        {
            try
            {
                var userDeleted = await recordsService.DeleteRecordAsync(username,recordId);
                if(userDeleted == false)
                {
                    return NotFound(new { message = "USERRECORD_NOT_EXIST" });
                }

                if((await recordsService.CommitAsync()) > 0)
                {
                    return Ok();
                }
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            return BadRequest();
        }

    }
}
