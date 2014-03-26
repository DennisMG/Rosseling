using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Providers.Entities;
using AttributeRouting.Web.Http;
using AutoMapper;
using FizzWare.NBuilder;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;
using MiniTrello.Api.Controllers;

namespace MiniTrello.Api.Controllers
{
    public class LaneController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;
        

        public LaneController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository, IMappingEngine mappingEngine)
        {
            
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("createlane/{BoardId}/{token}")]
        public LaneModel CreateNewLane([FromBody] LaneModel model,string token,long BoardId)
        {
            var session = NewValidSession(token);
            ValidateSession(session);
            var board = _readOnlyRepository.GetById<Board>(BoardId);
            var newLane = new Lane{ Name = model.Name};
            board.AddLane(newLane);
            var boardUpdated = _writeOnlyRepository.Update(board);
            var controller = new CardController(_writeOnlyRepository, _readOnlyRepository, _mappingEngine);
            var laneCreated = _writeOnlyRepository.Create(newLane);
            if (laneCreated != null)
                return new LaneModel{ Id=laneCreated.Id, Name = laneCreated.Name, Cards  = controller.GetAllForUser(token, laneCreated.Id)};
            throw new BadRequestException("Failed creating new lane") ;
        }

        [GET("getlanes/{IdBoard}/{Token}")]
        public List<LaneModel> GetAllForUser(string Token, int IdBoard)
        {
            var session = NewValidSession(Token);
            ValidateSession(session);
            var board = _readOnlyRepository.GetById<Board>(IdBoard);
            List<LaneModel> mappedLaneModelList = _mappingEngine.Map<IEnumerable<Lane>, IEnumerable<LaneModel>>(board.Lanes).ToList();
            var controller = new CardController(_writeOnlyRepository, _readOnlyRepository, _mappingEngine);
            foreach (var lane in mappedLaneModelList)
            {
                
                //lane.Cards.Insert(0, new CardModel { Id = 0, Content = "Card" });
                lane.Cards = controller.GetAllForUser(Token, lane.Id);
                /*foreach (var card in lane.Cards)
                {
                    
                }*/

                //lane.
                //List<CardModel> cards = _mappingEngine.Map<IEnumerable<Card>, IEnumerable<CardModel>>(lane.Cards).ToList();

            }
            
            //return mappedLaneModelList;
            return mappedLaneModelList.Where(lane => !lane.IsArchived).ToList();
            //var lanes = Builder<LaneModel>.CreateListOfSize(10).Build().ToList();
            //return lanes;
            
        }

        [AcceptVerbs(new[] { "DELETE" })]
        [DELETE("deletelane/{laneId}/{Token}")]
        public LaneModel Archive(string Token, long laneId)
        {
            var session = NewValidSession(Token);
            ValidateSession(session);
            var lane = _readOnlyRepository.GetById<Lane>(laneId);
            
            var archivedBoard = _writeOnlyRepository.Archive(lane);
            return _mappingEngine.Map<Lane, LaneModel>(archivedBoard);
        }

        private void ValidateSession(Sessions session)
        {
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
        }

        public Sessions NewValidSession(string token)
        {
            return _readOnlyRepository.First<Sessions>(session1 => token == session1.Token);
        }

        public void VerifyAdministrator(Accounts administrator, Accounts user)
        {
            if (administrator != user)
                throw new BadRequestException("You don't enough privileges to do this");
        }
    }
}
