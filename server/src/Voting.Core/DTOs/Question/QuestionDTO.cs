using System;
using System.Collections.Generic;
using System.Text;
using Voting.Core.Entities;

namespace Voting.Core.DTOs.Question
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
        public int TotalCount { get; set; }

        public List<AnswerDTO> Answers { get; set; }
    }

    public class AnswerDTO
    {
        public string Answer { get; set; }
        public int Count { get; set; }
        public bool Voted { get; set; }
    }
}
