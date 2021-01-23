using TimeTable.Application.Contracts.Models.Base;

namespace TimeTable.Application.Contracts.Models
{
    public class VacationDay : IBaseBusinessModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int VacationDays { get; set; }
        public Person Person { get; set; }
    }
}
