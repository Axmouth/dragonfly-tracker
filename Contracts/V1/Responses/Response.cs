using System.Collections.Generic;

namespace DragonflyTracker.Contracts.V1.Responses
{
    public class Response<T>
    {
        public Response() { }

        public Response(T response)
        {
            Data = response;
        }

        public Response(List<string> Errors)
        {
            this.Errors = Errors;
        }

        public Response(T response, IEnumerable<string> Errors)
        {
            Data = response;
            this.Errors = Errors;
        }

        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}