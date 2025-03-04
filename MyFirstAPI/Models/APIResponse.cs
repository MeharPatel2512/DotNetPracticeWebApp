using System.ComponentModel.DataAnnotations;

namespace MyFirstAPI.Models
{
    public class ApiResponse<T>
    {
        public string ?Message { get; set; }
        [Required]
        public String ?Code { get; set; }
        [Required]
        public bool Error { get; set; }
        public T? Response { get; set; }

        public ApiResponse (T ?data, string Message, string Code, bool error_status)
        {
                this.Message = Message;
                this.Code = Code;
                Error = error_status;
                Response = data;
        }
    }
}