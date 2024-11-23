namespace CustomerChurmPrediction.Entities.CompanyEntity
{
    public class Company : AbstractEntity
    {
        public string Name { get; set; } = null!;

        public string Story { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<string> OwnerIds { get; set; } = new List<string>();
    }
}
