using System;
using System.Collections.Generic;

namespace SwimmiePooLogParser.Common.DTOs
{
    public class AuthorizationWithRoles
    {
        public IEnumerable<string> Roles { get; set; }
        public Guid AuthorizationId { get; set; }
    }
}