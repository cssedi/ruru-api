using System;
using System.ComponentModel.DataAnnotations;

namespace ruru_api.src.model;

public class User
{
    [Key]
    public Guid Id {get; init;}
    [Required]
    [StringLength(25)]
    public string Name {get; init;}
    [Required]
    [StringLength(25)]
    public string Surname {get; init;}
    public int Age {get; init;}
    public float Weight {get;init;}
    public float Height {get; init;}
    public ICollection<Workout> Workouts {get; init;} = new List<Workout>();

}
