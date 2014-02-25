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

        [GET("organization/{accessToken}/{organizationId}")]
         public OrganizationModel GetById(string accessToken, long organizationId)
         {
             var organization = _readOnlyRepository.GetById<Organization>(organizationId);
             return _mappingEngine.Map<Organization, OrganizationModel>(organization);
         }

        [POST("organization/addorganization/{accessToken}")]
        public HttpResponseMessage AddNewOrganization([FromBody] OrganizationModel model, string accessToken)
        {
            var newOrganization = _mappingEngine.Map<OrganizationModel, Organization>(model);
            var session = _readOnlyRepository.First<Sessions>(session1 => session1.Token == accessToken);
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
            session.User.AddOrganization(newOrganization);
            var organizationCreated = _writeOnlyRepository.Create(newOrganization);

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }


    }
}
