using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Commands
    {
        [Key]
        public int Id { get; set; }
        public string? HowTo { get; set; }
        public string ?CommandLine { get; set; }
        public int? PlatformId { get; set; }
        public Platform ?Platform { get; set; }  
    }
}
