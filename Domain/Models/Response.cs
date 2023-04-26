using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Modelo de respuesta generico
    /// </summary>
    public class Response<T>
    {
        public int Code { get; set; } = 0;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
