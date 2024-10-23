namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }

        //Navigation Property - Tells EF that walk will have Difficulty - Defines ONE TO ONE 
        public Difficulty Difficulty { get; set; }
        public Guid RegionId { get; set; }
        //Navigation Property - Tells EF that walk will have Region  - Defines ONE TO ONE 
        public Region Region { get; set; }

    }
}
