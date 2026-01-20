using System.Security.Cryptography;
using System.Text;

namespace AmIAuthorised.Utility
{
    public static class KeyGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateKey(int length = 10)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(Chars[bytes[i] % Chars.Length]);
            }

            return result.ToString();
        }
    }
}
