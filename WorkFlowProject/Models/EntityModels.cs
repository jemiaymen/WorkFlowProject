using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Net.Http;

namespace WorkFlowProject.Models
{
    
    public class RestAvis
    {
        public int ID { get; set; }
        public string Lbl { get; set; }
        public string Email { get; set; }
    }

    public enum Choi
    {
        Category = 0,
        Fournisseur = 1,
        DemandeAccepter = 2

    }
    public class StatGenerale
    {
        public DateTime debu { get; set; }
        public DateTime fin { get; set; }
        public Choi choi { get; set; }
    }

    public class  Avancement
    {
        public Achat Achat { get; set; }
        public bool Dem { get; set; }
        public bool RespA { get; set; }
        public bool Daf { get; set; }
        public bool Audit { get; set; }
        public bool Dg { get; set; }
        public bool valid { get; set; }

        public Avancement()
        {
            Dem = false;
            RespA = false;
            Daf = false;
            Audit = false;
            Dg = false;
            valid = true;
        }
    }

    public class Historique
    {
        public IList<Achat> Accepted { get; set; }
        public IList<Refused> Refused { get; set; }
    }

    public class Refused
    {
        public Achat Achat { get; set; }
        public Avis Avis { get; set; }
    }

    public class Stat
    {
        public int Count { get; set; }
        public string Title { get; set; }
    }

    public class DemandeList
    {

        public IList<Notification> Notification { get; set; }

        public IList<Achat> Achat { get; set; }
    }

    public class Demande
    {

        public Notification Notification { get; set; }

        public IList<Avis> Avis { get; set; }

        public IList<AchaFournisseur> AF { get; set; }

        public Achat Achat { get; set; }

        public AchaFournisseur af { get; set; }

    }

    public enum AvisCode
    {
        init = 0,
        demandeur = 1,
        daf = 2,
        audit = 3,
        dg = 4
    }

    public enum TypeDemande
    {
        Besoin = 0,
        Demande = 1,
        BonCommande = 2,
        BonLivraison = 3
    }

    public enum StateDemande
    {
        BesoinCreer = 0,
        BesoinRefuser = 1,
        DemandeCreer = 2,
        DemandeConfirmerFournisseur = 3,
        DemandeAConfirmerFournisseur = 4,
        DemandeAModifier = 5,
        DemandeAccepter = 6,
        DemandeRefuser = 7,
        DemandeConfirmerDaf = 8,
        DemandeConfirmerAudit = 9,
        Reciption = 10
    }

    public enum StateFournisseur
    {
        Repond = 1,
        Chose = 2
    }

    public class Fournisseur
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Nom de fournisseur")]
        [StringLength(100, ErrorMessage = "minimum {2} characters .", MinimumLength = 4)]
        public string Nom_frn { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Adresse")]
        [StringLength(500, ErrorMessage = "minimum {2} characters .", MinimumLength = 10)]
        public string Adress_frn { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [EmailAddress]
        [Display(Name = "Mail")]
        public string Mail_frn { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Telephone")]
        [RegularExpression("^([2-5]|[7-9])[0-9]{8}",ErrorMessage = "Numero invalide") ]
        public string Tel_frn { get; set; }

    }

    public class Category
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Libellé de la catégorie")]
        [StringLength(100, ErrorMessage = "minimum {2} characters .", MinimumLength = 4)]
        [Index("IndexLblCategory", Order = 1, IsUnique = true )]
        public string Lbl { get; set; }

    }

    public class CategoryInFournisseur
    {
        [Key]
        [Column(Order = 0)]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [Key]
        [Column(Order = 1)]
        public int FournisseurID { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
    }

    public class Department
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Departement")]
        [StringLength(100, ErrorMessage = "minimum {2} characters .", MinimumLength = 4)]
        public string Dep { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Budget")]
        public float Budget { get; set; }

        [Display(Name = "Dépense")]
        public float Depense { get; set; }

    }

    public class Achat
    {
        public int ID { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Désignation")]
        [StringLength(500, ErrorMessage = "minimum {2} characters.", MinimumLength = 4)]
        public string Des { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Catégorie")]
        [StringLength(500, ErrorMessage = "minimum {2} characters .", MinimumLength = 4)]
        public string Categ { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date d'Achat")]
        public DateTime DtAcha { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Création")]
        public bool Creation { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Lieux de Livraison")]
        [StringLength(500, ErrorMessage = "minimum {2} characters .", MinimumLength = 4)]
        public string LieuLiv { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Imputation")]
        [StringLength(500, ErrorMessage = "minimum {2} characters.", MinimumLength = 4)]
        public string Imp { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Quantité")]
        public int Qte { get; set; }

        public TypeDemande Type { get; set; }

        public StateDemande State { get; set; }

        public virtual Department Department { get; set; }

        

        public Achat()
        {
            DtAcha = DateTime.Now;
            Type = TypeDemande.Besoin;
            State = StateDemande.BesoinCreer;
            
        }
    }

    public class Avis
    {
        public int ID { get; set; }

        [ForeignKey("Achat")]
        public int AchatID { get; set; }

        
        [ForeignKey("User")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Libellé")]
        [StringLength(500, ErrorMessage = "minimum {2} characters long.", MinimumLength = 4)]
        public string Lbl { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Code")]
        public AvisCode Code { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Acceptation")]
        public bool Accept { get; set; }

        public virtual Achat Achat { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
    
    public class AchaFournisseur
    {
        public int ID { get; set; }

        [ForeignKey("Achat")]
        public int AchatID { get; set; }

        [ForeignKey("Fournisseur")]
        public int FournisseurID { get; set; }

        [Required]
        [Display(Name = "Prix HT")]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Remise")]
        public float Remise { get; set; }

        [Required]
        [Display(Name = "Délais de Paiment")]
        public int Delais { get; set; }

        [Display(Name = "Etat")]
        public StateFournisseur State { get; set; }


        public virtual Fournisseur Fournisseur { get; set; }
        public virtual Achat Achat { get; set; }


    }

    public class Notification
    {
        public int ID { get; set; }

        [ForeignKey("Achat")]
        public int AchatID { get; set; }

        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Libellé")]
        [StringLength(500, ErrorMessage = "minimum {2} characters long.", MinimumLength = 4)]
        public string Lbl { get; set; }


        [Required(ErrorMessage = "Champ obligatoire (*)")]
        [Display(Name = "Date Notification")]
        public DateTime Dt { get; set; }

        [Required]
        [Display(Name = "Traiter")]
        public bool Etat { get; set; }


        [Required]
        public string Role { get; set; }

        public virtual Achat Achat { get; set; }

        public Notification()
        {
            Dt = DateTime.Now;
            Etat = false;
        }
    }



}