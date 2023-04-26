using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public class SettingsConfig
    {
        // JWT
        public const string Id = "id";
        public const string SettingsJwt = "SecurityJwtSettings";
        public const string Secret = "SecurityJwtSettings:Secret";
        public const string ValidateAudience = "SecurityJwtSettings:ValidateAudience";
        public const string ValidateIssuer = "SecurityJwtSettings:ValidateIssuer";
        public const string RequireHttpsMetadata = "SecurityJwtSettings:RequireHttpsMetadata";
        public const string SaveToken = "SecurityJwtSettings:SaveToken";
        public const string ValidAudience = "SecurityJwtSettings:ValidAudience";
        public const string ValidIssuer = "SecurityJwtSettings:ValidIssuer";

        // SQL
        public const string ConnectionString = "DefaultConnection";
    }
}
