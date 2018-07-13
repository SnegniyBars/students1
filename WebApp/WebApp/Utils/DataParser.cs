using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApp.Enums;
namespace WebApp.Utils
{
    public class DataParser
    {
        private IRep _rep;
        public List<ShortInfoDay> List;
        public DateTime? From { get; set; } = DateTime.Today.AddDays(-14);
        public DateTime? To { get; set; } = DateTime.Today.AddDays(14);
        public DataParser(IRep rep)
        {
            _rep = rep;
            List = _rep.GetSch();
        }
        public void Parse() //Count || 
        {
            var data = _rep.GetSch();
            
            //преобразование->
        }
        public List<ShortInfoDay> DataChange(DataChange change)
        {
            switch (change)
            {
                case Enums.DataChange.AddWeeks:
                    From = From.Value.AddDays(7);
                    To = To.Value.AddDays(7);                    
                    break;
                case Enums.DataChange.DecWeeks:
                   From = From.Value.AddDays(-7);
                    To = To.Value.AddDays(-7);
                    break;
                case Enums.DataChange.Add5Weeks:
                    From.Value.AddDays(35);
                    To.Value.AddDays(35);
                    break;
                case Enums.DataChange.Dec5Weeks:
                    From.Value.AddDays(-35);
                    To.Value.AddDays(-35);
                    break;
                default:
                    break;                
            }
            return List = _rep.GetSch1(From, To);
        }
        public int CountTheDay(DateTime date)
        {
            // if list.count() == 0?
            foreach (var item in List)
            {
                //логика
            }
            return 1;
        }
        public object ShowTheDay(DateTime date)
        {
           var filtered = List.Where(_ => _.GetType() == typeof(object));
            return new object();
        }
    }
}

