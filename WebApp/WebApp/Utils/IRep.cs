using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Utils
{
    public interface IRep
    {
        List<object> GetSch(DateTime? from = null, DateTime? to = null, int? kab = null);
    }
}
