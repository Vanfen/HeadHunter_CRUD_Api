using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techTask_IB.Candidates;
using techTask_IB.Enums;

namespace techTask_IB.DataModels
{
    public class CandidateSkill
    {
        public int Id { get; set; }
        public Skill Skill { get; set; }
    }

    public class CreateSkillset
    {
        public int CandidateId { get; set; }
        public List<Skill> Skills { get; set;}
    }

    public class DeleteSkillset
    {
        public int CandidateId { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
