namespace MV.Client.Model
{
    public class Channel
    {
        public Channel(int aPortNum, int bPortNum)
        {
            Index = $"{aPortNum}:{bPortNum}";
            APortID = aPortNum;
            BPortID = bPortNum;
        }

        public string Index { get; }
        public int APortID { get; }
        public int BPortID { get; }

        private double _att;
        public double Att
        {
            get { return _att; }
            set
            {
                _att = value;
                HasAttValue = true;
            }
        }

        private double _pha;
        public double Pha
        {
            get { return _pha; }
            set
            {
                var p = value;
                while (p < 0)
                {
                    p = 360 + p;
                }
                _pha = p % 360;
                HasPhaValue = true;
            }
        }

        public bool HasAttValue { get; private set; }
        public bool HasPhaValue { get; private set; }

        private double _attOffset;
        public double AttOffset
        {
            get { return _attOffset; }
            set
            {
                _attOffset = value;
                HasAttOffsetValue = true;
            }
        }

        private double _phaOffset;
        public double PhaOffset
        {
            get { return _phaOffset; }
            set
            {
                var p = value;
                while (p < 0)
                {
                    p = 360 + p;
                }
                _phaOffset = p % 360;
                HasPhaOffsetValue = true;
            }
        }

        public bool HasAttOffsetValue { get; private set; }
        public bool HasPhaOffsetValue { get; private set; }
    }
}
