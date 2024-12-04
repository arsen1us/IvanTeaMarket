namespace CustomerChurmPrediction.Utils
{
    /// <summary>
    /// Тип действия пользователя
    /// </summary>
    public enum UserActionType
    {
        // Попытка регистрации
        RegistrationAttempt = 0,
        // Попытка входа в аккаунт
        AuthenticationAttempt = 1,
        // Добавление в корзину
        AddToCart = 2,
        // Добавление в избранное
        AddToFavorite = 3,
        // Переход по ссылке
        FollowTheLink = 4,

        // надо/не надо - не знаю

        // Перейти на страницу
        GoToPage = 5,
        // Перейти на страницу с продуктом
        GoToProductPage = 6
    }

    /// <summary>
    /// Тип объекта действия пользователя
    /// </summary>
    public enum UserActionObjectType
    {
        // Страница с продуктом (какое либо дейсмтвие с продуктом)
        Product,
        // Реклама
        Promotion,
        // Профиль компании
        CompanyProdile,
        Other
    }
}
