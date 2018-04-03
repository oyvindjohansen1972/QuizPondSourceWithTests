using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QuizPond.Tests
{
    class MyTestSession : ISession
    {
        private Dictionary<string, byte[]> MyDictionary;
        

        public MyTestSession()
        {
            MyDictionary = new Dictionary<string, byte[]>();
        }


        public bool IsAvailable { get; set; }
        //public bool IsAvailable => throw new NotImplementedException();


        public string Id { get; set; }
        // public string Id => throw new NotImplementedException();

        public IEnumerable<string> Keys => MyDictionary.Keys;      
       

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, byte[] value)
        {
            MyDictionary[key] = value;            
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            value = new  byte[1];
            if (MyDictionary.ContainsKey(key))
            {
                value = MyDictionary[key];
                return true;
            }
            return false;
        }
    }
}
