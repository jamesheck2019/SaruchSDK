using System;
using System.Collections.Generic;
using System.Text;
using SaruchSDK;

namespace SaruchSDK
{
    public class ConnectionSettings
    {
        public TimeSpan? TimeOut = null;
        public bool? CloseConnection = true;
        public ProxyConfig Proxy = null;
    }
}
