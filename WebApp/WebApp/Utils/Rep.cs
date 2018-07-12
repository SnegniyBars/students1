using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Models;
namespace WebApp.Utils
{
    public class Rep : IRep
    {
        private string _baseUrl = "http://localhost:57678/";

        //public Rep(string baseUrl)
        //{
        //    _baseUrl = baseUrl;
        //}

        public List<ShortInfoDay> GetSch(DateTime? from = null, DateTime? to = null)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //var str = httpClient.GetStringAsync($"{_baseUrl}getSch").Result;
                    var str = httpClient.GetStringAsync($"{_baseUrl}GetScheduler").Result;
                    var res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShortInfoDay>>(str);

                    return res;
                }
            }
            catch (Exception)
            {

                return new List<ShortInfoDay>();
            }
           
        }

    }
}
