using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using Dii_TheaterManagement_Web.Clients;

namespace Dii_TheaterManagement_Web.Features.FakeUsers
{
    public class FakeUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FakeUserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserSignedIn()
        {
            return _httpContextAccessor.HttpContext.Session.Keys.Contains("FakeUser");
        }

        public FakeUser GetSignedInUser()
        {
            if (!_httpContextAccessor.HttpContext.Session.Keys.Contains("FakeUser"))
            {
                return null;
            }
            string json = _httpContextAccessor.HttpContext.Session.GetString("FakeUser");
            return JsonConvert.DeserializeObject<FakeUser>(json);
        }

        public void SignInUser(FakeUser user)
        {
            if (user != null)
            {
                string json = JsonConvert.SerializeObject(user, Formatting.None);
                _httpContextAccessor.HttpContext.Session.SetString("FakeUser", json);
            }
        }

        public void SignOutCurrentUser()
        {
            _httpContextAccessor.HttpContext.Session.Remove("FakeUser");
        }

        public bool TryGetTheaterInfo(out string theaterCode)
        {
            if (!IsUserSignedIn())
            {
                theaterCode = null;
                return false;
            }
            var user = GetSignedInUser();
            theaterCode = user.TheaterCode;
            return true;
        }
    }
}