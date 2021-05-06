using System.Net;

namespace IUdtSocket
{
    public interface IUdtSocket
    {

        /// <summary>
        /// UDT发送缓冲区队列大小
        /// </summary>
        public int UDTSendBufferSize
        {
            get;set;
        }


        /// <summary>
        /// UDT接收缓冲区队列大小
        /// </summary>
        public int UDTReviceBufferSize
        {
            get;set;
        }


        /// <summary>
        /// UDP发送缓冲区队列大小
        /// 
        /// </summary>
        public int UDPSendBufferSize
        {
            get;set;
        }


        /// <summary>
        /// UDP接收缓冲区队列大小
        /// </summary>
        public int UDPReviceBufferSize
        {
            get;set;
        }

        /// <summary>
        /// 发送超时时间
        /// </summary>
        public int SendTimeOut
        {
            get;set;
        }

        /// <summary>
        /// 接收超时时间
        /// </summary>
        public int ReviceTimeOut
        {
            get;set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        public UdtStatus State { get; }


        /// <summary>
        /// 获取UDT本地监听地址
        /// </summary>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public IPEndPoint LocalEndPoint
        {
            get;
        }

        /// <summary>
        /// 获取UDT远程地址
        /// </summary>
        /// <exception cref="ObjectDisposedException">UDT被释放时引发</exception>
        public IPEndPoint RmoteEndPoint
        {
            get;
        }


        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="remoteIP"></param>
        /// <param name="remotePort"></param>
        void Connect(string remoteIP, int remotePort);

       /// <summary>
       /// 绑定地址
       /// </summary>
       /// <param name="localIP">本地IP</param>
       /// <param name="localPort">本地端口</param>
        void Bind(string localIP, int localPort);

        
        /// <summary>
        /// 监听连接
        /// </summary>
        /// <param name="blobacklog"></param>
        void Listen(int blobacklog);

        /// <summary>
        /// 接收连接
        /// </summary>
        /// <param name="remote">连接地址</param>
        /// <returns></returns>
       IUdtSocket Accept(out IPEndPoint remote);

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        int Receive(byte[] buf);


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        int Send(byte[] buffer);

        public int SendMessage(byte[] buffer);
        public int RecviceMessage(byte[] buffer);


        public void Close();
    }
}
