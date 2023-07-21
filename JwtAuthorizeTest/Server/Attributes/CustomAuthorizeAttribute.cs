
using Microsoft.AspNetCore.Authorization;
using System.Web;
namespace JwtAuthorizeTest.Server.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CustomAuthorizeAttribute: AuthorizeAttribute
{
    private readonly string[] allowedPermissions;
    public CustomAuthorizeAttribute(params string[] permissions)
    {
        this.allowedPermissions = permissions;
        
    }

    

    //protected override bool AuthorizeCore(HttpContext httpContext)
    //{
    //    bool authorize = false;
    //    foreach (var permission in allowedPermissions)
    //    {
    //        var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
    //        m.IsActive == true); // checking active users with allowed roles.  
    //        if (user.Count() > 0)
    //        {
    //            authorize = true; /* return true if Entity has current user(active) with specific role */
    //        }
    //    }
    //    return authorize;
    //}
    //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //{
    //    filterContext.Result = new HttpUnauthorizedResult();
    //}
}
