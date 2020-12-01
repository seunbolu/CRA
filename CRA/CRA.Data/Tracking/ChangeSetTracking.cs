using CRA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Data.Tracking
{
    public class ChangeSetTracking : IDisposable
    {
        DataContext _context;
        public ChangeSetTracking(DataContext context)
        {
            _context = context;
            _context.BeginChangeSet();
        }

        public void Dispose()
        {
            _context.CompleteChangeSet();
        }
    }
}
