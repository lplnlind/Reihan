﻿namespace Application.DTOs
{
    public class AddressDto
    {
        public string State { get; private set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
    }
}
