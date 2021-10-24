using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.Dtos
{
    public class EmailSendDto
    {
        [DisplayName("Ad")]
        [Required(ErrorMessage ="{0} alanı zorunludur")]
        [MaxLength(60,ErrorMessage ="{0} alanı en fazla {1} karakter olmalı")]
        [MinLength(5, ErrorMessage = "{0} alanı en faz {1} karakter olmalı")]
        public string Name { get; set; }
        [DisplayName("Mail Adresi")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        [MaxLength(100, ErrorMessage = "{0} alanı en fazla {1} karakter olmalı")]
        [MinLength(10, ErrorMessage = "{0} alanı en faz {1} karakter olmalı")]
        public string Email { get; set; }
        [DisplayName("Konu")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        [MaxLength(125, ErrorMessage = "{0} alanı en fazla {1} karakter olmalı")]
        [MinLength(5, ErrorMessage = "{0} alanı en faz {1} karakter olmalı")]
        public string Subject { get; set; }
        [DisplayName("Mesaj")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        [MaxLength(1500, ErrorMessage = "{0} alanı en fazla {1} karakter olmalı")]
        [MinLength(5, ErrorMessage = "{0} alanı en faz {1} karakter olmalı")]
        public string Message { get; set; }
    }
}
