namespace Gustoso.Common.DTO.Communication
{
    public class Response<T>
    {
        public T Data { get; set; }

        public Error Error { get; set; }
    }
}
