namespace MV.Client.Model
{
    public class SignalPath
    {
        public SignalPath(Matrix matrix, CalBoxData calBoxData)
        {
            _calBoxData = calBoxData;
            _matrix = matrix;
        }

        private CalBoxData _calBoxData;
        private Matrix _matrix;

        public static bool HasAttStandardValue { get; private set; }
        public static bool HasPhaStandardValue { get; private set; }

        private static double _attStandard;
        public static double AttStandard
        {
            get { return _attStandard; }
            set
            {
                HasAttStandardValue = true;
                _attStandard = value;
            }
        }

        private static double _phaStandard;
        public static double PhaStandard
        {
            get { return _phaStandard; }
            set
            {
                HasPhaStandardValue = true;
                _phaStandard = value;
            }
        }

        public int ID { get => -1;/*_aPortID + (_bPortID - 1) * 64 + (_cPortID - 1) * 1024;*/  }

        public string Index { get => $"{APortID}:{BPortID}:{CPortID}"; }

        public string SignalPathIDIndex { get => $"A{APortID}B{BPortID}C{BPortID}D{CPortID}"; }

        public int APortID { get; set; }

        public int BPortID { get; set; }

        public int CPortID { get; set; }

        private double _attenuation;
        public double Attenuation
        {
            get { return _attenuation; }
            set
            {
                //去除校准盒子数据
                //_attenuation = value;
                _attenuation = value - _calBoxData[Port.A, APortID].Attenuation - _calBoxData[Port.B, BPortID].Attenuation;
            }
        }

        private double _phase;
        public double Phase
        {
            get { return _phase; }
            set
            {
                //去除校准盒子数据
                //var p = value; //- calBoxData[Port.A, _aPortID].Phase - calBoxData[Port.B, _aPortID].Phase;
                var p = value - _calBoxData[Port.A, APortID].Phase - _calBoxData[Port.B, BPortID].Phase;
                while (p < 0)
                {
                    p = 360 + p;
                }
                _phase = p % 360;
            }
        }

        public Channel ChannelToMatrix
        {
            get
            {
                if (_matrix.PhaseStepShiftDirection == 1)
                {
                    return new Channel(APortID, BPortID) { AttOffset = Attenuation - AttStandard, PhaOffset = Phase - PhaStandard };
                }
                else
                {
                    return new Channel(APortID, BPortID) { AttOffset = Attenuation - AttStandard, PhaOffset = PhaStandard - Phase };
                }
            }
        }
    }
}
