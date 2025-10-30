using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Data;

[Table("tasks")]
public class Tasks {
        
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        [Required]
        [Column("description")]
        public string text { get; set; }
        [Required]
        [Column("completed")]
        public bool completed {get; set; } = false;
        
        [Required]
        [Column("task_date")]
        public DateTime task_date {get; set; } = DateTime.Today;

        [Column("owner_id")]
        public int user_id { get; set; }

        [JsonIgnore]
        [Required]
        [ForeignKey(nameof(user_id))]
        public User user { get; set; }= null!;


        public Tasks() {}
        
        public Tasks(string text, int user_id, bool completed, DateTime task_date) {
                this.text = text;
                this.user_id = user_id;
                this.completed = completed;
                this.task_date = task_date;
        }
        public TaskDto IntoTaskDto()
                => new TaskDto(this.id, this.text, this.completed, this.task_date, this.user_id);
}
public record TaskDto(int id, string text, bool completed, DateTime task_date, int user_id);
