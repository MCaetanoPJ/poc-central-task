namespace CentralTask.Core.DTO.Broker
{
    public class MessageRequestModel
    {
        public string Message { get; set; }
        public string MessageType { get; set; }
        public bool Reprocess { get; set; }
        public string QueueEvent { get; set; }
        public Guid UserId { get; set; }
    }
}