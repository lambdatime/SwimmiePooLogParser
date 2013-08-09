using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities
{
    [Table("ActiveRoles")]
    public class ActiveRole
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}