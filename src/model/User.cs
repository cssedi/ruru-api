using System;
using System.ComponentModel.DataAnnotations;

namespace ruru_api.src.model;

public class User
{
    [Key]
    public Guid Id {get; set;}
    [Required]
    [StringLength(25)]
    public string Name {get; set;}

    [Required]
    [StringLength(25)]
    public string Surname {get; set;}
    public int Age {get; set;}
    public float Weight {get;set;}
    public float Height {get; set;}
    public ICollection<Workout> Workouts {get; set;} = [];

}
