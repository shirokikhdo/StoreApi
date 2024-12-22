namespace Api.Common;

/// <summary>
/// Класс <c>SharedData</c> содержит общие данные, используемые в приложении.
/// </summary>
public static class SharedData
{
    /// <summary>
    /// Класс <c>Roles</c> определяет роли пользователей в системе.
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Роль администратора.
        /// </summary>
        public const string Admin = "admin";
        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public const string Consumer = "consumer";

        /// <summary>
        /// Получает список всех ролей.
        /// </summary>
        public static IReadOnlyList<string> AllRoles => 
            new List<string> {Admin, Consumer};
    }

    /// <summary>
    /// Класс <c>OrderStatus</c> определяет статусы заказов в системе.
    /// </summary>
    public static class OrderStatus
    {
        /// <summary>
        /// Статус заказа: ожидание.
        /// </summary>
        public const string Pending = "pending";
        /// <summary>
        /// Статус заказа: готов к отправке.
        /// </summary>
        public const string ReadyToSheep = "ready-to-sheep";
        /// <summary>
        /// Статус заказа: завершен.
        /// </summary>
        public const string Completed = "completed";

        /// <summary>
        /// Получает список всех статусов заказов.
        /// </summary>
        public static IReadOnlyList<string> AllStatus =>
            new List<string> { Pending, ReadyToSheep, Completed };
    }
}