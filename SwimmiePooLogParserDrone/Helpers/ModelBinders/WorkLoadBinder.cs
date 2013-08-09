using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using SwimmiePooLogParser.Common.DTOs;

namespace SwimmiePooLogParserDrone.UI.Helpers.ModelBinders
{
    public class WorkLoadBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var resultWorkLoad = new WorkLoad();
            if (bindingContext.ModelType == typeof(WorkLoad))
            {
                var content = actionContext.ControllerContext.Request.Content.ReadAsStringAsync().Result;
            }

            bindingContext.Model = resultWorkLoad;
            return true;
        }
    }

    /*public class UserWithRolesDisplayModelBinder : IModelBinder
    {
        private IUserManagementService UserManagementService
        {
            get { return DependencyResolver.Get<IUserManagementService>(); }
        }
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var userDisplay = new UserDisplay();
            if (bindingContext.ModelType == typeof(UserDisplay))
            {

                var content = actionContext.ControllerContext.Request.Content.ReadAsStringAsync().Result;
                var decodedContent = HttpUtility.UrlDecode(content);
                var decodedContents = decodedContent.Split('&');
                var userValueStart = decodedContents[0].IndexOf("user=") + 5;
                var rolesValueStart = decodedContents[1].IndexOf("roles=") + 6;

                var userValue = decodedContents[0].Substring(userValueStart);
                var user = JsonConvert.DeserializeObject<UserDisplay>(userValue);
                var retrievedUser = UserManagementService.GetUser(user.UserName);

                var rolesValue = decodedContents[1].Substring(rolesValueStart);
                var roles = JsonConvert.DeserializeObject<IEnumerable<RoleDisplay>>(rolesValue).ToList();
                roles.ForEach(r =>
                {
                    var activeRole = UserManagementService.GetActiveRole(r.RoleId);
                    r.RoleName = activeRole.RoleName;
                });
                userDisplay = new UserDisplay
                {
                    UserId = retrievedUser.UserId,
                    UserName = retrievedUser.UserName,
                    Email = retrievedUser.Email,
                    UserRoles = roles
                };
            }
            bindingContext.Model = userDisplay;
            return true;
        }
    }*/
}