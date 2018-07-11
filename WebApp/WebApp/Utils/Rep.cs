using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Utils
{
    public class Rep : IRep
    {
        private string _baseUrl;

        public Rep(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public List<object> GetSch(DateTime? from = null, DateTime? to = null, int? kab = null)
        {
            using (var httpClient = new HttpClient())
            {
                var str = httpClient.GetStringAsync($"{_baseUrl}getSch").Result;

                var res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(str);

                return res;
            }
        }

    }
}
