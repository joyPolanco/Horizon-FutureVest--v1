namespace Application.Dtos.EconomicIndicator
{
    internal class IndicatorScoreDto
    {
        public int IndicatorId { get; set; }
        public decimal Value { get; set; }
        public decimal SubScore { get; set; }
    }
}