namespace TimeTable.Application.Constants
{
    public abstract class ErrorCodes
    {
        public const string ITEM_NOT_EXISTS = "ITEM_NOT_EXISTS";

        public const string COMPANY_NAME_EXISTS = "COMPANY_NAME_EXISTS";
        public const string COMPANY_HAS_PEOPLE = "COMPANY_HAS_PEOPLE";
        public const string COMPANY_NOT_EXISTS = "COMPANY_NOT_EXISTS";

        public const string PERSON_NAME_EXISTS = "PERSON_NAME_EXISTS";

        public const string TIME_RECORD_OVERLAPPING_EXISTS = "TIME_RECORD_OVERLAPPING_EXISTS";

        public const string USER_REGISTER_ERROR = "USER_REGISTER_ERROR";
    }
}
