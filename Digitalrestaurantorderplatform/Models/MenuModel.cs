using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Digitalrestaurantorderplatform.Models;

public class MenuModel
{
    [Required]
    public string foodname{get; set;}
    [Required]
    public int amount{get;set;}
    [Required]
    public string categories{get;set;}
    public string caption{get;set;}

    public byte[] image{get;set;}



}