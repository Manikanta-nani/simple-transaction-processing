namespace Customer.API.Models
{
    public class UpdateModel
    {       

        public string Key { get; set; }

        public UpdateEntry[] UpdateEntries { get; set; }
    }

    public class UpdateEntry
    {
        public string Value { get; set; }

        public string Path { get; set; }
    }
}
