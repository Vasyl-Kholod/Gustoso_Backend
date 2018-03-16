namespace Gustoso.Common.Interfaces
{
    public interface IContactUsDTO
    {
        string clientName { get; set; }

        string clientEmail { get; set; }

        string clientSubject { get; set; }

        string clientMessage { get; set; }
    }
}
