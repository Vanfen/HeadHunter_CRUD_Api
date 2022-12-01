using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using techTask_IB.DataModels;

namespace techTask_IB.Candidates
{
    public class Application
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int PositionId { get; set; }
    }

    public class CreateApplication
    {
        public int CandidateId { get; set; }
        public int PositionId { get; set; }
    }
    public class DeleteApplication
    {
        public int ApplicationId { get; set; }
    }
}