using System;
using System.Collections.Generic;
using SwimmiePooLogParser.Common.DTOs;
using SwimmiePooLogParser.Common.Domain.Models;

namespace SwimmiePooLogParser.Common
{
    public interface IMembershipService
    {
        AuthorizationWithRoles Login(UserInfo user);
    }

    public interface IUserManagementService
    {
        UserProfileInfo GetUser(string username);
        UserProfileInfo GetUser(int userId);
        void CreateUser(UserProfileInfo userProfile);
        void DeleteUser(string userName);
        void Register();
        bool Login(string userName, string password, bool persistCookie = false);
        bool ConfirmAccount(string accountConfirmationToken);
        void CreateAccount(string userName, string password, bool requireConfirmationToken = false);
        string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false);
        int GetUserId(string userName);
        IEnumerable<RoleInfo> GetUserRoles(string userName);
        IEnumerable<RoleInfo> GetAllActiveRoles();
        void Logout();
        void UpdateUserProfile(UserProfileInfo userProfile);
        bool IsAuthorized(Guid authorizationId);
        bool IsAuthenticated { get; }
        string CurrentUserName { get; }
        bool IsUserInRole(string userName, string roleName);
        //IEnumerable<UserProfileInfo> GetPagedUsers(int skip, int take);
       // int GetUserCount();
        RoleInfo GetActiveRole(int roleId);
        void SetUserRoles(UserProfileInfo userProfile, RoleInfo[] roles);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        string GetPasswordResetToken(string userName);
        bool AdminChangeUserPassword(string resetToken, string newPassword);
    }
}
