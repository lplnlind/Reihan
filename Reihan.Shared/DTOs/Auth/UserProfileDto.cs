﻿using Reihan.Shared.Enums;

namespace Reihan.Shared.DTOs
{
    public class UserProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
