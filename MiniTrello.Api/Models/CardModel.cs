using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniTrello.Api.Models
{
    public class CardModel
    {
        public long Id { get; set; }
        public string Content { get; set; }
    }
}