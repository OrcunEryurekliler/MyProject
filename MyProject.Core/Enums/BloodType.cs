using System.ComponentModel.DataAnnotations;

namespace MyProject.Core.Enums
{
    public enum BloodType
    {
        [Display(Name = "A+")] APozitif,
        [Display(Name = "A-")] ANegatif,
        [Display(Name = "B+")] BPozitif,
        [Display(Name = "B-")] BNegatif,
        [Display(Name = "AB+")] ABPozitif,
        [Display(Name = "AB-")] ABNegatif,
        [Display(Name = "0+")] OPozitif,
        [Display(Name = "0-")] ONegatif
    }
}
