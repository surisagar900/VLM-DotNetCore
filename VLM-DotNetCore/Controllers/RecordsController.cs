using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VLM.Core.Entities;
using VLM.Core.Helpers;
using VLM.Core.Models;
using VLM.Data;

namespace VLM_DotNetCore.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordsService recordsService;
        private readonly IMapper mapper;

        public RecordsController(IRecordsService recordsService, IMapper mapper)
        {
            this.recordsService = recordsService;
            this.mapper = mapper;
        }

        // GET: api/Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecordsDTO>>> GetRecords()
        {
            try
            {
                IEnumerable<Records> allRecords = await recordsService.GetAllRecordsAsync();
                if (allRecords == null) return NotFound();
                var recordsDTO = mapper.Map<IEnumerable<RecordsDTO>>(allRecords);
                return Ok(recordsDTO);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // GET: api/Records/5
        [HttpGet("{recordId:int}")]
        public async Task<ActionResult<Records>> GetRecordsByRecordId(int recordId)
        {
            try
            {
                Records record = await recordsService.GetRecordsByIdAsync(recordId);
                if(record == null) return NotFound();
                var recordDTO = mapper.Map<RecordsDTO>(record);
                return Ok(recordDTO);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

    }
}
