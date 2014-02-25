using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Lane : IEntity 

    {
        private readonly IList<Card> _cards = new List<Card>();
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string Name { get; set; }
        
        public virtual IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public virtual void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public virtual void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

    }
}