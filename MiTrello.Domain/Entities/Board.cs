using System.Collections.Generic;
namespace MiniTrello.Domain.Entities
{
    public class Board : IEntity

    {
        private readonly IList<Accounts> _members = new List<Accounts>();
        private readonly IList<Lane> _lanes = new List<Lane>();
        public virtual Accounts Administrator { get; set; }
        public virtual string Title { get; set; }
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual IEnumerable<Lane> Lanes{ get { return _lanes; } }

        public virtual void AddLane(Lane lane)
        {
            _lanes.Add(lane);
        }


        public virtual IEnumerable<Accounts> Members{ get { return _members; } }

        public virtual void AddMember(Accounts member)
        {
            if (!_members.Contains(member))
            {
                _members.Add(member);
            }

        }


    }
}