﻿namespace Reihan.Client.Models
{
    public class UpdateProfileRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public AddressDto Address { get; set; } = new AddressDto();
    }
}
