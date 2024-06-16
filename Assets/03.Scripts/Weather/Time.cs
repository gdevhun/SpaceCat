using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NTPClient
{
    private const string NtpServer = "time.windows.com";
    private const int NtpPort = 123;
    private const int NtpDataLength = 48;
    private const byte NtpMode = 3;
    private const byte NtpVersion = 3;

    public DateTime GetNetworkTime()
    {
        byte[] ntpData = new byte[NtpDataLength];

        ntpData[0] = (byte)(NtpMode | (NtpVersion << 3));

        IPAddress[] addresses = Dns.GetHostEntry(NtpServer).AddressList;
        IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], NtpPort);

        using (UdpClient udpClient = new UdpClient())
        {
            udpClient.Connect(ipEndPoint);
            udpClient.Send(ntpData, ntpData.Length);
            ntpData = udpClient.Receive(ref ipEndPoint);
        }

        ulong intPart = BitConverter.ToUInt32(ntpData, 40);
        ulong fractPart = BitConverter.ToUInt32(ntpData, 44);

        intPart = SwapEndianness(intPart);
        fractPart = SwapEndianness(fractPart);

        ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

        return networkDateTime.ToLocalTime();
    }

    private static uint SwapEndianness(ulong x)
    {
        return (uint)(((x & 0x000000ff) << 24) +
                      ((x & 0x0000ff00) << 8) +
                      ((x & 0x00ff0000) >> 8) +
                      ((x & 0xff000000) >> 24));
    }
   
}
