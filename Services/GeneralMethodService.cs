using System;
using System.Linq;
using Apollo.Data;

namespace Apollo.Services
{
    public class GeneralMethodService
    {
        private int stringLen = 30;
        private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();
        private readonly ApolloContext _db;
        public GeneralMethodService(
            ApolloContext db
        )
        {
            _db = db;
        }

        public string GenerateRandomString()
        {
            string response;
            while(true)
            {
                string randomString = new String(Enumerable.Repeat(chars, stringLen)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                var stringControl = _db.VerificationRequests.Any(x => x.URL == randomString);
                if(stringControl)
                {
                    continue;
                }
                else
                {
                    response = randomString;
                    break;
                }
            }
            return response;
        }
    }
}