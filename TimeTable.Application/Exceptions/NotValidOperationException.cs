using System;

namespace TimeTable.Application.Exceptions
{
    public class NotValidOperationException : Exception
    {
        public string Description { get; protected set; }

        public string Code { get; protected set; }

        public NotValidOperationException(string code, string description) : base(code)
        {
            Code = code;
            Description = description;
        }
    }
}
