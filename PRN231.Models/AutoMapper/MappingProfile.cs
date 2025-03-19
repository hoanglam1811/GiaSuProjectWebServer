using AutoMapper;
using PRN231.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();

            CreateMap<Credential, CredentialDTO>();
            CreateMap<CredentialDTO, Credential>();

            CreateMap<SubjectDTO, Subject>();
            CreateMap<Subject, SubjectDTO>();

            CreateMap<Level, LevelDTO>();
            CreateMap<LevelDTO, Level>();

            CreateMap<ScheduleDTO, Schedule>();
            CreateMap<Schedule, ScheduleDTO>();

            CreateMap<FeedbackDTO, Feedback>();
            CreateMap<Feedback, FeedbackDTO>();

            CreateMap<PostDTO, Post>();
            CreateMap<Post, PostDTO>();

            CreateMap<PostRating, PostRatingDTO>();
            CreateMap<PostRatingDTO, PostRating>();

            CreateMap<TransactionDTO, Transaction>();
            CreateMap<Transaction, TransactionDTO>();

        }
    }
}
