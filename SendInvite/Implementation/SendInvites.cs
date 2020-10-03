using Microsoft.AspNetCore.Mvc;
using SendInvite.DTOs;
using SendInvite.Helpers;
using SendInvite.Interface;
using System;

namespace SendInvite.Implementation
{
    public class SendInvites : ISendInvites
    {
        private const int user_id = 7;

        public ObjectResult Result(string[] numbers, string message)
        {
            ValidDTO result = new ValidDTO();
            try
            {
                result = Validates.Validate(new SendInvitesDTO { Numbers = numbers, Messages = message });

                if (result.Status == 200)
                {
                    Invite(user_id, numbers);
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(result) { StatusCode = 500, Value = ex.Message };
            }
            return new ObjectResult(result) { StatusCode = result.Status, Value = result.Message };
        }

        private void Invite(int user_id, string[] numbers)
        {
            //Save data to db
        }
    }
}
