using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniTrello.Api.Models
{
    public class AccountForgotPasswordModel
    {
        public string Email;
        public string NewPassword;
        public string ConfirmNewPassword;
    }
}