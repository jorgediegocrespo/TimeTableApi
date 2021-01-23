using System;
using TimeTable.Application.Contracts.Models.Base;

namespace TimeTable.Application.Contracts.Models
{
    public class BankDay : IBaseBusinessModel
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public Company Company { get; set; }
    }
}
