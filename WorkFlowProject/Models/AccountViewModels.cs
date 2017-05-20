using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WorkFlowProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Merci de spécifier l'Utilisateur")]
        [Display(Name = "Utilisateur")]
        [StringLength(100, ErrorMessage = "le champs {0} contient au moin {2} characters ", MinimumLength = 6)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Merci de saisir le Mot de passe")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Souvenir de moi ?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [StringLength(100, ErrorMessage = "minimum {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer Mot de passe")]
        [Compare("Password", ErrorMessage = "les deux mot de passe ne sont pas identique.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [StringLength(100, ErrorMessage = "minimum {2} characters .", MinimumLength = 6)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [StringLength(500, ErrorMessage = "minimum {2} characters .", MinimumLength = 7)]
        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de Naissance")]
        public DateTime Datenaiss { get; set; }


        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Telephone")]
        //[RegularExpression("^([2-5]|[7-9])[0-9]{8}", ErrorMessage = "Numero invalide")]
        [Phone]
        public string Tel { get; set; }


        [DataType(DataType.Upload)]
        [Display(Name = "Signature")]
        public HttpPostedFileBase Singimg { get; set; }

        [Description("Responsable Achat")]
        public bool RespAcha { get; set; }

        [Description("Demandeur")]
        public bool Demandeur { get; set; }

        [Description("Direction Administrative et Financière")]
        public bool DAF { get; set; }

        [Description("Direction Audit")]
        public bool Audit { get; set; }

        [Description("Direction Generale")]
        public bool DG { get; set; }

    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
