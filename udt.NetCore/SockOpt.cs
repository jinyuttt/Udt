namespace udt.NetCore
{
    public enum SOCKOPT
    {
        /// <summary>
        /// 最大传输单位
        /// </summary>
        UDT_MSS,
        /// <summary>
        /// 是否阻塞发送
        /// </summary>
        UDT_SNDSYN,
        /// <summary>
        /// 是否阻塞接收
        /// </summary>
        UDT_RCVSYN,
        /// <summary>
        /// 自定义拥塞控制算法
        /// </summary>
        UDT_CC,
        /// <summary>
        /// 窗口大小
        /// </summary>
        UDT_FC,
        /// <summary>
        /// 发送队列缓冲最大值
        /// </summary>
        UDT_SNDBUF,
        /// <summary>
        /// UDT接收缓冲大小
        /// </summary>
        UDT_RCVBUF,
        /// <summary>
        /// 关闭时等待数据发送完成
        /// </summary>
        UDT_LINGER,
        /// <summary>
        /// UDP发送缓冲大小
        /// </summary>
        UDP_SNDBUF,
        /// <summary>
        /// UDP接收缓冲大小
        /// </summary>
        UDP_RCVBUF,
        /// <summary>
        /// maximum datagram message size
        /// </summary>
        UDT_MAXMSG,
        /// <summary>
        /// time-to-live of a datagram message
        /// </summary>
        UDT_MSGTTL,
        /// <summary>
        /// 会合连接模式
        /// </summary>
        UDT_RENDEZVOUS,
        /// <summary>
        /// send()超时
        /// </summary>
        UDT_SNDTIMEO,
        /// <summary>
        /// recv()超时
        /// </summary>
        UDT_RCVTIMEO,
        /// <summary>
        /// 复用一个已存在的端口或者创建一个新的端口
        /// </summary>
        UDT_REUSEADDR,
        /// <summary>
        ///  当前连接可以使用的最大带宽(bytes per second)
        /// </summary>
        UDT_MAXBW,
        /// <summary>
        /// current socket state, see UDTSTATUS, read only
        /// </summary>
        UDT_STATE,
        /// <summary>
        /// current avalable events associated with the socket
        /// </summary>
        UDT_EVENT
    };
}