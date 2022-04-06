using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TelephoneDirectory.Validation;

namespace TelephoneDirectory.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryContact", controller: "Contact")]
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Cel { get; set; }
        public int NumberContact { get; set; }

    }
}
