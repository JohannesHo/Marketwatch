using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace UpdateLib
{
    /// <summary>
    /// The type of has to create
    /// </summary>
    internal enum HashType
    {
        MD5,
        SHA1,
        SHA512
    }

    /// <summary>
    /// Class used to generate hash sums of files
    /// </summary>
    internal static class Hasher
    {
        /// <summary>
        /// Generate a hash sum of a file
        /// </summary>
        /// <param name="filePath">The file to hash</param>
        /// <param name="algo">The Type of hash</param>
        /// <returns>The computed hash</returns>
        internal static string HashFile(string filePath, HashType algo)
        {
            FileStream file = new FileStream(filePath, FileMode.Open);
            string hash = "";

            switch (algo)
            {
                case HashType.MD5:
                    hash = MakeHashString(MD5.Create().ComputeHash(file));
                    break;
                case HashType.SHA1:
                    hash = MakeHashString(SHA1.Create().ComputeHash(file));
                    break;
                case HashType.SHA512:
                    hash = MakeHashString(SHA512.Create().ComputeHash(file));
                    break;
                default:
                    break;
            }

            file.Close();

            return hash;
        }

        /// <summary>
        /// Converts byte[] to string
        /// </summary>
        /// <param name="hash">The hash to convert</param>
        /// <returns>Hash as string</returns>
        private static string MakeHashString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
                sb.Append(b.ToString("X2").ToLower());

            return sb.ToString();
        }
    }
}
