using CustomerChurmPrediction.Entities.UserEntity;

namespace CustomerChurmPrediction.Entities.ReviewEntity
{
    public class ReviewModel
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Отзыв
        /// </summary>
        public Review Review { get; set; } = null!;
    }
}
