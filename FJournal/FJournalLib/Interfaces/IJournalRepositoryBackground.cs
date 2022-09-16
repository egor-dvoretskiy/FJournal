using FJournalLib.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FJournalLib.Interfaces
{
    public interface IJournalRepositoryBackground
    {
        IEnumerable<DBRecord> ObserveChanges(CancellationToken cancellationToken);
    }
}
