namespace SendInvite.Helpers
{
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
                    strGSMOutput += 27.ToString("X2");
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
