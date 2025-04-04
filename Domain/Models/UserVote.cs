using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserVote
    {
        [Key]
        public int UserVoteId { get; set; }

        [Required]
        public int PollFk { get; set; }

        [Required]
        public Guid UserFk { get; set; }

        public DateTime VoteDateCreated { get; set; }
    }
}
