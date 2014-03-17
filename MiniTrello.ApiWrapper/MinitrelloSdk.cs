using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniTrello.Api.Models;
using RestSharp;

namespace MiniTrello.ApiWrapper
{
    public class MinitrelloSdk
    {
        private static RestRequest InitRequest(string resource, Method method, object payload)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(payload);
            return request;
        }

        public static AuthenticationModel Login(AccountLoginModel loginModel)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("/login", Method.POST, loginModel);
            IRestResponse<AuthenticationModel> response = client.Execute<AuthenticationModel>(request);
            ConfigurationManager.AppSettings["accessToken"] = response.Data.Token;
            return response.Data;
        }

        public static AccountRegisterModel Register(AccountRegisterModel registerModel)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("/register", Method.POST, registerModel);
            IRestResponse<AccountRegisterModel> response = client.Execute<AccountRegisterModel>(request);
            return response.Data;
        }

        public static AccountLoginModel ChangePassword(AccountChangePassword model, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("/Account/ChangePassword/" + token, Method.POST, model);
            IRestResponse<AccountLoginModel> response = client.Execute<AccountLoginModel>(request);
            return response.Data;
        }

        public static AccountLoginModel ForgotPassword(AccountChangePassword model)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("/Account/ForgotPassword/", Method.POST, model);
            IRestResponse<AccountLoginModel> response = client.Execute<AccountLoginModel>(request);
            return response.Data;
        }

        public static UpdateAccountModel UpdateAccount(UpdateAccountModel model, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("/UpdateAccount/" + token, Method.POST, model);
            IRestResponse<UpdateAccountModel> response = client.Execute<UpdateAccountModel>(request);
            return response.Data;
        }

        public static AccountBoardModel CreateNewBoard(AccountBoardModel model, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("Boards/createBoard/" + token, Method.POST, model);
            IRestResponse<AccountBoardModel> response = client.Execute<AccountBoardModel>(request);
            return response.Data;
        }

        public static ChangeBoardNameModel ChangeBoardName(ChangeBoardNameModel model, long IdBoard, string token)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("Boards/ChangeBoardName/"+IdBoard+"/"+token, Method.PUT, model);
            IRestResponse<ChangeBoardNameModel> response = client.Execute<ChangeBoardNameModel>(request);
            return response.Data;
        }

        public static AccountBoardModel Archive(string token, BoardArchiveModel model)
        {
            var client = new RestClient(BaseUrl);
            var request = InitRequest("board/" + token, Method.DELETE, model);
            IRestResponse<AccountBoardModel> response = client.Execute<AccountBoardModel>(request);
            return response.Data;
        }





        private static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["baseUrl"]; }
        }

        public static List<OrganizationModel> GetOrganization()
        {

            return null;
        } 
    }
}
