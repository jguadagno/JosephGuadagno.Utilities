using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JosephGuadagno.Utilities.Security
{
    /// <summary>
    ///     Provides cryptography functions.
    /// </summary>
    public class Hash
    {
        /// <summary>
        ///     The type of hashing to use.
        /// </summary>
        public enum HashType
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        /// <summary>
        ///     The hash value computed.
        /// </summary>
        public byte[] HashValue;

        /// <summary>
        ///     The salt value used for the computed hash.
        /// </summary>
        public byte[] SaltValue;

        /// <summary>
        ///     A Base64 encoded copy of the <see cref="HashValue" />
        /// </summary>
        public string HashToBase64 => ByteToString(HashValue);

        /// <summary>
        ///     A Base64 encoded copy of the <see cref="SaltValue" />
        /// </summary>
        public string SaltToBase64 => ByteToString(SaltValue);

        /// <summary>
        ///     The representation of the class in a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ByteToString(HashValue);
        }

        /// <summary>
        ///     Generates a hash for the given plain text value and returns a
        ///     base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        ///     Plain text value to be hashed. The function does not check whether
        ///     this parameter is null.
        /// </param>
        /// <param name="hashType">
        ///     The type of hashing algorithm to use.
        /// </param>
        /// <param name="saltBytes">
        ///     Salt bytes. This parameter can be null, in which case a random salt
        ///     value will be generated.
        /// </param>
        /// <returns>
        ///     Hash value formatted as a base64-encoded string.
        /// </returns>
        public static Hash ComputeHash(string plainText,
            HashType hashType,
            byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
                saltBytes = GenerateSalt();

            Hash returnedHash = new Hash {SaltValue = saltBytes};

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Initialize appropriate hashing algorithm class.
            switch (hashType)
            {
                case HashType.SHA1:
                    hash = new SHA1Managed();
                    break;

                case HashType.SHA256:
                    hash = new SHA256Managed();
                    break;

                case HashType.SHA384:
                    hash = new SHA384Managed();
                    break;

                case HashType.SHA512:
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            returnedHash.HashValue = hash.ComputeHash(plainTextBytes);

            return returnedHash;
        }

        /// <summary>
        ///     Compares a hash of the specified plain text value to a given hash
        ///     value. Plain text is hashed with the same salt value as the original
        ///     hash.
        /// </summary>
        /// <param name="plainText">
        ///     Plain text to be verified against the specified hash. The function
        ///     does not check whether this parameter is null.
        /// </param>
        /// <param name="hashType">
        ///     The type of the hashing algorithm to use.
        /// </param>
        /// <param name="hash">
        ///     Base64-encoded hash value produced by ComputeHash function. This value
        ///     includes the original salt appended to it.
        /// </param>
        /// <returns>
        ///     If computed hash matches the specified hash the function the return
        ///     value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText,
            HashType hashType,
            Hash hash)
        {
            // Compute a new hash string.
            Hash expectedHash = ComputeHash(plainText, hashType, hash.SaltValue);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hash.HashToBase64 == expectedHash.HashToBase64);
        }

        /// <summary>
        ///     Verifies the hash.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashType">Type of the hash.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public static bool VerifyHash(string hashCode, string plainText, HashType hashType, byte[] salt)
        {
            Hash expectedHash = ComputeHash(plainText, hashType, salt);
            return hashCode == expectedHash.HashToBase64;
        }

        /// <summary>
        ///     Converts a byte array to a Base64 string.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A Base64 string.</returns>
        public static string ByteToString(byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        /// <summary>
        ///     Strings to byte.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public static byte[] StringToByte(string salt)
        {
            return Convert.FromBase64String(salt);
        }

        /// <summary>
        ///     Generates a Salt for hashing
        /// </summary>
        /// <returns>An array of bytes for the salt between 4 and 8 bytes.</returns>
        public static byte[] GenerateSalt()
        {
            return GenerateSalt(4, 8);
        }

        /// <summary>
        ///     Generates a Salt for hashing
        /// </summary>
        /// <param name="minSaltSize">The minimum size of the salt to generate</param>
        /// <param name="maxSaltSize">The maximum size of the salt to generate</param>
        /// <returns>
        ///     An array of bytes for the salt between <paramref name="minSaltSize" /> and <paramref name="maxSaltSize" />
        /// </returns>
        public static byte[] GenerateSalt(int minSaltSize, int maxSaltSize)
        {
            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            byte[] saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }

        /// <summary>
        ///     Converts an integer to a hash value
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string IntToHash(int id)
        {
            // Need to get the get the hash code
            byte[] salt = StringToByte(ConfigurationManager.AppSettings["salt"]);
            Hash hash = ComputeHash(id.ToString(), HashType.SHA512, salt);

            // UrlEncode the string
            return HttpUtility.UrlEncode(hash.HashToBase64);
        }

        /// <summary>
        ///     Validates that the integer matches the hash code.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static bool IntMatchesHash(string hashCode, int id)
        {
            byte[] salt = StringToByte(ConfigurationManager.AppSettings["salt"]);

            return VerifyHash(hashCode, id.ToString(), HashType.SHA512, salt);
        }

        /// <summary>
        ///     Gets a parameterized query string including a hash code.
        /// </summary>
        /// <param name="idFieldName">Name of the id field.</param>
        /// <param name="hashFieldName">Name of the hash field.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static string GetHashedParameter(string idFieldName, string hashFieldName, int id)
        {
            return $"{idFieldName}={id}&{hashFieldName}={IntToHash(id)}";
        }

        public static string GetUrlEncodedHash(string text, string salt)
        {
            byte[] byteSalt = StringToByte(salt);
            Hash hash = ComputeHash(text, HashType.SHA512, byteSalt);

            // UrlEncode the string
            return HttpUtility.UrlEncode(hash.HashToBase64);
        }
    }
}