namespace Customer.API.Models
{
    public class CustomerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Married { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }
        public string[] Teams { get; set; }

        public PhoneNumber[] PhoneNumbers { get; set; }

    }


    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }

    }



}
