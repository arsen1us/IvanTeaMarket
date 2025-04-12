﻿namespace CustomerChurmPrediction.Entities.TeaEntity
{
    public class Tea : AbstractEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Цена
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Тип упаковки
        /// </summary>
        public string? PackageType { get; set; } = null;

        /// <summary>
        /// Материалы упаковки
        /// </summary>
        public string? PackageMaterials { get; set; } = null;

        /// <summary>
        /// Вес продукта
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Дополнительная информация о весе продукта
        /// </summary>
        public string? WeightDetails { get; set; } = null;

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
        }
    }
}
