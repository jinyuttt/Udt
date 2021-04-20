using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace udt.NetCore
{
    public class UdtException : Exception
    {
        public int ErrorCode { get; private set; }

        public UdtException(string message, int errorCode)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public UdtException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return string.Format("ErrorCode={0},Exception:{0}", base.ToString());
        }
    }
}