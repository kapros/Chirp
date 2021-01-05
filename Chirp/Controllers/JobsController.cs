using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chirp.Data;
using Chirp.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly AcceptedJobsContext _acceptedJobsContext;
        private readonly UserId _userId;

        public JobsController(AcceptedJobsContext acceptedJobsContext, UserId userId)
        {
            _acceptedJobsContext = acceptedJobsContext;
            _userId = userId;
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get([FromRoute] string jobId)
        {
            var job = await _acceptedJobsContext.Jobs.FirstOrDefaultAsync(x => x.JobId == jobId);
            if (job.UserId != _userId.Id)
                return Unauthorized(new { Status = "Unauthorized", Error = "You may not view this job" });
            if (!job.DateFinished.HasValue)
                return Ok(new { Status = "Pending" });
            return Ok(new
            {
                Status = "Finished",
                ResourceId = job.CreatedObjectId
            });
        }
    }
}
