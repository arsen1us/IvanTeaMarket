using Newtonsoft.Json;

namespace CustomerChurmPrediction.Entities.TeaEntity
{
    public class Tea : AbstractEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; } = null;

        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
        /// Колимчсетво чая
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Описание чая
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Тип упаковки
        /// </summary>
        [JsonProperty("packageType")]
        public string? PackageType { get; set; } = null;

        /// <summary>
        /// Материалы упаковки
        /// </summary>
        [JsonProperty("packageMaterials")]
        public string? PackageMaterials { get; set; } = null;

        /// <summary>
        /// Вес продукта
        /// </summary>
        [JsonProperty("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// Дополнительная информация о весе продукта
        /// </summary>
        [JsonProperty("weightDetails")]
        public string? WeightDetails { get; set; } = null;

        /// <summary>
		/// Ссылки на изображения
		/// </summary>
        [JsonProperty("imageSrcs")]
        public List<string>? ImageSrcs { get; set; }

        /// <summary>
        /// Id категории чая
        /// </summary>
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; } = null!;

        public Tea()
        {
            
        }

        public Tea(TeaAddDto teaAddDto)
        {
            Name = teaAddDto.Name;
            Price = teaAddDto.Price;
            PackageType = teaAddDto.PackageType;
            PackageMaterials = teaAddDto.PackageMaterials;
            Weight = teaAddDto.Weight;
            WeightDetails = teaAddDto.WeightDetails;
            Description = teaAddDto.Description;
            Count = teaAddDto.Count;
            CategoryId = teaAddDto.CategoryId;
        }
    }
}
