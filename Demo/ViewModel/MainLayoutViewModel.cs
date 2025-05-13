using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Demo.Common;
using DBAccess.Model;

namespace Demo.ViewModel
{
    public class MainLayoutViewModel
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        [Display(Name = "ユーザーID")]
        [Required(ErrorMessage = MessageClass.EM0001)]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Display(Name = "パスワード")]
        [Required(ErrorMessage = MessageClass.EM0001)]
        public string PassWord { get; set; } = string.Empty;
    }
}
