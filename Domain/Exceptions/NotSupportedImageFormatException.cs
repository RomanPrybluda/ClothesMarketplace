using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotSupportedImageFormatException : Exception
    {
        public NotSupportedImageFormatException(string message) 
            : base(message){}

        public NotSupportedImageFormatException() : base("The provided image format is not supported.")
        {
        }
    }
}
