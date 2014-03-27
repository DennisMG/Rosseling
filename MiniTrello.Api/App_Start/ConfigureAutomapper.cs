using System;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Infrastructure;

namespace MiniTrello.Api
{
    public class ConfigureAutomapper : IBootstrapperTask
    {
        public void Run()
        {
            Mapper.CreateMap<Accounts, AccountLoginModel>().ReverseMap();
            Mapper.CreateMap<Accounts, AccountRegisterModel>().ReverseMap();
            Mapper.CreateMap<Organization, OrganizationModel>().ReverseMap();
            Mapper.CreateMap<Board, AccountBoardModel>().ReverseMap();
            Mapper.CreateMap<Accounts, UpdateAccountModel>().ReverseMap();
            Mapper.CreateMap<Board, BoardGetModel>().ReverseMap();
            Mapper.CreateMap<Card, CardModel>().ReverseMap();
            Mapper.CreateMap<Board, AccountBoardModel>().ReverseMap();
            Mapper.CreateMap<Card, CardArchiveModel>().ReverseMap();
            Mapper.CreateMap<Card, CardModel>().ReverseMap();
            Mapper.CreateMap<CardModel, Card>().ReverseMap();
            Mapper.CreateMap<Lane, LaneModel>().ReverseMap();
            Mapper.CreateMap<Card, CardArchiveModel>().ReverseMap();
            

            //Mapper.CreateMap<DemographicsEntity, DemographicsModel>().ReverseMap();
            //Mapper.CreateMap<IReportEntity, IReportModel>()
            //    .Include<DemographicsEntity, DemographicsModel>();
        }
    }
}