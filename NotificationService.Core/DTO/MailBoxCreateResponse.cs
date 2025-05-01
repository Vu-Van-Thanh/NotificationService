namespace NotificationService.Core.DTO
{
    public class MailBoxCreateResponse
    {
        public string MailboxId { get; set; }
        public string EmployeeEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }   
    }
}
