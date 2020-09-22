using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Voting.Api.Models.Question
{
    public class AddQuestionViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Days { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public int Minutes { get; set; }
        [Required]
        public string[] Answers { get; set; }
    }
}
