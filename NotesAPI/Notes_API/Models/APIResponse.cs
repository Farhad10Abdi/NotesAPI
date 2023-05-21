using System.Net;

namespace Notes_API.Models
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public Object result { get; set; }
        
    }
}
