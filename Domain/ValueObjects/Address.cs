namespace Domain.ValueObjects
{
    public class Address
    {
        public string State { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }

        public Address(string state, string street, string city, string zipCode)
        {
            State = state;
            Street = street;
            City = city;
            ZipCode = zipCode;
        }

        public Address() { }
    }
}
