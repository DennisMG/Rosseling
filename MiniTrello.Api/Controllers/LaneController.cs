using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Providers.Entities;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

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

        [POST("lane/createlane/{BoardId}/{token}")]
        public HttpResponseMessage CreateNewLane([FromBody] LaneModel model,string token,long BoardId)
        {
            var session = NewValidSession(token);
            ValidateSession(session);
            var board = _readOnlyRepository.GetById<Board>(BoardId);
            VerifyAdministrator(session.User, board.Administrator);
            var newLane = new Lane{ Name = model.Name};
            board.AddLane(newLane);
            var LaneCreated = _writeOnlyRepository.Create(newLane);
            if (LaneCreated != null)
                return new HttpResponseMessage(HttpStatusCode.OK);
            throw new BadRequestException("Failed creating new lane") ;
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

        public void VerifyAdministrator(Account administrator, Account user)
        {
            if (administrator != user)
                throw new BadRequestException("You don't enough privileges to do this");
        }
    }
}
