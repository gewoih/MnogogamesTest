using SharedLibrary.Enums;

namespace SharedLibrary.Models
{
    public sealed class Message
    {
        public readonly Guid Id;
        public readonly DateTime CreationDate;
        public readonly string Data;

        public Message(string data)
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
            Data = data;
        }
    }
}
