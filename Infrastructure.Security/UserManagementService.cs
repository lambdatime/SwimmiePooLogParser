using System;
using System.Collections.Generic;
using System.Linq;
using SwimmiePooLogParser.Common;
using SwimmiePooLogParser.Common.Domain.Models;
using SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security
{
    public class UserManagementService : IUserManagementService
    {
        public UserProfileInfo GetUser(string username)
        {
            var user = WebSecurity.GetUser(username);
            return new UserProfileInfo
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    ApiAuthorizationId = user.ApiAuthorizationId,
                    Expiration = user.Expiration
                };
        }

        public UserProfileInfo GetUser(int userId)
        {
            var user = WebSecurity.GetUser(userId);
            return new UserProfileInfo
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                ApiAuthorizationId = user.ApiAuthorizationId,
                Expiration = user.Expiration
            };
        }

        public void CreateUser(UserProfileInfo userProfile)
        {
            var user = new UserProfile
                {
                    UserName = userProfile.UserName,
                    Email = userProfile.Email
                };
            WebSecurity.CreateUser(user);
        }

        public void DeleteUser(string userName)
        {
            WebSecurity.DeleteUser(userName);
        }

        public void UpdateUserProfile(UserProfileInfo userProfile)
        {
            var user = new UserProfile
            {
                UserId = userProfile.UserId,
                UserName = userProfile.UserName,
                Email = userProfile.Email,
                ApiAuthorizationId = userProfile.ApiAuthorizationId,
                Expiration = userProfile.Expiration
            };
            WebSecurity.UpdateUserProfile(user);
        }

        public void SetUserRoles(UserProfileInfo userProfile, RoleInfo[] roles)
        {
            var user = new UserProfile
            {
                UserId = userProfile.UserId,
                UserName = userProfile.UserName,
                Email = userProfile.Email,
                ApiAuthorizationId = userProfile.ApiAuthorizationId,
                Expiration = userProfile.Expiration
            };
            WebSecurity.SetUserRoles(user, roles.Select(r => r.RoleName).ToArray());
        }

        public void Register()
        {
            WebSecurity.Register();
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }

        public string GetPasswordResetToken(string userName)
        {
            return WebSecurity.GetPasswordResetToken(userName);
        }

        public bool AdminChangeUserPassword(string resetToken, string newPassword)
        {
            return WebSecurity.AdminChangeUserPassword(resetToken, newPassword);
        }

        public bool ConfirmAccount(string accountConfirmationToken)
        {
            return WebSecurity.ConfirmAccount(accountConfirmationToken);
        }

        public void CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, email, requireConfirmationToken);
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public IEnumerable<RoleInfo> GetUserRoles(string userName)
        {
            return WebSecurity.GetUserRoles(userName).Select(r => new RoleInfo
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName
            });
        }

        public IEnumerable<RoleInfo> GetAllActiveRoles()
        {
            return WebSecurity.GetAllActiveRoles().Select(r => new RoleInfo
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName
                });
        }

        public RoleInfo GetActiveRole(int roleId)
        {
            var role = WebSecurity.GetActiveRole(roleId);
            return role == null ? null : new RoleInfo
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                };
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            return WebSecurity.IsUserInRole(userName, roleName);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool IsAuthorized(Guid authorizationId)
        {
            return WebSecurity.IsAuthorized(authorizationId);
        }

        public bool IsAuthenticated { get { return WebSecurity.IsAuthenticated; } }

        public string CurrentUserName { get { return WebSecurity.CurrentUserName; } }

        //public IEnumerable<UserProfileInfo> GetPagedUsers(int skip, int take)
        //{
        //    return WebSecurity.GetUsers().Skip(skip).Take(take).Select(x => new UserProfileInfo
        //        {
        //            UserId = x.UserId,
        //            UserName = x.UserName,
        //            Email = x.Email
        //        });
        //}

        //public int GetUserCount()
        //{
        //    return WebSecurity.GetUserCount();
        //}
    }
}