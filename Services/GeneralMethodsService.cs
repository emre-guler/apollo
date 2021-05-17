using System;
using System.Linq;
using System.Threading.Tasks;
using Apollo.Data;
using Apollo.Enums;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class GeneralMethodsService
    {
        private int stringLen = 30;
        private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();
        private readonly ApolloContext _db;
        public GeneralMethodsService(
            ApolloContext db
        )
        {
            _db = db;
        }

        public async Task<string> GenerateRandomString(RequestType requestType)
        {
            string response = "";
            while(true)
            {
                string randomString = new String(Enumerable.Repeat(chars, stringLen)
                .Select(s => s[random.Next(s.Length)]).ToArray());
                bool stringControl = false;
                if(requestType == RequestType.ResetPassword)
                {
                    stringControl = await _db.PasswordResetRequests.AnyAsync(x => x.URL == randomString);
                }
                else if(requestType == RequestType.MailVerification)
                {
                    stringControl = await _db.VerificationRequests.AnyAsync(x => x.URL == randomString);
                }

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

        public int GenerateRandomInt()
        {
            return random.Next(100000, 999999);
        }
    }
}