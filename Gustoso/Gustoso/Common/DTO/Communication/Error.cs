namespace Gustoso.Common.DTO.Communication
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string description)
        {
            ErrorDescription = description;
            ErrorCode = 500;
        }

        public Error(int errorCode, string description)
        {
            ErrorDescription = description;
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; private set; }
        public string ErrorDescription { get; set; }
    }
}
