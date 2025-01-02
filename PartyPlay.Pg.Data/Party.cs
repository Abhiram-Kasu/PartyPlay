using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyPlay.Pg.Data;

public class Party
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public required string Name { get; set; }

    [ForeignKey(nameof(Owner))]
    public int OwnerId { get; set; }
    public PartyUser? Owner { get; set; } 
    
    
    public ICollection<PartyUser> Users { get; set; } = [];
    
    
}