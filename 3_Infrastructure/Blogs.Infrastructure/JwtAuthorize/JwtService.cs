using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blogs.Core;
using Blogs.Core.Config;
using Blogs.Infrastructure.JwtAuthorize;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Blogs.Domain
{
    /// <summary>
    /// Jwt帮助类
    /// </summary>
    public class JwtTokenParser
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtTokenParser(string issuer, string audience, string securityKey)
        {
            _issuer = issuer;
            _audience = audience;

            // 确保密钥长度足够
            var keyBytes = Encoding.UTF8.GetBytes(securityKey);
            if (keyBytes.Length < 32)
            {
                var newKey = new byte[32];
                Array.Copy(keyBytes, newKey, Math.Min(keyBytes.Length, 32));
                keyBytes = newKey;
            }
            _signingKey = new SymmetricSecurityKey(keyBytes);
        }

        /// <summary>
        /// 解析 JWT Token 并返回 SecurityTokenDescriptor
        /// </summary>
        /// <param name="jwtToken">JWT Token 字符串</param>
        /// <returns>SecurityTokenDescriptor 对象</returns>
        public SecurityTokenDescriptor ParseTokenToDescriptor(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                // 不验证地读取令牌，以查看其中的声明
                var jwtSecurityTokenWithoutValidation = handler.ReadJwtToken(jwtToken);
                // 获取令牌中的受众
                var audiencesInToken = jwtSecurityTokenWithoutValidation.Audiences;

                // 验证并解析令牌
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidAudiences = new[] { _audience },
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
                // 如果令牌中没有受众声明，那么我们可以选择不验证受众，但这样不安全
                if (!audiencesInToken.Any())
                {
                    // 如果没有受众，则关闭受众验证
                    validationParameters.ValidateAudience = false;
                }

                // 验证令牌
                var principal = handler.ValidateToken(jwtToken, validationParameters, out var validatedToken);

                // 将验证后的令牌转换为 JwtSecurityToken
                var jwtSecurityToken = validatedToken as JwtSecurityToken;
                if (jwtSecurityToken == null)
                {
                    throw new ArgumentException("Invalid JWT token");
                }

                // 提取声明
                var claims = new List<Claim>();
                foreach (var claim in jwtSecurityToken.Claims)
                {
                    claims.Add(claim);
                }

                // 创建 SecurityTokenDescriptor
                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = jwtSecurityToken.ValidTo,
                    NotBefore = jwtSecurityToken.ValidFrom,
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256Signature)
                };

                return descriptor;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse JWT token: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 解析 JWT Token 并返回包含所有信息的自定义对象
        /// </summary>
        /// <param name="jwtToken">JWT Token 字符串</param>
        /// <returns>包含令牌信息的对象</returns>
        public JwtTokenInfo ParseToken(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                // 直接解析令牌而不验证（如果只需要提取信息）
                var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

                // 提取所有声明
                var claims = new Dictionary<string, string>();
                foreach (var claim in jwtSecurityToken.Claims)
                {
                    claims[claim.Type] = claim.Value;
                }

                // 创建令牌信息对象
                var tokenInfo = new JwtTokenInfo
                {
                    Subject = jwtSecurityToken.Subject,
                    Issuer = jwtSecurityToken.Issuer,
                    Audience = GetAudience(jwtSecurityToken),
                    ValidFrom = jwtSecurityToken.ValidFrom,
                    ValidTo = jwtSecurityToken.ValidTo,
                    Claims = claims,
                    Header = JsonConvert.SerializeObject(jwtSecurityToken.Header),
                    SignatureAlgorithm = jwtSecurityToken.SignatureAlgorithm
                };

                return tokenInfo;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to parse JWT token: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从 JWT Token 中获取受众
        /// </summary>
        /// <param name="jwtToken">JwtSecurityToken 对象</param>
        /// <returns>受众字符串</returns>
        private string GetAudience(JwtSecurityToken jwtToken)
        {
            // JwtSecurityToken 的 Audience 属性可能为 null，需要从声明中获取
            var audClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud);
            return audClaim?.Value ?? jwtToken.Audiences?.FirstOrDefault();
        }

        /// <summary>
        /// 从 SecurityTokenDescriptor 重新生成 JWT Token
        /// </summary>
        /// <param name="descriptor">SecurityTokenDescriptor 对象</param>
        /// <returns>JWT Token 字符串</returns>
        public string GenerateTokenFromDescriptor(SecurityTokenDescriptor descriptor)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.CreateToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to generate token from descriptor: {ex.Message}", ex);
            }
        }
    }

}
