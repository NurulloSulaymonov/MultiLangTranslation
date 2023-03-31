using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data.Entities;

public class Post
{
    public int Id { get; set; }
    [Column(TypeName = "jsonb")]
    public Dictionary<string,string> Title { get; set; }
    [Column(TypeName = "jsonb")]
    public Dictionary<string,string> Description { get; set; }
}

