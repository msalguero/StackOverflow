using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Models;

namespace StackOverflow.Web
{
    public static class AutoFacConfig
    {

        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(c => Mapper.Engine).As<IMappingEngine>();
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutoMapperConfig.RegisterMaps();
        }
    }

    public static class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<Account, AccountLoginModel>().ReverseMap();
            Mapper.CreateMap<Account, AccountRegisterModel>().ReverseMap();
            Mapper.CreateMap<Account, AccountProfileModel>()
                .ForMember(dest => dest.QuestionsAsked, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Count))
                .ForMember(dest => dest.AnswerList, opt => opt.MapFrom(src => src.Answers.Take(5)))
                .ForMember(dest => dest.QuestionList, opt=> opt.MapFrom(src => src.Questions.Take(5)))
                .ReverseMap();
            Mapper.CreateMap<Account, ChangePasswordModel>().ReverseMap();
            Mapper.CreateMap<Account, ForgotPasswordModel>().ReverseMap();
            Mapper.CreateMap<Question, AskQuestionModel>().ReverseMap();
            Mapper.CreateMap<Question, QuestionDetailsModel>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ReverseMap();
            Mapper.CreateMap<Question, QuestionListModel>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AnswersCount, opt => opt.MapFrom(src => src.Answers.Count))
                .ReverseMap();
            Mapper.CreateMap<Answer, AnswerModel>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Question.Id))
                .ReverseMap();
            Mapper.CreateMap<Comment, CommentListModel>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
                .ReverseMap();
        }
    }
}