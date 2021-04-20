using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace udt.NetCore
{
    public enum UdtSocketType
    {
        SOCK_STREAM = 1,
        SOCK_DGRAM = 2,

    }

    public enum UdtAddressFamily
    {
        InterNetwork=2,
        InterNetworkV6=23,
    }
}
