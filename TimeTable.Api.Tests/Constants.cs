namespace TimeTable.Api.Tests
{
    public class Constants
    {
        public const string AdminEmail = "Admin";
        public const string AdminName = "admin@email.com";
        public const string AdminPassword = "Admin_1234";

        public const string EmployeeEmail = "Employee";
        public const string EmployeeName = "employee@email.com";
        public const string EmployeePassword = "Employee_1234";
    }

    public class PeopleInfo
    {
        public static string AdminNameIdentifier { get; set; }
        public static int AdminId { get; set; }
        public static byte[] AdminRowVersion { get; set; }
        public static string EmployeeNameIdentifier { get; set; }
        public static int EmployeeId { get; set; }
        public static byte[] EmployeeRowVersion { get; set; }
    }
}
