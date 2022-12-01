using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using techTask_IB.DataModels;
using techTask_IB.Enums;
using System.ComponentModel.DataAnnotations;

namespace techTask_IB.Candidates
{
    public class Candidate
    {
        public int Id { get; set; }
        [MinLength(2)]
        public string Name { get; set; }
        [MinLength(5)]
        public string Email { get; set; }
        public int Age { get; set; }
        public ICollection<CandidateSkill>? Skillset { get; set; }
        public ICollection<Application>? CandidateApplications { get; set; }

    }

    public class CreateCandidate
    {
        [MinLength(2)]
        public string CandidateName { get; set; }
        [MinLength(5)]
        public string CandidateEmail { get; set;}
        public int Age { get; set; }
    }

    public class UpdateCandidate
    {
        public int CandidateId { get; set; }
        [MinLength(5)]
        public string? CandidateEmail { get; set;}
        public int? Age { get; set; }
    }

    public class DeleteCandidate
    {
        public int CandidateId { get; set;}
    }
}
