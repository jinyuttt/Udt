using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdtCSharp;

namespace Srv
{
    class Program
    {
       static SocketFactoty factoty = new SocketFactoty();
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.Is64BitProcess);
            Rec();
            Sned();
            Console.Read();
            Console.WriteLine("Hello World!");
        }

        static void Sned()
        {
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                udt.NetCore.UDTSocket socket = factoty.Factory.Create();
                socket.Connect("192.168.0.158", 6666);
                Task.Run(() =>
                {
                    while (true)
                    {
                        byte[] buf = new byte[10];
                        int r = socket.Receive(buf, 10);
                        string str = Encoding.Default.GetString(buf);
                        Console.WriteLine(str);
                        if (socket.State == udt.NetCore.UdtStatus.CLOSED)
                        { break; }
                    }
                });
                while (true)
                {
                    socket.Send(Encoding.Default.GetBytes("ssss"),0,10);
                    Thread.Sleep(1000);
                }
               
            });
        }

        static void Rec()
        {
            Task.Run(() =>
            {
                udt.NetCore.UDTSocket socket = factoty.Factory.Create();
                socket.Bind("192.168.0.158", 6666);
                socket.Listen(100);
                while (true)
                {
                    System.Net.IPEndPoint remote;
                   var c= socket.Accept(out remote);
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            byte[] buf = new byte[10];
                            int r = c.Receive(buf, 10);
                            string str = Encoding.Default.GetString(buf);
                            Console.WriteLine(str);
                            if (c.State == udt.NetCore.UdtStatus.CLOSED)
                            { break; }
                            c.Send(Encoding.Default.GetBytes("收到"), 0, 4);
                        }

                    });
                  ;
                }

            });
        }
    }
}
