namespace TimeTable.Business.Models
{
    public class DeleteRequest
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class DeleteOwnRequest
    {
        public byte[] RowVersion { get; set; }
    }
}
