using Microsoft.AspNetCore.Authorization;

namespace AuthApi.Helpers
{
    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params string[] roles )
        {
            Roles = string.Join( ",", roles );
        }
    }
}
