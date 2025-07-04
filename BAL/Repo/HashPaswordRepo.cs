using BAL.Interfaces;
using System;
using System.Security.Cryptography;

namespace BAL.Repo
{
    public class HashPaswordRepo : IPasswordHash
    {
        // BCrypt work factor - higher is more secure but slower
        private const int WORK_FACTOR = 12;

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");

            try
            {
                // Generate a salt and hash the password using BCrypt
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, WORK_FACTOR);
                return passwordHash;
            }
            catch (Exception)
            {
                // Log the error here (without exposing the actual password)
                throw new InvalidOperationException("An error occurred while hashing the password");
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword), "Hashed password cannot be null or empty");
            
            if (string.IsNullOrEmpty(providedPassword))
                throw new ArgumentNullException(nameof(providedPassword), "Provided password cannot be null or empty");

            try
            {
                // Use a constant-time comparison to prevent timing attacks
                return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            }
            catch (Exception)
            {
                // Log the error here (without exposing the actual passwords)
                return false;
            }
        }

        /// <summary>
        /// Checks if a password needs to be rehashed (e.g., if work factor has changed)
        /// </summary>
        public bool NeedsRehash(string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.GetWorkFactor(hashedPassword) < WORK_FACTOR;
            }
            catch
            {
                // If we can't determine the work factor, assume it needs rehashing
                return true;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random password
        /// </summary>
        public string GenerateSecurePassword(int length = 16)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?";
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            var chars = new char[length];

            rng.GetBytes(bytes);
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[bytes[i] % validChars.Length];
            }

            return new string(chars);
        }
    }
}
