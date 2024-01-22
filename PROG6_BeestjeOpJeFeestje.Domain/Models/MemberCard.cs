using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public enum MemberCard
{
    [Display(Name = "None")] None,
    [Display(Name = "Silver")] Silver,
    [Display(Name = "Gold")] Gold,
    [Display(Name = "Platinum")] Platinum
}