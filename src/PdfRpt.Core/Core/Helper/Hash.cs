using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Hash class contains some useful hashing methods such as MD5.
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Calculates the MD5 of a byte array.
        /// </summary>
        public static string MD5Hash(this byte[] inputBytes)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Calculates the MD5 of the input string.
        /// </summary>
        public static string MD5Hash(this string strPass)
        {
            byte[] data = Encoding.UTF8.GetBytes(strPass);
            return MD5Hash(data);
        }
    }
}
