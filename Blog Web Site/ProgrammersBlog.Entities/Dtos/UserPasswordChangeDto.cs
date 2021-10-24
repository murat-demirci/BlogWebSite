﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.Dtos
{
    public class UserPasswordChangeDto
    {
        [DisplayName("Eski Şifre")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DisplayName("Yeni Şifre")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DisplayName("Yeni Şifre Tekrar")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden fazla olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Şifre Aynı Olmalı")]
        public string RepeatPassword { get; set; }
    }
}
