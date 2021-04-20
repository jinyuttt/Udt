using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using udt.NetCore;

namespace IUdtSocket
{
    public class UdtFactory
    {
     public   UDTSocket Create()
        {
            return new UDTSocket();
        }
        public UDTSocket Create(UdtAddressFamily family, UdtSocketType socketType)
        {
            return new UDTSocket(family, socketType);
        }
    }
}
