﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUser : MyEntityBase
    {
        [DisplayName("İsim"),
            StringLength(25, ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyisim"), StringLength(25)]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]  // images/user_12.jpg vs. örneği gibi.
        //Profil resminin üretilmesini istemiyorum kullanıcı kendi profilinden güncellesin attribute'u anlamına geliyor.
        public string ProfileImageFilename { get; set; }

        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActiveGuid { get; set; }
        

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
