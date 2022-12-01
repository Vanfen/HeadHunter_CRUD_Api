using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
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
    public class CandidateController : ControllerBase
    {
        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ILogger<CandidateController> logger)
        {
            _logger = logger;
        }


        [HttpPost("AddNewCandidate")]
        public ActionResult AddNewCandidate([FromBody] CreateCandidate newCandidate)
        {
            using (var context = new Context())
            {
                Candidate candidate = new Candidate()
                {
                    Name = newCandidate.CandidateName,
                    Email = newCandidate.CandidateEmail,
                    Age = newCandidate.Age,
                };
                var result = context.Candidates.AddIfNotPresent(candidate, x => x.Email == newCandidate.CandidateEmail);
                if (result != null)
                {
                    context.SaveChanges();
                    return Ok(result);
                } else
                {
                    return BadRequest("User with email " + newCandidate.CandidateEmail + " already exists.");
                }
            }
        }

        [HttpPatch("UpdateCandidate")]
        public ActionResult UpdateCandidate([FromBody] UpdateCandidate newCandidateInfo)
        {
            using (var context = new Context())
            {
                if (newCandidateInfo.Age == null && newCandidateInfo.CandidateEmail == null)
                    return Ok("Nothing changed");
                var query = context.Candidates.Where(c => c.Id == newCandidateInfo.CandidateId).Include(s => s.Skillset);
                if (query.Any())
                {
                    var candidate = query.First();
                    if (candidate.Age != newCandidateInfo.Age && newCandidateInfo.Age != null)
                        candidate.Age = (int)newCandidateInfo.Age;
                    if (candidate.Email != newCandidateInfo.CandidateEmail && newCandidateInfo.CandidateEmail != null)
                        candidate.Email = (string)newCandidateInfo.CandidateEmail;
                    context.SaveChanges();
                    return Ok(candidate);
                }
            }
            return NotFound("User with id: " + newCandidateInfo.CandidateId + " not found.");
        }

        [HttpDelete("DeleteCandidate")]
        public ActionResult DeleteCandidate([FromBody] DeleteCandidate toRemove)
        {
            using (var context = new Context())
            {
                if (toRemove.CandidateId < 0)
                    return BadRequest();
                var candidate = context.Candidates.Where(c => c.Id == toRemove.CandidateId).Include(s => s.Skillset).FirstOrDefault();
                CandidateSkill[]? skills = null;
                if (candidate?.Skillset != null) {
                    skills = new CandidateSkill[candidate.Skillset.Count];
                    int i = 0;
                    foreach (var skill in candidate.Skillset)
                    {
                        skills[i] = skill;
                        i++;
                    }
                }
                if (candidate != null)
                {
                    context.Remove(candidate);
                    if (skills != null)
                        foreach (var skill in skills)
                        {
                            context.Remove(skill);
                        }
                    context.SaveChanges(true);
                    return Ok();
                }
            }
            return NotFound("Couldn't find candidate with id " + toRemove.CandidateId);
        }

        [HttpPost("AddNewSkills")]
        public ActionResult AddNewSkills([FromBody] CreateSkillset newSkillset)
        {
            using (var context = new Context())
            {
                var candidate = context.Candidates.Where(c => c.Id == newSkillset.CandidateId).Include(c => c.Skillset).FirstOrDefault();
                if (candidate != null)
                {
                foreach (var skill in newSkillset.Skills)
                    {
                        var candidateSkill = new CandidateSkill()
                        {
                            Skill = skill,
                        };
                        if (candidate.Skillset == null)
                        {
                            candidate.Skillset = new List<CandidateSkill>() { 
                                new CandidateSkill()
                                {
                                    Skill = skill,
                                }
                            };
                        } else if (!candidate.Skillset.Contains(candidateSkill))
                        {
                            candidate.Skillset.Add(candidateSkill);
                        }
                    }
                    context.SaveChanges(true);
                    return Ok(candidate);
                } else
                {
                    return NotFound("No candidate with id " + newSkillset.CandidateId.ToString() + " found.");
                }
            }
        }

        [HttpDelete("DeleteSkills")]
        public ActionResult DeleteSkills([FromBody] DeleteSkillset toRemove)
        {
            using (var context = new Context())
            {
                if (toRemove.Skills.Count <= 0)
                    return BadRequest("Please enter any skills.");
                var candidate = context.Candidates.Where(c => c.Id == toRemove.CandidateId).Include(c => c.Skillset).FirstOrDefault();
                if (candidate != null)
                {
                    if (candidate.Skillset == null)
                        return NotFound("Candidate doesn't have any skills.");
                    foreach (var skill in toRemove.Skills)
                    {
                        var skillToRemove = candidate.Skillset.Where(s => s.Skill == skill).FirstOrDefault();
                        if (skillToRemove != null)
                        {
                            context.Remove(skillToRemove);
                        }
                    }
                    context.SaveChanges(true);
                    return Ok(candidate);
                }
                else
                {
                    return NotFound("No candidate with id " + toRemove.CandidateId.ToString() + " found.");
                }
            }
        }
    }
}
