using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class LaneModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<CardModel> Cards { get; set; }
        public bool IsArchived { get; set; }
    }
}