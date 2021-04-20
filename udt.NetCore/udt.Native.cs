using System;

using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Net.Sockets;

namespace udt.NetCore
{
    using static Globals;
    public sealed class UnsafeNativeMethods
    {
        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartUp();

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CleanUp();

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr NewSocket(UdtAddressFamily family, UdtSocketType socktype,int protocol);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindSocket(IntPtr udtSocket, IntPtr udpsockHandle);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int BindSocket(IntPtr udtSocket, Socket udpsockHandle);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Bind(IntPtr udtSocket, UdtAddressFamily family, UdtSocketType sockType, string ip, int port);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Listen(IntPtr udtSocket, int backlog);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
         public static extern IntPtr Accept(IntPtr udtSocket, byte[] ip, ref int iplen, ref int port);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
         public static extern int GetSockName(IntPtr usock, byte[] remoteip, ref int iplen, ref int port);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
         public static extern int GetSockPeerName(IntPtr usock, byte[] remoteip, ref int iplen, ref int port);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Connect(IntPtr udtSocket, UdtAddressFamily family, UdtSocketType sockType, string ip, int port);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Close(IntPtr udtSocket);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetSocketOption(IntPtr udtSocket, int level, SOCKOPT optname, IntPtr optval, ref int optlen);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSocketOption(IntPtr udtSocket, int level, Socket optname, IntPtr optval, int optlen);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSocketOption(IntPtr udtSocket, int level, SOCKOPT optname, ref int optval, int optlen);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetSocketOption(IntPtr udtSocket, int level, SOCKOPT optname, ref bool optval, int optlen);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Send(IntPtr udtSocket, byte[] buf, int len);


        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Receive(IntPtr udtSocket, byte[] buf, int len);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SendMsg(IntPtr udtSocket, byte[] buf, int len, int ttl, bool inorder);


        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RecviceMsg(IntPtr udtSocket, byte[] buf, int len);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 SendFile(IntPtr u, string filePath, bool isbinary, Int64 offset, Int64 size, int block);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 RecvFile(IntPtr u, string filePath,bool isbinary, Int64 offset, Int64 size, int block);
       

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Perfmon(IntPtr udtSocket, ref CPerfMon perf, bool clear);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern UdtStatus GetSocketState(IntPtr udtSocket);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetErrorCode();

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetErrorMessage(StringBuilder errorMsg, int len, ref int errorlen, ref  int errorCode);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true), SuppressUnmanagedCodeSecurity()]
        public static extern int Select([In]int nfds, [In, Out]IntPtr[] readfds, ref int readLen, [In, Out]IntPtr[] writefds, ref int writeLen, [In, Out]IntPtr[] exceptfds, ref int exceptLen, [In]IntPtr timeoutMicroseconds);

        [DllImport(UdtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true), SuppressUnmanagedCodeSecurity()]
        public static extern int Select([In]int nfds, [In, Out]IntPtr[] readfds, ref int readLen, [In, Out]IntPtr[] writefds, ref int writeLen, [In, Out]IntPtr[] exceptfds, ref int exceptLen, [In]ref TimeValue timeoutMicroseconds);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimeValue
    {
        public int Seconds;
        public int Microseconds;
    }
}