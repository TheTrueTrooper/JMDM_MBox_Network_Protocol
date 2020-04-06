using JMDM_Network_Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JMDM_Network_Protocol
{
    public class JMDM_MBOXUDP
    {
        IPEndPoint ReceiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
        IPEndPoint SendEndPoint;

        string SendIPAddress
        {
            get
            {
                return SendEndPoint.Address.ToString();
            }
        }
        int SendPort
        {
            get
            {
                return SendEndPoint.Port;
            }
        }

        string ReceiveIPAddress
        {
            get
            {
                return ReceiveEndPoint.Address.ToString();
            }
        }
        int ReceivePort
        {
            get
            {
                return ReceiveEndPoint.Port;
            }
        }

        string LocalIP
        {
            get
            {
                return Dns.GetHostAddresses(Environment.MachineName)[0].ToString();
            }
        }

        //1 VerificationCode is always 0x55, 0xAA >2bytes
        static readonly byte[] VerificationCode = new byte[2] { 0x55, 0xAA };

        //2 VerificationCode is defualt 0x00, 0x00 >2bytes
        readonly byte[] PassCode = new byte[2] { 0x00, 0x00 };

        //3 Fuc Codes >2bytes
        readonly byte[] AbsoluteTimeFuctionCode = new byte[2] { 0x13, 0x01 };
        readonly byte[] RelativeTimeFuctionCode = new byte[2] { 0x14, 0x01 };

        //4 Channel number (the number of pistons to use) >2bytes

        //5 ReciveFromIP & ReplyToIP >2 bytes each, 4 bytes in total.
        //box defualt IP 192.168.15.201 where ReplyTo is 192.168.ReplyToIP[0].ReplyToIP[1] and ReciveFrom is 192.168.ReciveFromIP[0].ReciveFromIP[1]
        //ReplyToIP 0000 = no replay and FFFF means all recive and reply 192.168.15.201 would be 0x0fc9 (0x0f = 15, 0xc9 = 201) 
        internal byte[] ReciveFromIP = new byte[2] { 0xFF, 0xFF };
        //5 ReciveFromIP & ReplyToIP cont >2 bytes each, 4 bytes in total.
        internal byte[] ReplyToIP = new byte[2] { 0xFF, 0xFF };

        //6 Serial of instruction. I guess this just climbs
        uint SerialNumberOfInstruction = 1;

        //7 Time Code time to exicute in MS 100 = 0x64 >4 bytes
        //in absolute mode this will be the diffenece in time from last instruction else it is just the time put in
        // if 0 is input than the assumed value is 100ms
        uint TimeingMS = 0;

        //8 LocationSet >4 bytes * number of axis
        // if 5 mm lead on one revolution has 10,000 pulses per revolution and can go 0-25mm
        // then 25/5 * 10000 = 50000 or 0x0000c350
        // Or reduction ratio is (turntable)1:60(motor) thgen
        //30/360 * (60 * 10000) = 50,000 for 30 degrees in the turn table

        //9 Special Effects
        // Each is a state, first 4 are reserved and rest are sp effects 0 = off 1 = on
        byte[] SpecicalEffects = new byte[2] { 0x00, 0x00 };

        //10 dual Anolog channels
        // There are 2 anolog channel that you can set. dont
        byte[] DualAnologChannels = new byte[4] { 0x00, 0x00 , 0x00, 0x00 };


        //other bytes
        byte[] WriteRegisters = new byte[2] { 0x12, 0x01};
        byte[] ReadRegisters = new byte[2] { 0x011, 0x01 };

        //write types
        byte[] SavelessWrite = new byte[2] { 0x00, 0x00 };
        byte[] ObjectOrCommandRegisters = new byte[2] { 0x00, 0x02 };

        byte[] CommandRegistersAddress = new byte[2] { 0x00, 0x00 };
        byte[] ParamRegistersAddress = new byte[2] { 0x00, 0x90 };

        UdpClient UDPReceivePort;
        UdpClient UDPSendPort;

        public JMDM_MBOXUDP(string TargetConnectionIP, int SendPort = /*8401*/ 7408, int ReceivePort = /*7408*/ 8401)
        {
            SendEndPoint = new IPEndPoint(IPAddress.Parse(TargetConnectionIP), SendPort);
            UDPSendPort = new UdpClient(SendPort);
            UDPReceivePort = new UdpClient(ReceivePort);
        }

        void SendData(byte[] Bytes)
        {
            UDPSendPort.Send(Bytes, Bytes.Count(), SendEndPoint);
        }

        void MakeWriteStart(byte[] DataFrame)
        {
            Array.Copy(VerificationCode, 0, DataFrame, 0, 2);
            Array.Copy(PassCode, 0, DataFrame, 2, 2);
            Array.Copy(WriteRegisters, 0, DataFrame, 4, 2);
            //skip one reg for write type to Ips
            Array.Copy(ReciveFromIP, 0, DataFrame, 8, 2);
            Array.Copy(ReplyToIP, 0, DataFrame, 10, 2);
        }

        void MakeCommandStart(byte[] DataFrame)
        {
            MakeWriteStart(DataFrame);
            //fill in write type
            Array.Copy(ObjectOrCommandRegisters, 0, DataFrame, 6, 2);
            Array.Copy(CommandRegistersAddress, 0, DataFrame, 12, 2);
        }

        public void MakeParamStart(byte[] DataFrame)
        {
            //fill in write type
            Array.Copy(SavelessWrite, 0, DataFrame, 6, 2);
            Array.Copy(ParamRegistersAddress, 0, DataFrame, 12, 2);
        }

        public void Reset()
        {
            byte[] DataFrame = new byte[18];
            MakeCommandStart(DataFrame);
            Array.Copy(CommandRegistersAddress, 0, DataFrame, 12, 2);
            //operate on only one register
            DataFrame[14] = 0x00;
            DataFrame[15] = 0x01;
            //reset command
            DataFrame[16] = 0x00;
            DataFrame[17] = 0x00;
            SendData(DataFrame);
        }

        public void EMStop()
        {
            byte[] DataFrame = new byte[18];
            MakeParamStart(DataFrame);
            
            //operate on only one register
            DataFrame[14] = 0x00;
            DataFrame[15] = 0x01;
            //EMStop command
            DataFrame[16] = 0x00;
            DataFrame[17] = 0x01;
            SendData(DataFrame);
        }

        public void EMStopRelease()
        {
            byte[] DataFrame = new byte[18];
            MakeParamStart(DataFrame);

            //operate on only one register
            DataFrame[14] = 0x00;
            DataFrame[15] = 0x01;
            //EMStop release command
            DataFrame[16] = 0x00;
            DataFrame[17] = 0x00;
            SendData(DataFrame);
        }

        public void CancelEmergancyStop()
        {
            byte[] DataFrame = new byte[18];
            MakeCommandStart(DataFrame);
            Array.Copy(CommandRegistersAddress, 0, DataFrame, 12, 2);
        }

        public void SetAxis(uint[] AxisSets, Channels Channels = Channels.Set3Axis, bool Absolute = false)
        {
            const string AxisError = "Number of axis doesn't match picked set";
            byte[] DataFrame = null;
            switch (Channels)
            {
                case Channels.Set3Axis:
                    if (AxisSets.Length != 3)
                        throw new Exception(AxisError);
                    DataFrame = new byte[26 + 12];

                    break;
                case Channels.Set6Axis:
                    if (AxisSets.Length != 6)
                        throw new Exception(AxisError);
                    DataFrame = new byte[26 + 24];
                    break;
                case Channels.Set10Axis:
                    if (AxisSets.Length != 10)
                        throw new Exception(AxisError);
                    DataFrame = new byte[26 + 40];
                    break;
            }
            Array.Copy(VerificationCode, 0, DataFrame, 0, 2);
            Array.Copy(PassCode, 0, DataFrame, 2, 2);
            if (Absolute)
                Array.Copy(AbsoluteTimeFuctionCode, 0, DataFrame, 4, 2);
            else
                Array.Copy(RelativeTimeFuctionCode, 0, DataFrame, 4, 2);

            DataFrame[7] = (byte)Channels;

            Array.Copy(ReciveFromIP, 0, DataFrame, 8, 2);
            Array.Copy(ReplyToIP, 0, DataFrame, 10, 2);

            byte[] TempArray = BitConverter.GetBytes(SerialNumberOfInstruction);
            if (BitConverter.IsLittleEndian)
                TempArray = TempArray.Reverse().ToArray();
            Array.Copy(TempArray, 0, DataFrame, 12, 4);

            TempArray = BitConverter.GetBytes(TimeingMS);
            if (BitConverter.IsLittleEndian)
                TempArray = TempArray.Reverse().ToArray();
            Array.Copy(TempArray, 0, DataFrame, 16, 4);
            //20 is where motion starts and static breaks
            int CopyTo = 20;
            foreach (int Axis in AxisSets)
            {
                byte[] Number = BitConverter.GetBytes(Axis);
                if (BitConverter.IsLittleEndian)
                    Number = Number.Reverse().ToArray();
                Array.Copy(Number, 0, DataFrame, CopyTo, 4);
                CopyTo += 4;
            }
            Array.Copy(SpecicalEffects, 0, DataFrame, DataFrame.Length - 6, 2);
            Array.Copy(DualAnologChannels, 0, DataFrame, DataFrame.Length - 4, 4);

            SerialNumberOfInstruction += 1;
            SendData(DataFrame);

            //{
            //    VerificationCode[0], VerificationCode[1],
            //    PassCode[0], PassCode[1],
            //    RelativeTimeFuctionCode[0], RelativeTimeFuctionCode[1],
            //    ChannelNumber3Axis[0], ChannelNumber3Axis[1],
            //    ReciveFromIP[0], ReciveFromIP[1],
            //    ReplyToIP[0], ReplyToIP[1],
            //    SerialNumberOfInstruction[0], SerialNumberOfInstruction[1], SerialNumberOfInstruction[2], SerialNumberOfInstruction[3],
            //    TimeCode[0], TimeCode[1], TimeCode[2], TimeCode[3],
            //    LocationSet3Axis[0,0], LocationSet3Axis[0,1], LocationSet3Axis[0,2], LocationSet3Axis[0,3],
            //    LocationSet3Axis[1,0], LocationSet3Axis[1,1], LocationSet3Axis[1,2], LocationSet3Axis[1,3],
            //    LocationSet3Axis[2,0], LocationSet3Axis[2,1], LocationSet3Axis[2,2], LocationSet3Axis[2,3],
            //    SpecicalEffects[0], SpecicalEffects[1],
            //    DualAnologChannels[0,0], DualAnologChannels[0,1], DualAnologChannels[1,0], DualAnologChannels[1,1]
            //}
        }

        public byte[] Receive()
        {
            return UDPReceivePort.Receive(ref ReceiveEndPoint);
        }

    }
}
