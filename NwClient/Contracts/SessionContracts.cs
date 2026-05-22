namespace NwClient.Contracts
{
    public class SessionContract
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ImageUrl { get; set; }
        public List<int> DifficultyIds { get; set; } = new();
    }

    public class CreateSessionContract
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ImageUrl { get; set; }
        public List<int> DifficultyIds { get; set; } = new();
    }

    public class DifficultyContract
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = "";
    }
}
