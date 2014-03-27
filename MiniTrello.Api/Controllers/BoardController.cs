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
        public AccountBoardModel createNewBoard([FromBody] AccountBoardModel model,long IdOrganization, string Token)
        {
           var Board = _mappingEngine.Map<AccountBoardModel, Board>(model);
           var session = _readOnlyRepository.First<Sessions>(session1 => session1.Token == Token);
           Board.Administrator = session.User;
           if (session == null || !session.IsTokenActive())
               throw new BadRequestException("Session has expired. Please login again.");

           Organization organization = _readOnlyRepository.GetById<Organization>(IdOrganization);
           organization.AddBoard(Board);
           var organizacionUpdate = _writeOnlyRepository.Update(organization);
           var BoardCreated = _writeOnlyRepository.Create(Board);
           return new AccountBoardModel { Title = BoardCreated.Title, Id = BoardCreated.Id};
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
        [DELETE("deleteboard/{boardId}/{Token}")]
        public AccountBoardModel Archive(string Token, long boardId,[FromBody] BoardArchiveModel model)
        {
            var session = NewValidSession(Token);
            ValidateSession(session);
            var board = _readOnlyRepository.GetById<Board>(boardId);
            VerifyAdministrator(board.Administrator,session.User);
            var archivedBoard = _writeOnlyRepository.Archive(board);
            return _mappingEngine.Map<Board, AccountBoardModel>(archivedBoard);
        }

        public void VerifyAdministrator(Accounts administrator, Accounts user)
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

        [GET("getboards/{IdOrganization}/{Token}")]
        public List<AccountBoardModel> GetAllForUser(string Token,int IdOrganization)
         {
             var session = NewValidSession(Token);
            List<AccountBoardModel> AllBoards= new List<AccountBoardModel>();
            if (IdOrganization < 0)
            {
                var mappedOrganizationModelList = _mappingEngine.Map<IEnumerable<Organization>, IEnumerable<OrganizationModel>>(session.User.Organizations).ToList().Where(orga => !orga.IsArchived).ToList();
                foreach (var organizationModel in mappedOrganizationModelList)
                {
                    
                var _organization = _readOnlyRepository.GetById<Organization>(organizationModel.Id);
                    foreach (var board in _organization.Boards)
                    {
                        AllBoards.Add( new AccountBoardModel {Id = board.Id,Title = board.Title,IsArchived = board.IsArchived} );
                    }
                }
                return AllBoards;
            }
             var organization = _readOnlyRepository.GetById<Organization>(IdOrganization);
             var mappedBoardModelList = _mappingEngine.Map<IEnumerable<Board>,IEnumerable<AccountBoardModel>> (organization.Boards).ToList();
             //return mappedOrganizationModelList;
             return mappedBoardModelList.Where(board => !board.IsArchived).ToList();
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
        [POST("invitemember/{Token}/{BoardId}")]
        public AccountBoardModel InviteMembertoBoard(string Token, long BoardId, [FromBody] InviteMemberToBoardModel model)
        {
            var session = NewValidSession(Token);
            var board = _readOnlyRepository.GetById<Board>(BoardId);
            VerifyAdministrator(session.User, board.Administrator);
            var member = _readOnlyRepository.GetById<Accounts>(model.MemberId);
            if(member == null )
                throw new BadRequestException("Failed to add member to board: Member not Found");
            board.AddMember(member);
            //member.AddBoard(board);
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
