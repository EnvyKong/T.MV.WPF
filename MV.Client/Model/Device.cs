using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;
using TopYoung.MV.Core;
using System.Windows;

namespace MV.Client.Model
{
    public abstract class Device : IConnected
    {
        protected Socket Socket { get; set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public string Name { get => GetType().Name; }

        internal int Quantity { get => (int)AppConfigInfo.GetProperty($"{Name}Quantity"); }//(int)typeof(AppConfigInfo).GetProperty($"{Name}Quantity").GetValue(null, null); }

        internal int APortNum { get => (int)AppConfigInfo.GetProperty($"{Name}ANum"); }//(int)typeof(AppConfigInfo).GetProperty($"{Name}ANum").GetValue(null, null); }

        internal int BPortNum { get => (int)AppConfigInfo.GetProperty($"{Name}BNum"); }//(int)typeof(AppConfigInfo).GetProperty($"{Name}BNum").GetValue(null, null); }

        internal int APortConnectNum { get => (int)AppConfigInfo.GetProperty($"{Name}AConnectNum"); }//(int)typeof(AppConfigInfo).GetProperty($"{Name}AConnectNum").GetValue(null, null); }

        internal int BPortConnectNum { get => (int)AppConfigInfo.GetProperty($"{Name}BConnectNum"); }//(int)typeof(AppConfigInfo).GetProperty($"{Name}BConnectNum").GetValue(null, null); }

        internal int Frequency { get; set; }

        internal int this[int aPortID, int bPortID]
        {
            get
            {
                if (aPortID > APortNum | bPortID > BPortNum)
                {
                    throw new Exception($"{Name} Signal Path ID Error! ");
                }
                return BPortNum * (aPortID - 1) + bPortID;
            }
        }

        internal int SignalPathCount
        {
            get
            {
                return APortNum * BPortNum;
            }
        }

        protected virtual string Cmd { get; set; }

        protected Device(string ip, int portNum)
        {
            IP = ip;
            PortNum = portNum;
        }

        private string _ip;
        public string IP
        {
            get
            {
                return _ip;
            }
            private set
            {
                if (IPAddress.TryParse(value, out IPAddress address))
                {
                    IPAddress = address;
                    _ip = address.ToString();
                }
                else
                {
                    MessageBox.Show($"{Name} IP Formal Error!");
                    Log.log.ErrorFormat("{0} IP Formal Error!", Name);
                }
                Ping ping = new Ping();
                var pingReply = ping.Send(address);
                if (pingReply.Status != IPStatus.Success)
                {
                    MessageBox.Show($"{Name} IP Ping Error!");
                    Log.log.ErrorFormat("{0} IP Ping Error!", Name);
                }
            }
        }

        protected IPAddress IPAddress { get; private set; }

        protected int PortNum { get; private set; }

        public virtual void Connect()
        {
            Socket.Connect(IPAddress, PortNum);
        }

        public string Send(string cmd)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(cmd + "\r\n");
            Socket.Send(sendBuffer);
            var receiveBuffer = new byte[1024 * 1024];
            int num = 0;
            num = Socket.Receive(receiveBuffer);
            return Encoding.UTF8.GetString(receiveBuffer, 0, num);
        }

        public virtual bool Connected
        {
            get
            {
                return Socket.Connected;
            }
            set { }
        }

        public virtual void Close()
        {
            Socket.Close();
        }
    }
}