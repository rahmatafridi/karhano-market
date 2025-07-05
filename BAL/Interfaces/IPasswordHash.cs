﻿namespace BAL.Interfaces
{
    public interface IPasswordHash
    {
        /// <summary>
        /// Hashes a password using a secure hashing algorithm
        /// </summary>
        /// <param name="password">The plain text password to hash</param>
        /// <returns>The hashed password</returns>
        /// <exception cref="ArgumentNullException">Thrown when password is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when hashing fails</exception>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a password against its hash
        /// </summary>
        /// <param name="hashedPassword">The stored hash of the password</param>
        /// <param name="providedPassword">The password to verify</param>
        /// <returns>True if the password matches the hash, false otherwise</returns>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null or empty</exception>
        bool VerifyPassword(string hashedPassword, string providedPassword);

        /// <summary>
        /// Checks if a password hash needs to be upgraded (e.g., if work factor has changed)
        /// </summary>
        /// <param name="hashedPassword">The stored hash to check</param>
        /// <returns>True if the hash should be upgraded, false otherwise</returns>
        bool NeedsRehash(string hashedPassword);

        /// <summary>
        /// Generates a cryptographically secure random password
        /// </summary>
        /// <param name="length">The desired length of the password</param>
        /// <returns>A secure random password</returns>
        string GenerateSecurePassword(int length = 16);
    }
}
