using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzshka2
{
    public interface IJournalEntry
    {
        string ToLogLine();
        string ToScreenLine();
    }
}
