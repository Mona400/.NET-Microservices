using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        public int? ExternalID { get; set; }
        public string ?Name { get; set; }
        public ICollection<Commands> ?Commands { get; set; }=new List<Commands>();
    }
}
