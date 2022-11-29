namespace SharedLibrary.Models
{
    public sealed class Message
    {
        public readonly Guid Id;
        public readonly DateTime CreationDate;
        public readonly string Data;

        public Message(string data, DateTime creationDate)
        {
            Id = Guid.NewGuid();
            CreationDate = creationDate;
            Data = data;
        }

        public override string ToString()
        {
            return $"[Id: {Id}][Created: {CreationDate}][Data: {Data}]";
        }
    }
}
