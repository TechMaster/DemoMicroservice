using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Models
{
    public class EditUserViewModel : UserFormViewModel
    {
       public string Id { get; set; }

       public string AvatarPath { get; set; }

       public string PhoneNumber { get; set; }
    }
}
