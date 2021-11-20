using TimeTable.Business.Models.Base;

namespace TimeTable.Business.Models
{
    public class Company : IBaseBusinessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
