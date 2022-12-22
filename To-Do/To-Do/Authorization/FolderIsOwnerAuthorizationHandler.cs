using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using To_Do.Models;
using To_Do.Areas.Identity.Data;

namespace To_Do.Authorization
{
    public class FolderIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Folder>
    {
        UserManager<To_DoUser> _userManager;

        public FolderIsOwnerAuthorizationHandler(UserManager<To_DoUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Folder resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            //If not asking for CRUD permission, return.

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
