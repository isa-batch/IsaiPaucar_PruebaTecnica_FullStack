using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public Response()
        {
            Success = true;
            Errors = new List<string>();
        }

        public Response(T data, string message = "Operación exitosa")
        {
            Success = true;
            Data = data;
            Message = message;
            Errors = new List<string>();
        }

        public Response(string message, bool success = false)
        {
            Success = success;
            Message = message;
            Errors = new List<string>();
        }

        public Response(List<string> errors)
        {
            Success = false;
            Message = "Ocurrieron errores";
            Errors = errors;
        }
    }
}
