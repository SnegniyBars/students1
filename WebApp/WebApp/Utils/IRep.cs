using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApp.Utils
{
    public interface IRep
    {
        List<ShortInfoDay> GetSch(DateTime? from = null, DateTime? to = null);
        List<ShortInfoDay> GetSch1(DateTime? from = null, DateTime? to = null);

    
}
}
