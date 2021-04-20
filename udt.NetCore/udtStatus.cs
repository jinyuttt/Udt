namespace udt.NetCore
{
    public enum  UdtStatus
    {
        INIT = 1,
        OPENED,
        LISTENING,
        CONNECTING,
        CONNECTED,
        BROKEN,
        CLOSING,
        CLOSED,
        NONEXIST
    }
}