using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Providers.Entities;
using AttributeRouting.Web.Http;
using AutoMapper;
using FizzWare.NBuilder;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

namespace MiniTrello.Api.Controllers
{
    public class OrganizationController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public OrganizationController(IMappingEngine mappingEngine, IWriteOnlyRepository writeOnlyRepository, IReadOnlyRepository readOnlyRepository)
        {
            _mappingEngine = mappingEngine;
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        [AcceptVerbs(new []{"DELETE"})]
         [DELETE("organization/{accessToken}")]
         public OrganizationModel Archive(string accessToken, [FromBody] OrganizationArchiveModel model)
         {
             var organization = _readOnlyRepository.GetById<Organization>(model.Id);
             var archivedOrganization = _writeOnlyRepository.Archive(organization);
             return _mappingEngine.Map<Organization, OrganizationModel>(archivedOrganization);
         }

        [GET("organization/{organizationId}/{accessToken}")]
         public OrganizationModel GetById(string accessToken, long organizationId)
         {
             var organization = _readOnlyRepository.GetById<Organization>(organizationId);
             return _mappingEngine.Map<Organization, OrganizationModel>(organization);
         }

        [POST("organization/addorganization/{accessToken}")]
        public OrganizationModel AddNewOrganization([FromBody] OrganizationModel model, string accessToken)
        {
            var newOrganization = _mappingEngine.Map<OrganizationModel, Organization>(model);
            var session = _readOnlyRepository.First<Sessions>(session1 => session1.Token == accessToken);
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
            session.User.AddOrganization(newOrganization);
            var organizationCreated = _writeOnlyRepository.Create(newOrganization);

            return new OrganizationModel{Description = organizationCreated.Description,Name = organizationCreated.Name};
        }

        [GET("organizations/{Token}")]
        public List<OrganizationModel> GetAllForUser(string Token)
        {
            var session = NewValidSession(Token);
            //obtener el usuario que pertenece al token
            //validar la session
            //var account = _readOnlyRepository.GetById<Account>(1);
            var mappedOrganizationModelList = _mappingEngine.Map<IEnumerable<Organization>,IEnumerable<OrganizationModel >> (session.User.Organizations).ToList();
            return mappedOrganizationModelList;
            //var organizations = Builder<OrganizationModel>.CreateListOfSize(10).Build().ToList();
            //return organizations;
        }

        

        public Sessions NewValidSession(string token)
        {

            var session = _readOnlyRepository.First<Sessions>(session1 => token == session1.Token);
            ValidateSession(session);
            return session;
        }

        public void ValidateSession(Sessions session)
        {
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
        }


    }
}
