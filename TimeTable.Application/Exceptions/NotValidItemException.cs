using System;

namespace TimeTable.Application.Exceptions
{
    public class NotValidItemException : Exception
    {
        public string Description { get; protected set; }

        public string Code { get; protected set; }

        public NotValidItemException(string code, string description) : base(description)
        {
            Code = code;
            Description = description;
        }
    }
}
