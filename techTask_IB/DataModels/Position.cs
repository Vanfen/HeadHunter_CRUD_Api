using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techTask_IB.Candidates;

namespace techTask_IB.DataModels
{
    public class Position
    {
        public int Id { get; set; }
        [MinLength(2)]
        public string Title { get; set; }
        [MinLength(5)]
        public string Description { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }

    public class CreatePosition
    {
        public int CompanyId { get; set; }
        [MinLength(2)]
        public string Title { get; set; }
        [MinLength(5)]
        public string Description { get; set; }
    }

    public class UpdatePosition
    {
        public int PositionId { get; set; }
        [MinLength(2)]
        public string? Title { get; set; }
        [MinLength(5)]
        public string? Description { get; set; }
    }

    public class DeletePosition
    {
        public int PositionId { get; set;}
    }
}
