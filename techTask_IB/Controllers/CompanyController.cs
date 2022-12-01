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
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }


        [HttpPost("AddNewCompany")]
        public ActionResult AddNewCompany([FromBody] CreateCompany newCompany)
        {
            using (var context = new Context())
            {
                Company company = new Company()
                {
                    Name = newCompany.CompanyName,
                    CompanyUrl = newCompany.CompanyUrl,
                };
                var result = context.Companies.AddIfNotPresent(company, x => x.Name == newCompany.CompanyName);
                if (result != null)
                {
                    context.SaveChanges();
                    return Ok(result);
                } else
                {
                    return BadRequest("Company with name " + newCompany.CompanyName + " already exists.");
                }
            }
        }

        [HttpPatch("UpdateCompany")]
        public ActionResult UpdateCompany([FromBody] UpdateCompany newCompanyInfo)
        {
            using (var context = new Context())
            {
                if (newCompanyInfo.CompanyName == null && newCompanyInfo.CompanyUrl == null)
                    return Ok("Nothing changed");
                var query = context.Companies.Where(c => c.Id == newCompanyInfo.CompanyId).Include(p => p.Positions);
                if (query.Any())
                {
                    var company = query.First();
                    if (newCompanyInfo.CompanyName == company.Name && newCompanyInfo.CompanyUrl == company.CompanyUrl)
                        return Ok("Nothing changed");
                    if (company.Name != newCompanyInfo.CompanyName && newCompanyInfo.CompanyName != null)
                        company.Name = newCompanyInfo.CompanyName;
                    if (company.CompanyUrl != newCompanyInfo.CompanyUrl && newCompanyInfo.CompanyUrl != null)
                        company.CompanyUrl = newCompanyInfo.CompanyUrl;
                    
                    context.SaveChanges();
                    return Ok(company);
                }
            }
            return NotFound("Company with id: " + newCompanyInfo.CompanyId + " not found.");
        }

        [HttpDelete("DeleteCompany")]
        public ActionResult DeleteCompany([FromBody] DeleteCompany toRemove)
        {
            using (var context = new Context())
            {
                if (toRemove.CompanyId < 0)
                    return BadRequest();
                var company = context.Companies.Where(c => c.Id == toRemove.CompanyId).Include(p => p.Positions).FirstOrDefault();
                Position[] positions = null;
                if (company?.Positions != null) {
                    positions = new Position[company.Positions.Count];
                    int i = 0;
                    foreach (var position in company.Positions)
                    {
                        positions[i] = position;
                        i++;
                    }
                }
                if (company != null)
                {
                    context.Remove(company);
                    if (positions != null)
                        foreach (var position in positions)
                        {
                            context.Remove(position);
                        }
                    context.SaveChanges(true);
                    return Ok();
                }
            }
            return NotFound("Couldn't find company with id " + toRemove.CompanyId);
        }
        [HttpPost("AddNewPosition")]
        public ActionResult AddNewPosition([FromBody] CreatePosition newPosition)
        {
            using (var context = new Context())
            {
                var query = context.Companies.Where(c => c.Id == newPosition.CompanyId).Include(p => p.Positions);
                if (query.Any())
                {
                    var company = query.First();
                    var position = company.Positions.FirstOrDefault(p => p.Title == newPosition.Title);
                    if (position == null)
                    {
                        company.Positions.Add(new Position()
                        {
                            Title = newPosition.Title,
                            Description = newPosition.Description,
                        });
                        context.SaveChanges();
                        return Ok(company);
                    }
                    else
                    {
                        return BadRequest("Position with title " + newPosition.Title + " already exists.");
                    }
                }
                else
                {
                    return NotFound("No company with id " + newPosition.CompanyId.ToString() + " found");
                }
            }
        }

        [HttpPatch("UpdatePosition")]
        public ActionResult UpdatePosition([FromBody] UpdatePosition newPositionInfo)
        {
            using (var context = new Context())
            {
                if (newPositionInfo.Title == null && newPositionInfo.Description == null)
                    return Ok("Nothing changed");
                var query = context.Positions.Where(p => p.Id == newPositionInfo.PositionId);
                if (query.Any())
                {
                    var position = query.First();
                    if (newPositionInfo.Title == position.Title && newPositionInfo.Description == position.Description)
                        return Ok("Nothing changed");
                    if (position.Title != newPositionInfo.Title && newPositionInfo.Title != null)
                        position.Title = newPositionInfo.Title;
                    if (position.Description != newPositionInfo.Description && newPositionInfo.Description != null)
                        position.Description = newPositionInfo.Description;
                    context.SaveChanges();
                    return Ok(position);
                }
            }
            return NotFound("Position with id: " + newPositionInfo.PositionId + " not found.");
        }

        [HttpDelete("DeletePosition")]
        public ActionResult DeletePosition([FromBody] DeletePosition toRemove)
        {
            using (var context = new Context())
            {
                if (toRemove.PositionId < 0)
                    return BadRequest();
                var position = context.Positions.Where(p => p.Id == toRemove.PositionId).Include(a => a.Applications).FirstOrDefault();
                Application[]? applications = null;
                if (position?.Applications != null)
                {
                    applications = new Application[position.Applications.Count];
                    int i = 0;
                    foreach (var application in position.Applications)
                    {
                        applications[i] = application;
                        i++;
                    }
                }
                if (position != null)
                {
                    context.Remove(position);
                    if (applications != null)
                        foreach (var application in applications)
                        {
                            context.Remove(application);
                        }
                    context.SaveChanges(true);
                    return Ok();
                }
            }
            return NotFound("Couldn't find position with id " + toRemove.PositionId);
        }

    }
}
