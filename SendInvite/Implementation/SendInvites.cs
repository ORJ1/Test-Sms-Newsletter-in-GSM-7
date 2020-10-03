using Microsoft.AspNetCore.Mvc;
using SendInvite.DTOs;
using SendInvite.Interface;
using System;
using System.Linq;

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


    internal class Encoding
    {

        public static string GSMChar(string PlainText)
        {
            string strGSMTable = "";
            strGSMTable += "@£$¥èéùìòÇ`Øø`Åå";
            strGSMTable += "Δ_ΦΓΛΩΠΨΣΘΞ`ÆæßÉ";
            strGSMTable += " !\"#¤%&'()*=,-./";
            strGSMTable += "0123456789:;<=>?";
            strGSMTable += "¡ABCEFGHIJKLMNO";
            strGSMTable += "PQRSTUVWXYZÄÖÑÜ`";
            strGSMTable += "¿abcdefghijklmno";
            strGSMTable += "pqrstuvwxyzäöñüà";

            string strExtendedTable = "";
            strExtendedTable += "````````````````";
            strExtendedTable += "````^```````````";
            strExtendedTable += "````````{}`````\\";
            strExtendedTable += "````````````[~]`";
            strExtendedTable += "|```````````````";
            strExtendedTable += "````````````````";
            strExtendedTable += "`````€``````````";
            strExtendedTable += "````````````````";

            string strGSMOutput = "";
            foreach (char cPlainText in PlainText.ToCharArray())
            {
                int intGSMTable = strGSMTable.IndexOf(cPlainText);

                if (intGSMTable != -1)
                {
                    strGSMOutput += intGSMTable.ToString("X2");
                    continue;
                }
                int intExtendedTable = strExtendedTable.IndexOf(cPlainText);
                if (intExtendedTable != -1)
                {
                    strGSMOutput += (27).ToString("X2");
                    strGSMOutput += intExtendedTable.ToString("X2");
                }
                else
                {
                    return "";
                }
            }
            return strGSMOutput;
        }

        public static string Translit(string str)
        {
            string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya" };
            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya" };
            string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_up[i], lat_up[i]);
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }

    }
}
