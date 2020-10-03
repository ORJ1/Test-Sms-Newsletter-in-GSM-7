using SendInvite.DTOs;
using System;
using System.Linq;

namespace SendInvite.Helpers
{
    internal class Validates
    {
        private const int apiid = 4;

        public static bool ChekDB(int id)
        {
            //Checking the number of requests in the database
            return GetCountInvitations(id) > 128 ? true : false;
        }

        private static int GetCountInvitations(int id)
        {
            return new Random().Next(0, 128);
        }

        public static ValidDTO Validate(SendInvitesDTO data)
        {
            if (ChekDB(apiid))
            {
                return new ValidDTO()
                {
                    Message = "PHONE_NUMBERS_INVALID: Too much phone numbers, should be less or equal to 128 per day.",
                    Status = 403
                };
            }

            if (string.IsNullOrEmpty(data.Messages))
            {
                return new ValidDTO()
                {
                    Message = "MESSAGE_EMPTY: Invite message is missing.",
                    Status = 405
                };
            }

            if (data.Numbers.Length == 0)
            {
                return new ValidDTO()
                {
                    Message = "PHONE_NUMBERS_EMPTY: Phone_numbers is missing.",
                    Status = 401
                };
            }

            if (data.Numbers.Length >= 16)
            {
                return new ValidDTO()
                {
                    Message = "PHONE_NUMBERS_INVALID: Too much phone numbers, should be less or equal to 16 per request.",
                    Status = 402
                };
            }

            //Здесь о5, в зависимости от тз, но лучше бы просто пропустил рассылку.
            if (data.Numbers.Distinct().Count() != data.Numbers.Length)
            {
                return new ValidDTO()
                {
                    Message = "PHONE_NUMBERS_INVALID: Duplicate numbers detected.",
                    Status = 404
                };
            }

            string message = Encoding.Translit(data.Messages);

            message = Encoding.GSMChar(message);
            if (message.Length > 160)
            {
                return new ValidDTO()
                {
                    Message = "MESSAGE_INVALID: Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.",
                    Status = 407
                };
            }

            if (string.IsNullOrEmpty(message))
            {
                return new ValidDTO()
                {
                    Message = "MESSAGE_INVALID: Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well.",
                    Status = 406
                };
            }

            //Вариативно от т.з., переделал бы на regex подправляющий номера в формат, съедал бы и + и скобки и т.п.
            foreach (string x in data.Numbers)
            {
                if (!x.StartsWith("7") || x.Length != 11)
                {
                    return new ValidDTO()
                    {
                        Message = "PHONE_NUMBERS_INVALID: One or several phone numbers do not match with international format",
                        Status = 400
                    };
                }
            }

            return new ValidDTO() { Status = 200, Message = "" };
        }
    }
}
