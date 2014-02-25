using System.Collections.Generic;
using System.Linq;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniTrello.Domain.Entities;
using NHibernate;

namespace MiniTrello.DatabaseDeployer
{
    public class AccountSeeder : IDataSeeder
    {
        readonly ISession _session;

        public AccountSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            IList<Account> accountList = Builder<Account>.CreateListOfSize(10).Build();
            
            foreach (Account account in accountList)
            {
                var boards = Builder<Board>.CreateListOfSize(2).Build();
                foreach (var board in boards)
                {
                    var lanes = Builder<Lane>.CreateListOfSize(2).Build();
                    foreach (var lane in lanes)
                    {
                        _session.Save(lane);
                    }
                    _session.Save(board);
                    board.AddLane(lanes[0]);
                    board.AddLane(lanes[1]);
                }
                account.AddBoard(boards[0]);
                account.AddBoard(boards[1]);
                
                _session.Save(account);
            }
        }
    }
}