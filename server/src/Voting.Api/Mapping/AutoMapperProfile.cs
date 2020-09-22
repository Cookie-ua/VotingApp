using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voting.Api.Models.Account;
using Voting.Api.Models.Question;
using Voting.Core.DTOs;
using Voting.Core.DTOs.Comment;
using Voting.Core.DTOs.User;
using Voting.Core.Entities;

namespace Voting.Api.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region QUESTION MAPPING
            
                CreateMap<AddQuestionViewModel, Question>()
                    .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateTime.Now.AddDays(src.Days).AddHours(src.Hours).AddMinutes(src.Minutes)));

                //CreateMap<VoteForQuestionViewModel, Vote>()
                //    .ForMember(dest => dest.VoteDate, opt => opt.MapFrom(src => DateTime.Now));

                CreateMap<AddQuestionViewModel, Question>()
                    .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => DateTime.Now.AddDays(src.Days).AddHours(src.Hours).AddMinutes(src.Minutes)))
                    .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Where(a => !string.IsNullOrWhiteSpace(a)).Select(x => new Answer() { Text = x }).ToList()));

            #endregion

            #region COMMENT MAPPING

            CreateMap<AddCommentDTO, Comment>()
                    .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => DateTime.Now));
            
            #endregion

            #region USER MAPPING

                CreateMap<RegisterViewModel, CreateUserDTOs>();
                CreateMap<User, UserDTO>();
            
            #endregion
        }
    }
}
