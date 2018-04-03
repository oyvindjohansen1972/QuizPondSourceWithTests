using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace QuizPond.Infrastructure
{

    public static class SessionExtentions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }


        


        //public static void SetJson(this ISession session, string key, object value)
        //{
        //    session.SetString(key, JsonConvert.SerializeObject(value));
        //}

        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }


        //----------------------------------------------------


        //public static void SetString(this ISession session, string key, string value)
        //{
        //    session.Set(key, Encoding.UTF8.GetBytes(value));
        //}

        //public static string GetString(this ISession session, string key)
        //{
        //    var data = session.Get(key);
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    return Encoding.UTF8.GetString(data);
        //}

        public static byte[] Get(this ISession session, string key)
        {
            byte[] value = null;
            session.TryGetValue(key, out value);
            return value;
        }


    }
}
