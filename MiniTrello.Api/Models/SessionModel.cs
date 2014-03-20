using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class SessionModel
    {
        public Accounts User { get; set; }
        public DateTime LoginDate { get; set; }
        public int DurationTime { get; set; }
        public string Token { get; set; }


    }

    
}