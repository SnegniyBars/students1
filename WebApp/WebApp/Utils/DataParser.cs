using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApp.Utils
{
    public class DataParser
    {
        private IRep _rep;
        public List<ShortInfoDay> List;
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

