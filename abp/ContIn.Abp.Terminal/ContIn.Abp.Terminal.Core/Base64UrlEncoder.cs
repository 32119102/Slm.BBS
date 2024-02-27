using System;

namespace ContIn.Abp.Terminal.Core
{
    public static class Base64UrlEncoder
    {
        public static System.Text.Encoding TextEncoding = System.Text.Encoding.UTF8;

        private static char Base64PadCharacter = '=';

        private static string DoubleBase64PadCharacter = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{0}", new object[]
        {
            Base64PadCharacter
        });

        private static char Base64Character62 = '+';

        private static char Base64Character63 = '/';

        private static char Base64UrlCharacter62 = '-';

        private static char Base64UrlCharacter63 = '_';

        public static string Encode(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new Exception("arg is null");
            }
            return Encode(TextEncoding.GetBytes(arg));
        }

        public static string Encode(byte[] arg)
        {
            string text = Convert.ToBase64String(arg);
            text = text.Split(new char[]
            {
                Base64PadCharacter
            })[0];
            text = text.Replace(Base64Character62, Base64UrlCharacter62);
            return text.Replace(Base64Character63, Base64UrlCharacter63);
        }

        public static byte[] DecodeBytes(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new Exception("arg is null");
            }
            string text = arg.Replace(Base64UrlCharacter62, Base64Character62);
            text = text.Replace(Base64UrlCharacter63, Base64Character63);
            switch (text.Length % 4)
            {
                case 0:
                    goto IL_7D;
                case 2:
                    text += DoubleBase64PadCharacter;
                    goto IL_7D;
                case 3:
                    text += Base64PadCharacter;
                    goto IL_7D;
            }
            throw new ArgumentException("Illegal base64url string!", arg);
        IL_7D:
            return Convert.FromBase64String(text);
        }

        public static string Decode(string arg)
        {
            return TextEncoding.GetString(DecodeBytes(arg));
        }
    }
}
