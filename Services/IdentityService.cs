using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Options;

namespace DragonflyTracker.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<DragonflyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly PgMainDataContext _pgMainDataContext;
        
        public IdentityService(UserManager<DragonflyUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, PgMainDataContext pgMainDataContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _pgMainDataContext = pgMainDataContext;
            _roleManager = roleManager;
        }
        
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (existingUserWithEmail != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User with this email address already exists"}
                };
            }

            var newUserId = Guid.NewGuid();
            var newUser = new DragonflyUser
            {
                Id = newUserId.ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password).ConfigureAwait(false);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }
            
            return await GenerateAuthenticationResultForUserAsync(newUser).ConfigureAwait(false);
        }


        public async Task<AuthenticationResult> RegisterAsync(string username, string email, string password)
        {
            var existingUserWithEmail = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (existingUserWithEmail != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }
            var existingUserWithUsername = await _userManager.FindByNameAsync(username).ConfigureAwait(false);

            if (existingUserWithUsername != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this Username already exists" }
                };
            }

            var newUserId = Guid.NewGuid();
            var newUser = new DragonflyUser
            {
                Id = newUserId.ToString(),
                Email = email,
                UserName = username
            };

            var createdUser = await _userManager.CreateAsync(newUser, password).ConfigureAwait(false);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser).ConfigureAwait(false);

            return await GenerateAuthenticationResultForUserAsync(newUser).ConfigureAwait(false);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User does not exist"}
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User/password combination is wrong"}
                };
            }
            
            return await GenerateAuthenticationResultForUserAsync(user).ConfigureAwait(false);
        }

        public async Task<AuthenticationResult> LogoutAsync(string RefreshToken)
        {
            var refreshToken = _pgMainDataContext.RefreshTokens.SingleOrDefault(rt => rt.Token == RefreshToken);

            if (refreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
            }

            refreshToken.Invalidated = true;
            _pgMainDataContext.RefreshTokens.Update(refreshToken);
            var updated = await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);

            // var refreshToken = new RefreshToken { Token = RefreshToken };
            // _context.RefreshTokens.Attach(refreshToken);
            // _context.RefreshTokens.Remove(refreshToken);
            //var deleted = await _context.SaveChangesAsync().ConfigureAwait(false);

            return new AuthenticationResult { Success = true };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult {Errors = new[] {"Invalid Token"}};
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            
            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult {Errors = new[] {"This token hasn't expired yet"}};
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _pgMainDataContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken).ConfigureAwait(false);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token does not exist"}};
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token has expired"}};
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token has been invalidated"}};
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token has been used"}};
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token does not match this JWT"}};
            }

            storedRefreshToken.Used = true;
            _pgMainDataContext.RefreshTokens.Update(storedRefreshToken);
            await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value).ConfigureAwait(false);
            return await GenerateAuthenticationResultForUserAsync(user).ConfigureAwait(false);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(DragonflyUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole).ConfigureAwait(false);
                if(role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

                foreach (var roleClaim in roleClaims)
                {
                    if(claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _pgMainDataContext.RefreshTokens.AddAsync(refreshToken).ConfigureAwait(false);
            await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<bool> UpdatePasswordAsync(DragonflyUser userToUpdate, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(userToUpdate).ConfigureAwait(false);
            var passChangeResult = await _userManager.ResetPasswordAsync(userToUpdate, token, newPassword).ConfigureAwait(false);
            return passChangeResult.Succeeded;
        }

        public async Task<bool> ResetPasswordEmailAsync(DragonflyUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task<bool> ResetPasswordAsync(DragonflyUser user, string token, string newPassword)
        {
            var passChangeResult = await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);
            return passChangeResult.Succeeded;
        }

        public async Task<bool> CheckUserPasswordAsync(DragonflyUser user, string password)
        {
            var result =  await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false);
            return result;
        }

        public Task<bool> ValidatePasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmEmailAsync(DragonflyUser user,string token)
        {
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);
            return confirmResult.Succeeded;
        }
    }
}