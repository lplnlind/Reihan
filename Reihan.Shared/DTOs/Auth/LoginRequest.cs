﻿namespace Reihan.Shared.DTOs
{
    public class LoginRequest
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
