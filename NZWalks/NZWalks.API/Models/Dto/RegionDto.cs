namespace NZWalks.API.Models.Dto
{
    public class RegionDto
    {
        //we will exponse everything back to the client
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
