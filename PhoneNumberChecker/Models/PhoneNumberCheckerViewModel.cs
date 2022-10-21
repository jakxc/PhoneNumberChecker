using System.ComponentModel.DataAnnotations;

namespace PhoneNumberChecker.Models
{
    public class PhoneNumberCheckViewModel
    {
        private string countryCodeSelected;

        [Required]
        [Display(Name = "Country")]
        public string CountryCodeSelected
        {
            get => countryCodeSelected;
            set => countryCodeSelected = value.ToUpperInvariant();
        }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is Valid")]
        public bool IsValid { get; set; }

        [Display(Name = "Is Possible")]
        public bool IsPossible { get; set; }

        [Display(Name = "Phone Type")]
        public string PhoneType { get; set; }

        [Display(Name = "International Format")]
        public string InternationalFormat { get; set; }
    }
}
