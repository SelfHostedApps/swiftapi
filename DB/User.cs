using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

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
        [Column("roles")]
        public string roles {get;set;}
                
        public ICollection<Tasks> tasks { get; set; } = new List<Tasks>();

        public User(){}

        public User(string email, string username, string password, int preference, string roles){
                this.email = email;
                this.username = username;
                this.password = password;
                this.preference = preference;
                this.roles = roles;
        }
        public UserDto IntoDto()
                => new UserDto(this.id, this.email, this.username, this.preference, this.roles);
}

public record UserDto(int id, string email, string username, int preference,string role);

