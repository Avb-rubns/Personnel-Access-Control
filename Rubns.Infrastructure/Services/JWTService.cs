namespace Rubns.Infrastructure.Services
{
    internal class JWTService : IJWTService
    {

        public UserClaim JWTtoUserInfo(string jwt)
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
            return new UserClaim();
        }

        private UserClaim ParseUserFromJWT(string jwt)
        {
            UserClaim userInfo = new();

            // Separa el JWT
            var payload = jwt.Split('.')[1];

            // Convierte Base64URL a Base64 estándar y decodifica
            var jsonBytes = ParseBase64WithoutPadding(payload);
            using var doc = JsonDocument.Parse(jsonBytes);
            var root = doc.RootElement;

            userInfo.FirstName = root.GetProperty("firstName").GetString();
            userInfo.Email = root.GetProperty("email").GetString();
            userInfo.Status = bool.Parse(root.GetProperty("status").GetString());
            userInfo.FrindlyId = root.GetProperty("userId").GetString();
            userInfo.FrindlyRolId = root.GetProperty("rolId").GetString();
            userInfo.Expiration = root.GetProperty("exp").GetInt64();

            // role puede ser un array
            if (root.TryGetProperty("role", out var roleElement) && roleElement.ValueKind == JsonValueKind.Array)
            {
                userInfo.Role = roleElement[0].GetString(); // si siempre hay un solo rol
            }

            return userInfo;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            base64 = base64.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
