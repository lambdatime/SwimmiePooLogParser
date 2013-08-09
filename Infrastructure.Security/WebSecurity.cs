using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities;
using SwimmiePooLogParserDispatcher.Infrastructure.Security.Repositories;
using WebMatrix.WebData;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security
{
    public static class WebSecurity
    {
        public static UserProfile GetUser(string username)
        {
            var uow = new UnitOfWork();
            return uow.UserProfileRepository.Get(u => u.UserName == username).SingleOrDefault();
        }

        public static UserProfile GetUser(int userId)
        {
            var uow = new UnitOfWork();
            return uow.UserProfileRepository.Get(u => u.UserId == userId).SingleOrDefault();
        }

        public static UserProfile GetCurrentUser()
        {
            return GetUser(CurrentUserName);
        }

        public static void CreateUser(UserProfile user)
        {
            var dbUser = GetUser(user.UserName);
            if (dbUser != null)
                throw new Exception("User with that username already exists.");
            var uow = new UnitOfWork();
            uow.UserProfileRepository.Insert(user);
            uow.Save();
        }

        public static void DeleteUser(string userName)
        {
            var dbUser = GetUser(userName);
            if (dbUser == null)
                throw new Exception("User with that username does not exist.");

            SetUserRoles(dbUser, new string[]{});

            var membershipProvider = (SimpleMembershipProvider)System.Web.Security.Membership.Provider;
            membershipProvider.DeleteAccount(userName);

            var uow = new UnitOfWork();
            uow.UserProfileRepository.Delete(dbUser);
            uow.Save();
        }

        public static void UpdateUserProfile(UserProfile user)
        {
            var dbUser = GetUser(user.UserId);
            if (dbUser == null) return;

            dbUser.UserName = user.UserName;
            dbUser.Email = user.Email;
            dbUser.ApiAuthorizationId = user.ApiAuthorizationId;
            dbUser.Expiration = user.Expiration;

            var uow = new UnitOfWork();
            uow.UserProfileRepository.Update(dbUser);
            uow.Save();
        }

        public static void SetUserRoles(UserProfile user, string[] roles)
        {
            var roleProvider = (SimpleRoleProvider)System.Web.Security.Roles.Provider;

            var existingRoles = GetUserRoles(user.UserName).Select(r => r.RoleName).ToArray();
            var existingRolesNeedingRemoved = existingRoles.Except(roles).ToArray();
            if (roles.Any())
            {
                roleProvider.AddUsersToRoles(new[] {user.UserName}, roles.Except(existingRoles).ToArray());
                roleProvider.RemoveUsersFromRoles(new[] { user.UserName }, existingRolesNeedingRemoved);
            }
            else
                roleProvider.RemoveUsersFromRoles(new[] {user.UserName}, existingRoles);
        }

        public static void Register()
        {
            Database.SetInitializer<SecurityContext>(null);
            var context = new SecurityContext();
            context.Database.Initialize(true);
            if (!WebMatrix.WebData.WebSecurity.Initialized)
                WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("MembershipConnection",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        public static bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebMatrix.WebData.WebSecurity.Login(userName, password, persistCookie);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebMatrix.WebData.WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }

        public static string GetPasswordResetToken(string userName)
        {
            return WebMatrix.WebData.WebSecurity.GeneratePasswordResetToken(userName);
        }

        public static bool AdminChangeUserPassword(string resetToken, string newPassword)
        {
            return WebMatrix.WebData.WebSecurity.ResetPassword(resetToken, newPassword);
        }

        public static bool ConfirmAccount(string accountConfirmationToken)
        {
            return WebMatrix.WebData.WebSecurity.ConfirmAccount(accountConfirmationToken);
        }

        public static void CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            WebMatrix.WebData.WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public static string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false)
        {
            return WebMatrix.WebData.WebSecurity.CreateUserAndAccount(userName, password, new { Email = email }, requireConfirmationToken);
        }

        public static int GetUserId(string userName)
        {
            return WebMatrix.WebData.WebSecurity.GetUserId(userName);
        }

        public static IEnumerable<ActiveRole> GetUserRoles(string userName)
        {
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            var uow = new UnitOfWork();
            var roleNames = roles.GetRolesForUser(userName);
            return uow.ActiveRoleRepository.Get(r => roleNames.Contains(r.RoleName));
        }

        public static IEnumerable<ActiveRole> GetAllActiveRoles()
        {
             var uow = new UnitOfWork();
            return uow.ActiveRoleRepository.Get(role => true);
        }

        public static ActiveRole GetActiveRole(int roleId)
        {
            var uow = new UnitOfWork();
            return uow.ActiveRoleRepository.Get(role => role.RoleId == roleId).FirstOrDefault();
        }

        public static bool IsUserInRole(string userName, string roleName)
        {
            var roles = (SimpleRoleProvider)System.Web.Security.Roles.Provider;
            return roles.IsUserInRole(userName, roleName);
        }

        public static void Logout()
        {
            WebMatrix.WebData.WebSecurity.Logout();
        }

        public static bool IsAuthorized(Guid authorizationId)
        {
            var uow = new UnitOfWork();
            var user = uow.UserProfileRepository.Get(u => u.ApiAuthorizationId == authorizationId).FirstOrDefault();
            if (user == null) return false;

            return user.ApiAuthorizationId == authorizationId && DateTime.Now < user.Expiration;
        }

        public static bool IsAuthenticated { get { return WebMatrix.WebData.WebSecurity.IsAuthenticated; } }

        public static string CurrentUserName { get { return WebMatrix.WebData.WebSecurity.CurrentUserName; } }

        public static IEnumerable<UserProfile> GetUsers()
        {
            var uow = new UnitOfWork();
            return uow.UserProfileRepository.Get(profile => true);
        }

        public static int GetUserCount()
        {
            var uow = new UnitOfWork();
            return uow.Context.Set<UserProfile>().Count();
        }
    }
}
