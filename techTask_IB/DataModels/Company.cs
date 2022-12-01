using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techTask_IB.Candidates;
using techTask_IB.Enums;

namespace techTask_IB.DataModels
{
    public class Company
    {
        public int Id { get; set; }
        [MinLength(1)]
        public string Name { get; set; }
        public string? CompanyUrl { get; set; }
        public ICollection<Position>? Positions { get; set; }
    }

    public class CreateCompany
    {
        [MinLength(2)]
        public string CompanyName { get; set; }
        public string CompanyUrl { get; set;}
    }

    public class UpdateCompany
    {
        public int CompanyId { get; set; }
        [MinLength(2)]
        public string? CompanyName { get; set; }
        public string? CompanyUrl { get; set; }
    }

    public class DeleteCompany
    {
        public int CompanyId { get; set;}
    }
}
