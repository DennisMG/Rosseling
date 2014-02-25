using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Providers.Entities;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

using Session = System.Web.Providers.Entities.Session;

namespace MiniTrello.Api.Controllers
{
    public class AccountController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("login")]
        public AuthenticationModel Login([FromBody] AccountLoginModel model)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Email == model.Email && account1.Password == model.Password);
            if (account != null)
            {
                var newSession = new Sessions
                {
                    User = account,
                    ExpireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).AddHours(24), 
                    LoginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                    Token = Guid.NewGuid().ToString()
                };
                
                var sessionCreated = _writeOnlyRepository.Create(newSession);
                
                return new AuthenticationModel(){Token = sessionCreated.Token};
            }
            
            throw new BadRequestException(
                "Usuario o clave incorrecto");
        }

        [POST("register")]
        public AccountRegisterModel Register([FromBody] AccountRegisterModel model)
        {
            Account accountCreated = null;
            if (PasswordIsValid(model.Password, model.ConfirmPassword) && EmailIsValid(model.Email))
            {
                Account account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                accountCreated = _writeOnlyRepository.Create(account);
            }

            
            if (accountCreated != null)
            {
                return model;
            }
            throw new BadRequestException("Hubo un error al guardar el usuario");
        }

        private bool PasswordIsValid(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new BadRequestException("Claves no son iguales");
            }

            if (password.Any(char.IsDigit) && password.Length > 5)
            {
                return true;
            }
            throw new BadRequestException("La clave debe contener por lo menos 1 numero y debe ser de 6 caracteres o mas.");
        }

        [POST("/Account/ChangePassword/{token}")]
        public AccountLoginModel ChangePasword([FromBody] AccountChangePassword model, string token)
        {
            var session = _readOnlyRepository.First<Sessions>(session1 => session1.Token == token );
            if (session != null && PasswordIsValid(model.ConfirmNewPassword,model.NewPassword))
            {
                session.User.Password = model.NewPassword;
                var accountUpdated = _writeOnlyRepository.Update(session);
                var newModel= new AccountLoginModel {Email = accountUpdated.User.Email, Password = accountUpdated.User.Password};
                return newModel;
            }
            throw new BadRequestException("Hubo un error al cambiar de Password"); 
        }

        [POST("/Account/ForgotPassword")]
        public AccountLoginModel ForgotPasword([FromBody] AccountForgotPasswordModel model)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Email == model.Email);
           
            if (PasswordIsValid(model.NewPassword,model.ConfirmNewPassword))
            {
                account.Password = model.NewPassword;
                var accountUpdated = _writeOnlyRepository.Update(account);
                return new AccountLoginModel {Email = accountUpdated.Email, Password = accountUpdated.Password};;
            }
            throw new BadRequestException("Hubo un error al cambiar el password"); 
        }

        [POST("/UpdateAccount/{Token}")]
        public UpdateAccountModel UpdateAccount([FromBody] UpdateAccountModel model, string Token)
        {
            var session = _readOnlyRepository.First<Sessions>(session1 => session1.Token == Token);
            ValidateSession(session);
            session.User.FirstName = model.FirstName;
            session.User.LastName = model.LastName;
            session.User.Email = model.Email;
            
            var accountUpdated = _writeOnlyRepository.Update(session.User);
            return  new UpdateAccountModel {Email = accountUpdated.Email, FirstName = accountUpdated.FirstName,LastName = accountUpdated.LastName};


        }

        private bool EmailIsValid(string email)
        {
            if (!email.Contains('@') || !email.Contains('.'))
                throw new BadRequestException("Correo invalido");
            return true;
        }

        public static void ValidateSession(Sessions session)
        {
            if (session == null || !session.IsTokenActive())
                throw new BadRequestException("Session has expired. Please login again.");
        }


    }

    public class BadRequestException : HttpResponseException
    {
        public BadRequestException(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public BadRequestException(HttpResponseMessage response) : base(response)
        {
        }

        public BadRequestException(string errorMessage) : base(HttpStatusCode.BadRequest)
        {
            
            this.Response.ReasonPhrase = errorMessage;
        }
    }
}