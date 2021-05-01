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

        static Type type = null;
        static SocketFactoty()
        {
            var managedAssemblyPath = Path.GetDirectoryName(typeof(SocketFactoty).Assembly.Location);
            var alc = new UdtLoadContext(managedAssemblyPath);
            type= UdtLoadContext.Init(alc);
        }

       public   static IUdtSocket.IUdtSocket Create()
        {
            return (IUdtSocket.IUdtSocket)Activator.CreateInstance(type);
        }
        public static IUdtSocket.IUdtSocket Create(int family, int sockTyp)
        {
            return (IUdtSocket.IUdtSocket)Activator.CreateInstance(type,family,sockTyp);
        }

    }
}
