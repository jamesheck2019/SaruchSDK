using System;

namespace SaruchSDK
{
    public class SaruchException : Exception
    {
        public SaruchException(string errorMesage, int errorCode) : base(errorMesage) { }
    }
}
