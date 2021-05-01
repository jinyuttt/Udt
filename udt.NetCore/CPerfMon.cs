namespace udtnative
{
    public struct CPerfMon
    {
        // global measurements
        public long msTimeStamp;                 // time since the UDT entity is started, in milliseconds

        public long pktSentTotal;                // total number of sent data packets, including retransmissions
        public long pktRecvTotal;                // total number of received packets
        public int pktSndLossTotal;                 // total number of lost packets (sender side)
        public int pktRcvLossTotal;                 // total number of lost packets (receiver side)
        public int pktRetransTotal;                 // total number of retransmitted packets
        public int pktSentACKTotal;                 // total number of sent ACK packets
        public int pktRecvACKTotal;                 // total number of received ACK packets
        public int pktSentNAKTotal;                 // total number of sent NAK packets
        public int pktRecvNAKTotal;                 // total number of received NAK packets
        public long usSndDurationTotal;		// total time duration when UDT is sending data (idle time exclusive)

        // local measurements
        public long pktSent;                     // number of sent data packets, including retransmissions

        public long pktRecv;                     // number of received packets
        public int pktSndLoss;                      // number of lost packets (sender side)
        public int pktRcvLoss;                      // number of lost packets (receiver side)
        public int pktRetrans;                      // number of retransmitted packets
        public int pktSentACK;                      // number of sent ACK packets
        public int pktRecvACK;                      // number of received ACK packets
        public int pktSentNAK;                      // number of sent NAK packets
        public int pktRecvNAK;                      // number of received NAK packets
        public double mbpsSendRate;                 // sending rate in Mb/s
        public double mbpsRecvRate;                 // receiving rate in Mb/s
        public long usSndDuration;		// busy sending time (i.e., idle time exclusive)

        // instant measurements
        public double usPktSndPeriod;               // packet sending period, in microseconds

        public int pktFlowWindow;                   // flow window size, in number of packets
        public int pktCongestionWindow;             // congestion window size, in number of packets
        public int pktFlightSize;                   // number of packets on flight
        public double msRTT;                        // RTT, in milliseconds
        public double mbpsBandwidth;                // estimated bandwidth, in Mb/s
        public int byteAvailSndBuf;                 // available UDT sender buffer size
        public int byteAvailRcvBuf;                 // available UDT receiver buffer size
    }
}