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
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;
using MiniTrello.Api.Models;

namespace MiniTrello.Api.Controllers
{
    public class BoardController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public BoardController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

       [POST("createBoard/{IdOrganization}/{Token}")]
        public AccountBoardModel createNewBoard([FromBody] AccountBoardModel model,long IdOrganization,string Token)
        {
           var session = NewValidSession(Token);
           Organization organization = _readOnlyRepository.GetById<Organization>(IdOrganization);
           var newBoard = new Board {Title = model.Title, Administrator = session.User, IsArchived = false};
           organization.AddBoard(newBoard);
           var organizacionUpdate = _writeOnlyRepository.Update(organization);
           //var accountUpdate = _writeOnlyRepository.Update(session);
           //session.User.AddBoard(newBoard);
           var BoardCreated = _writeOnlyRepository.Create(newBoard);
           return new AccountBoardModel { Title = BoardCreated.Title};
        }

        public void ValidateSession(Sessions session)
        {
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
        }

        [AcceptVerbs("PUT")]
        [PUT("ChangeBoardName/{IdBoard}/{Token}")]
        public ChangeBoardNameModel ChangeBoardName([FromBody] ChangeBoardNameModel model, long IdBoard, string Token)
        {
            var session = NewValidSession(Token);
            var board = _readOnlyRepository.GetById<Board>(IdBoard);
            VerifyAdministrator(board.Administrator, session.User);
            board.Title = model.Title;
            var BoardUpdated = _writeOnlyRepository.Update(board);
            return new ChangeBoardNameModel{Title = BoardUpdated.Title};

        }

        [AcceptVerbs(new[] { "DELETE" })]
        [DELETE("deleteboard/{Token}")]
        public AccountBoardModel Archive(string Token, [FromBody] BoardArchiveModel model)
        {
            var session = NewValidSession(Token);
            var board = _readOnlyRepository.GetById<Board>(model.Id);
            VerifyAdministrator(board.Administrator,session.User);
            var archivedBoard = _writeOnlyRepository.Archive(board);
            return _mappingEngine.Map<Board, AccountBoardModel>(archivedBoard);
        }

        public void VerifyAdministrator(Account administrator, Account user)
        {
            if (administrator != user)
                throw new BadRequestException("You don't have the privileges to do this");
        }

        [GET("boards/{Token}/{BoardId}")]
        public BoardGetModel GetById(string Token, long BoardId)
        {
            var session = NewValidSession(Token);
            var board = _readOnlyRepository.GetById<Board>(BoardId);
            if (board != null)
            {
                BoardGetModel boardRetrieved = _mappingEngine.Map<Board, BoardGetModel>(board);
                boardRetrieved.Administrator = session.User.Email;
                return boardRetrieved;
            }
                
            throw new BadRequestException("Couldn't retrieve board info");
        }

        [GET("getboards/{Token}")]
        public List<AccountBoardModel> GetAllForUser(string Token)
         {
             var session = NewValidSession(Token);
             //var account = _readOnlyRepository.GetById<Account>(1);
             var mappedOrganizationModelList = _mappingEngine.Map<IEnumerable<Board>,IEnumerable<AccountBoardModel>> (session.User.Boards).ToList();
             return mappedOrganizationModelList;
             //var boards = Builder<AccountBoardModel>.CreateListOfSize(10).Build().ToList();
             //return boards;
         }

        public Sessions NewValidSession(string token)
        {
            
            var session = _readOnlyRepository.First<Sessions>(session1 => token == session1.Token);
            ValidateSession(session);
            return session;
        }

       // [AcceptVerbs(new[] {"PUT"})]
        [POST("boards/{Token}/{BoardId}")]
        public AccountBoardModel InviteMembertoBoard(string Token, long BoardId, [FromBody] InviteMemberToBoardModel model)
        {
            var session = NewValidSession(Token);
            var board = _readOnlyRepository.GetById<Board>(BoardId);
            VerifyAdministrator(session.User, board.Administrator);
            var member = _readOnlyRepository.GetById<Account>(model.MemberId);
            if(member == null )
                throw new BadRequestException("Failed to add member to board: Member not Found");
            board.AddMember(member);
            member.AddBoard(board);
            var boardUpdated = _writeOnlyRepository.Create(board);
            var memberUpdated = _writeOnlyRepository.Update(member);
            var NewMember = _mappingEngine.Map<Board, AccountBoardModel>(boardUpdated);
            if(boardUpdated!=null )
                return NewMember;
            throw new BadRequestException("Failed to add member to board");
        }

    }

    public class InviteMemberToBoardModel
    {
        public long MemberId;
    }
}
