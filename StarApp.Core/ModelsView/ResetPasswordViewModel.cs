﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarApp.Core.ModelsView
{
    public class ResetPasswordViewModel
    {
        public string token {  get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
