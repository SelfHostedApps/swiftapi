using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data {
        [Table("users")]
        public class User {
                [Key]
                [Column("id")]
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public int id { get; set; }
                
                [Required]
                [Column("email")]
                public string email { get; set; }
                
                [Required]
                [Column("password")]
                public string password { get; set; }
                
                [Required]
                [Column("username")]
                public string username { get; set; }
                
                [Column("preference")]
                public int preference { get; set; }=1;
                
                [Required]
                [Column("role")]
                public string[] roles {get;set;}

                public User(){}

                public User(string email, string username, string password, int preference, String[] roles){
                        this.email = email;
                        this.username = username;
                        this.password = password;
                        this.preference = preference;
                        this.roles = roles;
                }
                public UserDto IntoDto()
                        => new UserDto(this.email, this.username, this.preference, this.roles);
                
        }

        public record UserDto(string email, string username, int preference,string[] roles);
}
