﻿using System;

namespace Auth.Domain
{
    public class User
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }
    }
}
