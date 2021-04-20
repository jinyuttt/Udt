using IUdtSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdtCSharp
{
   public class SocketFactoty
    {

       static UdtFactory udtFactory = null;
        static SocketFactoty()
        {
            var managedAssemblyPath = Path.GetDirectoryName(typeof(SocketFactoty).Assembly.Location);
            var alc = new UdtLoadContext(managedAssemblyPath);
            udtFactory = UdtLoadContext.Init(alc);
        }
        public UdtFactory Factory
        {
            get { return udtFactory; }
        }
    }
}
