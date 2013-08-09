using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwimmiePooLogParserDispatcher.Infrastructure.Security.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public System.DateTime? Expiration { get; set; }
        public System.Guid? ApiAuthorizationId { get; set; }
    }
}
