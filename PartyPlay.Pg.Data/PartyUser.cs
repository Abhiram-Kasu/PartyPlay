using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlay.Pg.Data;

public class PartyUser
{
    [Required,MaxLength(100)]
    public required string Name { get; set; }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int? PartyId { get; set; }
    public Party? Party { get; set; }
    
    public bool IsOwner { get; set; }
}