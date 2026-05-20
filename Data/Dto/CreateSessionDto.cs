namespace Data.Dto
{
    public class CreateSessionDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ImageUrl { get; set; }
        public List<int> DifficultyIds { get; set; } = new();
    }
}
