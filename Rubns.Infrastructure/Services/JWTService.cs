namespace Rubns.Infrastructure.Services
{
    internal class JWTService : IJWTService
    {

        public UserInfoDTO JWTtoUserInfo(string jwt)
        {
            var userinfo = ParseUserFromJWT(jwt);

            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            var expiration = DateTimeOffset.FromUnixTimeSeconds(userinfo.Expiration).DateTime;
            DateTime nzDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, nzTimeZone);
            DateTime expirationDateTime = TimeZoneInfo.ConvertTime(expiration, TimeZoneInfo.Utc, nzTimeZone);

            if (expirationDateTime >= nzDateTime)
            {
                return userinfo;

            }
            return new UserInfoDTO();
        }

        private IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64Withoutpadding(payload);
            var keyValuesPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            List<Claim> result = new();
            Claim aux;
            foreach (var data in keyValuesPairs)
            {
                if (data.Key == "role")
                {
                    var aux_role = data.Value.ToString().Replace("[\"", "");
                    aux_role = aux_role.Replace("\"]", "");
                    aux = new Claim("role", aux_role);
                    result.Add(aux);
                }
                else
                {
                    aux = new Claim(data.Key, data.Value.ToString());
                    result.Add(aux);
                }

            }

            return result;
        }

        private UserInfoDTO ParseUserFromJWT(string jwt)
        {
            UserInfoDTO userInfo = new UserInfoDTO();

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64Withoutpadding(payload);
            var keyValuesPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            userInfo.Email = keyValuesPairs["email"].ToString();
            userInfo.FirstName = keyValuesPairs["firstName"].ToString();
            userInfo.Status = Convert.ToBoolean(keyValuesPairs["status"].ToString());
            userInfo.ID = Convert.ToInt32(keyValuesPairs["userId"].ToString());
            userInfo.Expiration = Convert.ToInt64(keyValuesPairs["exp"].ToString());
            var aux_role = keyValuesPairs["role"].ToString().Replace("[\"", "").Replace("\"]", "");
            userInfo.Role = aux_role;


            return userInfo;
        }

        private byte[] ParseBase64Withoutpadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
