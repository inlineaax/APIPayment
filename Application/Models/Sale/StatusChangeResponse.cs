namespace Application.Models.Sale
{
    public class StatusChangeResponse
    {
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string Message { get; set; }

        public StatusChangeResponse(string oldStatus, string newStatus, string message)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Message = message;

        }
    }
}
