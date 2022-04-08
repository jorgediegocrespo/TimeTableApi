namespace TimeTable.Application.Constants
{
    public abstract class ErrorCodes
    {
        public const string ITEM_NOT_EXISTS = "ITEM_NOT_EXISTS";
        public const string NO_COMPANY_CREATED = "NO_COMPANY_CREATED";
        public const string PERSON_NAME_EXISTS = "PERSON_NAME_EXISTS";
        public const string PERSON_DEFAULT = "PERSON_DEFAULT";
        public const string TIME_RECORD_OVERLAPPING_EXISTS = "TIME_RECORD_OVERLAPPING_EXISTS";
        public const string USER_REGISTER_ERROR = "USER_REGISTER_ERROR";
        public const string USER_UPDATING_ROLE_ERROR = "USER_UPDATING_ROLE_ERROR";
        public const string CONCURRENCY_ERROR = "CONCURRENCY_ERROR";
        public const string INVALID_ROLE = "INVALID_ROLE";
    }
}
