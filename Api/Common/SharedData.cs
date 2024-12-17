namespace Api.Common;

public static class SharedData
{
    public static class Roles
    {
        public const string Admin = "admin";
        public const string Consumer = "consumer";

        public static IReadOnlyList<string> AllRoles => 
            new List<string> {Admin, Consumer};
    }

    public static class OrderStatus
    {
        public const string Pending = "pending";
        public const string ReadyToSheep = "ready-to-sheep";
        public const string Completed = "completed";

        public static IReadOnlyList<string> AllStatus =>
            new List<string> { Pending, ReadyToSheep, Completed };
    }
}