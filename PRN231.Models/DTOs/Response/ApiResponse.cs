namespace PRN231.Models.DTOs.Response
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(int code, string message,object data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
}
