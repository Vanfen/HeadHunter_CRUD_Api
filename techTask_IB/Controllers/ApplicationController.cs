using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using techTask_IB.Candidates;
using techTask_IB.DataModels;
using techTask_IB.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace techTask_IB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ILogger<ApplicationController> logger)
        {
            _logger = logger;
        }


        [HttpPost("AddNewApplication")]
        public ActionResult AddNewApplication([FromBody] CreateApplication newApplication)
        {
            using (var context = new Context())
            {
                var candidateQuery = context.Candidates.Where(c => c.Id == newApplication.CandidateId);
                if (!candidateQuery.Any())
                {
                    return NotFound("There is no candidate with id " + newApplication.CandidateId.ToString());
                }
                var positionQuery = context.Positions.Where(p => p.Id == newApplication.PositionId);
                if (!positionQuery.Any())
                {
                    return NotFound("There is no position with id " + newApplication.PositionId.ToString());
                }
                if (candidateQuery.Any() && positionQuery.Any())
                {
                    var applicationQuery = context.Applications.Where(a => a.CandidateId == newApplication.CandidateId && a.PositionId == newApplication.PositionId);
                    if (!applicationQuery.Any())
                    {
                        var application = new Application()
                        {
                            CandidateId = newApplication.CandidateId,
                            PositionId = newApplication.PositionId,
                        };
                        context.Applications.Add(application); 
                        context.SaveChanges();
                        return Ok(application);
                    } else
                    {
                        return BadRequest("Application already exists.");
                    }                   
                }
            }
            return NotFound();
        }

        [HttpDelete("DeleteApplication")]
        public ActionResult DeleteApplication([FromBody] DeleteApplication toRemove)
        {
            using (var context = new Context())
            {
                if (toRemove.ApplicationId < 0)
                    return BadRequest();
                var application = context.Applications.Where(a => a.Id == toRemove.ApplicationId).FirstOrDefault();
                
                if (application != null)
                {
                    context.Remove(application);
                    context.SaveChanges(true);
                    return Ok();
                }
            }
            return NotFound("Couldn't find position with id " + toRemove.ApplicationId);
        }

        
    }
}
