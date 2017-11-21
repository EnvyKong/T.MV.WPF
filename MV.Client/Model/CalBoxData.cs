using System.Collections.Generic;

namespace MV.Client.Model
{
    public class CalBoxData
    {
        public List<PortData> APortDataList = new List<PortData>();
        public List<PortData> BPortDataList = new List<PortData>();

        public PortData this[Port port, int id]
        {
            get
            {
                switch (port)
                {
                    case Port.A:
                        return APortDataList[id - 1];
                    case Port.B:
                        return BPortDataList[id - 1];
                    default:
                        return null;
                }
            }
        }
    }

    public enum Port
    {
        A, B
    }
}
