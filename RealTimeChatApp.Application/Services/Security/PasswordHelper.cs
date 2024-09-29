using MediatR.NotificationPublishers;
using System.Security.Cryptography;

namespace RealTimeChatApp.Application.Services.Security;

public class PasswordHelper
{
    public static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static byte[] HashPassword(string password, byte[] salt, int iterations = 10000, int hashLength = 32)
    {
        using(var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
        {
            return pbkdf2.GetBytes(hashLength);
        }
    }

    public static string HashPasswordWithSalt(string password, byte[] salt)
    {
        var hash = HashPassword(password, salt);

        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string enteredPassword, string storedPassword) {
        
        var parts = storedPassword.Split(':');
        var storedSalt = Convert.FromBase64String(parts[0]);
        var storedHash = Convert.FromBase64String(parts[1]);

        var enteredHash = HashPassword(enteredPassword, storedSalt);

        return storedHash.SequenceEqual(enteredHash);

    }
}
