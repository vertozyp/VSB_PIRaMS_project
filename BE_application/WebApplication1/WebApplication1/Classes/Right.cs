using Microsoft.Extensions.Primitives;
using NHibernate.Util;

namespace WebApplication1.Classes
{
    public class Right
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsEmployee { get; set; }

        public Right(int UserId, string Username, bool isEmployee)
        {
            this.UserId = UserId;
            this.Username = Username;
            this.IsEmployee = isEmployee;
        }

        public static Right? parseRequestAuthentication (HttpRequest request)
        {
            StringValues userId;
            StringValues username;
            StringValues isEmployee;
            if (
                request.Headers.TryGetValue("X-AUTH-USERID", out userId) &&
                request.Headers.TryGetValue("X-AUTH-USERNAME", out username) &&
                request.Headers.TryGetValue("X-AUTH-ISEMPLOYEE", out isEmployee)
                )
                    return new Right(int.Parse(userId.First()), username.FirstOrDefault(), bool.Parse(isEmployee.First()));
            else    return null;
        }
    }
}
