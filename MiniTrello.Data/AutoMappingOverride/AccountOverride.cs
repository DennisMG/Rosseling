using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Data.AutoMappingOverride
{
    public class AccountOverride : IAutoMappingOverride<Accounts>
    {
        public void Override(AutoMapping<Accounts> mapping)
        {
             //mapping.HasMany(x => x.Referrals)
             //    .Inverse()
             //    .Access.CamelCaseField(Prefix.Underscore);
        }
    }
}
