using FanficsWorldAPI.Common.Configurations;
using FanficsWorldAPI.Common.Constants;
using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FanficsWorldAPI.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IValidator<LoginDto> _loginDtoValidator;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IRepository<User> _userRepository;
        private readonly JwtOptions _jwtOptions;

        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private const short MaxFailedLoginAttempts = 5;

        public AuthService(
            IValidator<LoginDto> loginDtoValidator,
            IRepository<User> userRepository,
            IOptions<JwtOptions> jwtOptions,
            IValidator<RegisterDto> registerDtoValidator)
        {
            _loginDtoValidator = loginDtoValidator;
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
            _registerDtoValidator = registerDtoValidator;
        }

        public async Task<ServiceResultDto<string>> LoginAsync(LoginDto loginDto)
        {
            await _loginDtoValidator.ValidateAndThrowAsync(loginDto);

            var user = await _userRepository.EntitySet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
            {
                return new ServiceResultDto<string>(false, ErrorMessageConstants.UserNotFound);
            }

            if (!user.IsActive)
            {
                return new ServiceResultDto<string>(false, ErrorMessageConstants.UserInactive);
            }

            var (hash, _) = HashPassword(loginDto.Password, user.PasswordSalt);
            var isPasswordValid = user.PasswordHash == hash;
            if (!isPasswordValid)
            {
                ++user.FailedLoginAttempts;
                if (user.FailedLoginAttempts == MaxFailedLoginAttempts)
                {
                    user.IsActive = false;
                    Log.Information("User {userEmail} was blocked after {failedAttempts} failed login attempts.", user.Email, user.FailedLoginAttempts);
                }

                await _userRepository.UpdateAsync(user);
            }

            return new ServiceResultDto<string>
            {
                IsSuccess = isPasswordValid,
                Errors = isPasswordValid ? []
                    : [$"Invalid password. You have {MaxFailedLoginAttempts - user.FailedLoginAttempts} login attempt(s) left."],
                Value = isPasswordValid ? GenerateToken(user) : null
            };
        }

        public async Task<ServiceResultDto> RegisterAsync(RegisterDto registerDto)
        {
            const int UserRoleId = 3;

            await _registerDtoValidator.ValidateAndThrowAsync(registerDto);

            var user = new User
            {
                Name = registerDto.Name,
                BirthDate = registerDto.BirthDate,
                Email = registerDto.Email,
                Bio = registerDto.Bio,
                IsActive = true,
                RoleId = UserRoleId
            };

            (user.PasswordHash, user.PasswordSalt) = HashPassword(registerDto.Password);

            var userCreated = await _userRepository.InsertAsync(user);

            return new ServiceResultDto
            {
                IsSuccess = userCreated,
                Errors = userCreated ? [] : ["Failed to register a user."]
            };
        }

        #region Private methods

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: [
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                ],
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static (string hash, string salt) HashPassword(string password, string? oldSalt = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(password);
            byte[] saltBytes;
            if (string.IsNullOrWhiteSpace(oldSalt))
            {
                using var rng = RandomNumberGenerator.Create();
                saltBytes = new byte[SaltSize];
                rng.GetBytes(saltBytes);
            }
            else
            {
                saltBytes = Convert.FromBase64String(oldSalt);
            }

            var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256)
                .GetBytes(HashSize);

            string saltBase64 = !string.IsNullOrWhiteSpace(oldSalt)
                ? oldSalt
                : Convert.ToBase64String(saltBytes);
            string hashBase64 = Convert.ToBase64String(hashBytes);

            return (hashBase64, saltBase64);
        }

        #endregion
    }
}
