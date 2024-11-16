using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TicketingSystem.Dtos;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;
using TicketingSystem.Services.Interface;
using TicketingSystem.Utils;

namespace TicketingSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Registration: Hash password and save user to the database
        public async Task<bool> Register(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            // Hash the password using a secure hashing algorithm (e.g., BCrypt)
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword, // Store hashed password
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(newUser);
            return true;
        }

        // Login: Check user credentials, validate password, generate JWT
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            // Decrypt the password sent from the frontend  
            //var decryptedPassword = DecryptPassword(request.Password);

            // Retrieve user from the repository
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            // Verify password with BCrypt
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null; // User not found or invalid credentials
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Username = user.Username,
                Token = token
            };
        }

        // Method to decrypt the password using the same secret key used in the frontend
        private string DecryptPassword(string encryptedPassword)
        {
            const string secretKey = "YzMyNjk4ZjYyMjA2ZTE2YjUzZDMyYjk1NjM4NjkxYzY="; // The same key used in the frontend (Base64 encoded)
            const string iv = "0000000000000000"; // Use the same IV as used in the frontend, or securely transmit IV

            // Decode base64-encoded encrypted password
            var bytes = Convert.FromBase64String(encryptedPassword);
            var decryptedBytes = DecryptAES(bytes, secretKey, iv); // Decrypt with the AES key and IV
            return Encoding.UTF8.GetString(decryptedBytes); // Convert back to string
        }

        // AES decryption method
        private byte[] DecryptAES(byte[] encryptedData, string key, string iv)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key); // Decode the key (Base64 to bytes)
                aesAlg.IV = Encoding.UTF8.GetBytes(iv); // IV must match the frontend's IV

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                }
            }
        }

        // Token validation
        public Task<bool> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            if (key.Length == 0)
            {
                throw new ArgumentException("JWT SecretKey is not configured correctly.");
            }

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        // Helper method to generate a JWT token
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            if (key.Length == 0)
            {
                throw new ArgumentException("JWT SecretKey is not configured correctly.");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<bool> DeleteUser(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return false; // User not found
            }

            await _userRepository.DeleteAsync(user); 
            return true; 
        }
    }
}
