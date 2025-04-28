using System;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;

namespace myShopAPI.Models;


public class SystemUser : IdentityUser<Guid>
{
    public string Gender {get; set;}
    public DateTime BirthDate { get; set; }
}