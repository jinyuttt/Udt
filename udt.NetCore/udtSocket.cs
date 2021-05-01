

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace udt.NetCore
{
  
    public sealed class UDTSocket: IUdtSocket.IUdtSocket
    {
        private Socket bindSocket = null; 

        /// <summary>
        /// 获取UDT本地监听地址
        /// </summary>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public IPEndPoint LocalEndPoint
        {
            get
            {
               
                int port = 0;
                byte[] ip = new byte[15];
                 int iplen = 0;
               
                 if (UnsafeNativeMethods.GetSockName(this.Handle, ip, ref iplen, ref port) != 0)
                 {
                     return null;
                 }
                 else
                 {
                     return new IPEndPoint(IPAddress.Parse(Encoding.Default.GetString(ip, 0, iplen)), port);
                 }
              
               
            }
        }

        /// <summary>
        /// 获取UDT远程地址
        /// </summary>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
         public IPEndPoint RmoteEndPoint
        {
            get
            {
               
                int port = 0;
                byte[] ip = new byte[15];

                int iplen = 0;
                if (UnsafeNativeMethods.GetSockPeerName(this.Handle, ip, ref iplen, ref port) != 0)
                {
                    return null;
                }
                else
                {
                    return new IPEndPoint(IPAddress.Parse(Encoding.Default.GetString(ip, 0, iplen)), port);
                }
             
                
            }
        }

        /// <summary>
        /// 当前UDT句柄
        /// </summary>
        public IntPtr Handle
        {
            get;
            private set;
        }

        static UDTSocket()
        {
           
            UnsafeNativeMethods.StartUp();

        }

      

        public UdtAddressFamily Family { get; private set; }

        public UdtSocketType SocketType { get; private set; }

        public UDTSocket()
            : this(UdtAddressFamily.InterNetwork, UdtSocketType.SOCK_STREAM)
        {
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="sockType"></param>
        public UDTSocket(UdtAddressFamily family, UdtSocketType sockType)
        {
            // SOCK_STREAM： 提供面向连接的稳定数据传输，即TCP协议。
            // SOCK_DGRAM： 使用不连续不可靠的数据包连接。
            // SOCK_PACKET： 与网络驱动程序直接通信。
            // 注：此处的SOCK_STREAM并不是表示UDT将会使用TCP类型的Socket，在底层将会转化为UDT_STREAM
            // 并且在UDT中仅支持SOCK_STREAM和SOCK_DGRAM，分别对应UDT_STREAM和UDT_DGRAM
            // 此处实际上最终调用了CUDTUnited的newSocket，第一个参数会被直接设置到CUDT的m_iIPversion，第二个参数会被映射为UDT的连接类型，第三个参数被忽略，没有实际意义
            this.Handle = UnsafeNativeMethods.NewSocket(family, sockType,0);
            if (this.Handle == IntPtr.Zero)
            {
                throw GetUDTException();
            }
            this.Family = family;
            this.SocketType = sockType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="family">1：InterNetwork 2：InterNetworkV6</param>
        /// <param name="sockType">1：SOCK_STREAM 2：SOCK_DGRAM</param>
        public UDTSocket(int family, int sockType)
        {
            UdtAddressFamily vf = UdtAddressFamily.InterNetwork;

            UdtSocketType vs = UdtSocketType.SOCK_STREAM;

            if (family == 2)
            {
                vf = UdtAddressFamily.InterNetworkV6;
            }
            if (sockType == 2)
            {
                vs = UdtSocketType.SOCK_DGRAM;
            }
            this.Handle = UnsafeNativeMethods.NewSocket(vf, vs, 0);
            if (this.Handle == IntPtr.Zero)
            {
                throw GetUDTException();
            }
            this.Family = vf;
            this.SocketType = vs;
        }

        
        private UDTSocket(IntPtr handle)
        {
            this.Handle = handle;
        }

        /// <summary>
        /// udt状态
        /// </summary>
        public IUdtSocket.UdtStatus State
        {
            get
            {
               
                return UnsafeNativeMethods.GetSocketState(this.Handle);
            }
        }


        /// <summary>
        /// 获取UDT性能相关数据
        /// </summary>
        public CPerfMon Perfmon
        {
            get
            {
               
                CPerfMon perfmon = new CPerfMon();
                UnsafeNativeMethods.Perfmon(this.Handle, ref perfmon, false);
                return perfmon;
            }
        }


        /// <summary>
        /// 最大传输带宽
        /// </summary>
        public int MaxBandWith
        {
            set
            {
               
                this.SetSocketOption(SOCKOPT.UDT_MAXBW, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_MAXBW, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// UDT发送缓冲区队列大小
        /// </summary>
        public int UDTSendBufferSize
        {
            set
            {
             
                if (value < 1) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDT_SNDBUF, value);
            }
            get
            {
              
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_SNDBUF, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }


        /// <summary>
        /// UDT接收缓冲区队列大小
        /// </summary>
        public int UDTReviceBufferSize
        {
            set
            {
              
                if (value < 0) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDT_RCVBUF, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_RCVBUF, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }


        /// <summary>
        /// UDP发送缓冲区队列大小
        /// 
        /// </summary>
        public int UDPSendBufferSize
        {
            set
            {
              
                if (value < 0) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDP_SNDBUF, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDP_SNDBUF, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }


        /// <summary>
        /// UDP接收缓冲区队列大小
        /// </summary>
        public int UDPReviceBufferSize
        {
            set
            {
              
                if (value < 0) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDP_RCVBUF, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDP_RCVBUF, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 窗口大小
        /// </summary>
        public int WindowSize
        {
            set
            {
               
                if (value < 0) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDT_FC, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_FC, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        ///  最大传输单元
        /// </summary>
        public int MSS
        {
            set
            {
                
                if (value < 0) throw new ArgumentException();
                this.SetSocketOption(SOCKOPT.UDT_MSS, value);
            }
            get
            {
                
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_MSS, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 发送超时时间
        /// </summary>
        public int SendTimeOut
        {
            set
            {
               
                this.SetSocketOption(SOCKOPT.UDT_SNDTIMEO, value);
            }
            get
            {
              
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_SNDTIMEO, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 接收超时时间
        /// </summary>
        public int ReviceTimeOut
        {
            set
            {
                
                this.SetSocketOption(SOCKOPT.UDT_RCVTIMEO, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(4);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_RCVTIMEO, ptr, 4);
                    return Marshal.ReadInt32(ptr);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 是否汇合连接模式
        /// </summary>
        public bool IsRendezvous
        {
            set
            {
                
                this.SetSocketOption(SOCKOPT.UDT_RENDEZVOUS, value);
            }
            get
            {
                
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_RENDEZVOUS, ptr, 1);
                    return Convert.ToBoolean(Marshal.ReadByte(ptr));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 复用一个已存在的端口或者创建一个新的端口
        /// </summary>
        public bool IsReuseAddress
        {
            set
            {
               
                this.SetSocketOption(SOCKOPT.UDT_REUSEADDR, value);
            }
            get
            {
                
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_REUSEADDR, ptr, 1);
                    return Convert.ToBoolean(Marshal.ReadByte(ptr));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 设置关闭时是否等待数据发送完成
        /// </summary>
        public bool IsClosedLinger
        {
            set
            {
              
                this.SetSocketOption(SOCKOPT.UDT_LINGER, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_REUSEADDR, ptr, 1);
                    return Convert.ToBoolean(Marshal.ReadByte(ptr));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 是否同步接收
        /// </summary>
        public bool IsReviceSyn
        {
            set
            {
              
                this.SetSocketOption(SOCKOPT.UDT_RCVSYN, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_RCVSYN, ptr, 1);
                    return Convert.ToBoolean(Marshal.ReadByte(ptr));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        /// <summary>
        /// 是否同步发送
        /// </summary>
        public bool IsSendSyn
        {
            set
            {
                
                this.SetSocketOption(SOCKOPT.UDT_RCVSYN, value);
            }
            get
            {
               
                IntPtr ptr = Marshal.AllocHGlobal(1);
                try
                {
                    this.GetSocketOption(SOCKOPT.UDT_SNDSYN, ptr, 1);
                    return Convert.ToBoolean(Marshal.ReadByte(ptr));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

     

        /// <summary>
        /// 绑定本机地址
        /// </summary>
        /// <param name="ip">本机IP</param>
        /// <param name="port">监听端口</param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void Bind(string ip, int port)
        {
          
            if (UnsafeNativeMethods.Bind(this.Handle, this.Family, this.SocketType, ip, port) != 0)
            {
                throw GetUDTException();
            }
        }

        

        /// <summary>
        /// 绑定现有Socket
        /// </summary>
        /// <param name="socket">现有Socket</param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void Bind(Socket socket)
        {
           
            this.Bind(socket.Handle);
            bindSocket = socket;
        }

        /// <summary>
        /// 绑定现有Socket
        /// </summary>
        /// <param name="socket">现有Socket句柄</param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void Bind(IntPtr udpHandle)
        {
            
            if (udpHandle == IntPtr.Zero)
            {
                throw new ArgumentException("udpHandle");
            }
            if (UnsafeNativeMethods.BindSocket(this.Handle, udpHandle) != 0)
            {
                throw GetUDTException();
            }
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <param name="backlog">挂起连接队列的最大长度</param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void Listen(int backlog)
        {
            
            if (UnsafeNativeMethods.Listen(this.Handle, backlog) != 0)
            {
                throw GetUDTException();
            }
        }

        /// <summary>
        /// 建立与远程主机的连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port)
        {
          
            if (UnsafeNativeMethods.Connect(this.Handle, this.Family, this.SocketType, ip, port) != 0)
            {
                throw GetUDTException();
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public int StartUp()
        {

           return UnsafeNativeMethods.StartUp();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public int CleanUp()
        {
            return UnsafeNativeMethods.CleanUp();
        }

     

        /// <summary>
        /// 为新建连接创建新的UDTSocket
        /// </summary>
        /// <param name="remote">请求连接的远程地址</param>
        /// <returns>连接的客户端</returns>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
         public UDTSocket AcceptSocket(out IPEndPoint remote)
        {
           
            int port = 0;
            byte[] ip = new byte[15];
          
                int iplen = 0;
                IntPtr acceptHandle = UnsafeNativeMethods.Accept(this.Handle, ip, ref iplen, ref port);
              
                if (acceptHandle == IntPtr.Zero)
                {
                    throw GetUDTException();
                }
                else
                {
                    remote = new IPEndPoint(IPAddress.Parse(Encoding.Default.GetString(ip, 0, iplen)), port);
                    UDTSocket sock = new UDTSocket(acceptHandle);
                    sock.Family = this.Family;
                    sock.SocketType = this.SocketType;
                    return sock;
                }
           
        }


        /// <summary>
        /// 为新建连接创建新的UDTSocket
        /// </summary>
        /// <param name="remote">请求连接的远程地址</param>
        /// <returns>连接的客户端</returns>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public IUdtSocket.IUdtSocket Accept(out IPEndPoint remote)
        {

            int port = 0;
            byte[] ip = new byte[15];

            int iplen = 0;
            IntPtr acceptHandle = UnsafeNativeMethods.Accept(this.Handle, ip, ref iplen, ref port);

            if (acceptHandle == IntPtr.Zero)
            {
                throw GetUDTException();
            }
            else
            {
                remote = new IPEndPoint(IPAddress.Parse(Encoding.Default.GetString(ip, 0, iplen)), port);
                UDTSocket sock = new UDTSocket(acceptHandle);
                sock.Family = this.Family;
                sock.SocketType = this.SocketType;
                return sock;
            }

        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            UnsafeNativeMethods.Close(this.Handle);
            if (this.bindSocket != null)
            {
                this.bindSocket.Close();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer">发送缓冲区</param>
        /// <param name="offset">发送起始索引</param>
        /// <param name="len">需要发送的数据长度</param>
        /// <returns>发送的数据大小</returns>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        /// <remarks>数据长度0不代表发送失败，只说明发送的数据量为0</remarks>


        public int Send(byte[] buffer)
        {
            int sendLen = UnsafeNativeMethods.Send(this.Handle, buffer, buffer.Length);

            if (sendLen < 0) throw GetUDTException();
            return sendLen;

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
         public int SendMessage(byte[] buffer)
         {

             int sendLen = UnsafeNativeMethods.SendMsg(this.Handle, buffer, buffer.Length,-1,false);
           
             if (sendLen < 0) throw GetUDTException();
             return sendLen;
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int RecviceMessage(byte[] buffer)
        {

            int slen;
            slen = UnsafeNativeMethods.ReceiveMsg(this.Handle, buffer, buffer.Length);
            return slen;
        }


        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="buffer">接收缓冲区</param>
        /// <param name="offset">接收缓冲区起始索引</param>
        /// <param name="len">最大接收长度</param>
        /// <returns>接收的数据大小</returns>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        /// <remarks>接收数据长度0不代表客户端关闭，只说明接收的数据量为0</remarks>
        public int Receive(byte[] buffer)
        {
            int reviceLen = UnsafeNativeMethods.Receive(this.Handle, buffer, buffer.Length);

            if (reviceLen < 0) throw GetUDTException();
            return reviceLen;
        }

    
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isBinary"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public long SendFile(string file, bool isBinary, long offset, long len, int block)
        {
          
            long sendLen = UnsafeNativeMethods.SendFile(this.Handle, file, isBinary, offset, len, block);

            if (sendLen < 0) throw GetUDTException();
            return sendLen;

        }

        /// <summary>
        /// 接收文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isBinary"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public long RecviceFile(string file, bool isBinary, long offset, long len, int block)
        {

            long sendLen = UnsafeNativeMethods.RecvFile(this.Handle, file,isBinary, offset, len, block);

            if (sendLen < 0) throw GetUDTException();
            return sendLen;
        }
       
      
        /// <summary>
        /// 设置UDT相关属性
        /// </summary>
        /// <param name="optname"></param>
        /// <param name="value"></param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void SetSocketOption(SOCKOPT optname, int value)
        {
          
            int result = UnsafeNativeMethods.SetSocketOption(this.Handle, 0, optname, ref value, sizeof(int));
            if (result != 0)
            {
                throw GetUDTException();
            }
        }

        /// <summary>
        /// 设置UDT相关属性
        /// </summary>
        /// <param name="optname"></param>
        /// <param name="value"></param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void SetSocketOption(SOCKOPT optname, bool value)
        {
          
            int result = UnsafeNativeMethods.SetSocketOption(this.Handle, 0, optname, ref value, sizeof(bool));
            if (result != 0)
            {
                throw GetUDTException();
            }
        }

        /// <summary>
        /// 获取UDT相关属性
        /// </summary>
        /// <param name="optname"></param>
        /// <param name="ptr"></param>
        /// <param name="len"></param>
        /// <exception cref="UDTException"/>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public void GetSocketOption(SOCKOPT optname, IntPtr ptr, int len)
        {
          
            int result = UnsafeNativeMethods.GetSocketOption(this.Handle, 0, optname, ptr, ref len);
            if (result != 0)
            {
                throw GetUDTException();
            }
        }

        private static UdtException GetUDTException()
        {
            int len = 2048;
            StringBuilder sb = new StringBuilder(len);
            int errorCode = 0;
            int errorLen = 0;
            UnsafeNativeMethods.GetErrorMessage(sb, len, ref errorLen, ref errorCode);
            return new UdtException(sb.ToString().Substring(0, errorLen), errorCode);
        }

        private static void MicrosecondsToTimeValue(long microSeconds, ref TimeValue socketTime)
        {
            socketTime.Seconds = (int)(microSeconds / 0xf4240L);
            socketTime.Microseconds = (int)(microSeconds % 0xf4240L);
        }

        public static int Select(IList<UDTSocket> checkRead, IList<UDTSocket> checkWrite, IList<UDTSocket> checkError, int microSeconds)
        {
            if ((((checkRead == null) || (checkRead.Count == 0)) && ((checkWrite == null) || (checkWrite.Count == 0))) && ((checkError == null) || (checkError.Count == 0)))
            {
                throw new ArgumentNullException("checkRead or checkWrite or checkError must be one is not null.");
            }
            IntPtr[] readfds = SocketListToFileDescriptorSet(checkRead);
            IntPtr[] writefds = SocketListToFileDescriptorSet(checkWrite);
            IntPtr[] exceptfds = SocketListToFileDescriptorSet(checkError);
            int readlen = readfds == null ? 0 : readfds.Length;
            int writelen = writefds == null ? 0 : writefds.Length;
            int exceptlen = exceptfds == null ? 0 : exceptfds.Length;
            int result = 0;
            if (microSeconds != -1)
            {
                TimeValue socketTime = new TimeValue();
                MicrosecondsToTimeValue((long)((ulong)microSeconds), ref socketTime);
                result = UnsafeNativeMethods.Select(0, readfds, ref readlen, writefds, ref writelen, exceptfds, ref exceptlen, ref socketTime);
            }
            else
            {
                result = UnsafeNativeMethods.Select(0, readfds, ref readlen, writefds, ref writelen, exceptfds, ref exceptlen, IntPtr.Zero);
            }

            SelectFileDescriptor(checkRead, readfds, readlen);
            SelectFileDescriptor(checkWrite, writefds, writelen);
            SelectFileDescriptor(checkError, exceptfds, exceptlen);
            return result;
        }

        private static void SelectFileDescriptor(IList<UDTSocket> socketList, IntPtr[] fileDescriptorSet, int len)
        {
            if ((socketList != null) && (socketList.Count != 0))
            {
                if (len == 0)
                {
                    socketList.Clear();
                }
                else
                {
                    lock (socketList)
                    {
                        for (int i = 0; i < socketList.Count; i++)
                        {
                            UDTSocket socket = socketList[i];
                            int num2 = 0;
                            while (num2 < len)
                            {
                                if (fileDescriptorSet[num2] == socket.Handle)
                                {
                                    break;
                                }
                                num2++;
                            }
                            if (num2 == ((int)fileDescriptorSet[0]))
                            {
                                socketList.RemoveAt(i--);
                            }
                        }
                    }
                }
            }
        }

        private static IntPtr[] SocketListToFileDescriptorSet(IList<UDTSocket> socketList)
        {
            if ((socketList == null) || (socketList.Count == 0))
            {
                return null;
            }
            IntPtr[] ptrArray = new IntPtr[socketList.Count];
            for (int i = 0; i < socketList.Count; i++)
            {
                ptrArray[i] = socketList[i].Handle;
            }
            return ptrArray;
        }
         

       
    }
}