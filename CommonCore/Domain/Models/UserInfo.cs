using System;
using System.Runtime.Serialization;

namespace SwimmiePooLogParser.Common.Domain.Models
{
    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string[] Roles { get; set; }

        [DataMember]
        public Guid AuthorizationId { get; set; }
    }

    public class RoleInfo
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserProfileInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public System.DateTime? Expiration { get; set; }
        public System.Guid? ApiAuthorizationId { get; set; }
    }
}
