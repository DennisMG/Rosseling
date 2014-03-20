using System.Collections.Generic;

namespace MiniTrello.Domain.Entities
{
    public class Accounts : IEntity
    {
       
        private readonly IList<Organization> _organizations = new List<Organization>();
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }

        public virtual IEnumerable<Organization> Organizations
        {
            get { return _organizations; }
        }

        public virtual void AddOrganization(Organization organization)
        {
            if (!_organizations.Contains(organization))
            {
                _organizations.Add(organization);
            }
        }

        
    }
}