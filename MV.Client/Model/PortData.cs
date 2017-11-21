namespace MV.Client.Model
{
    public class PortData
    {
        public double Attenuation { set; get; }

        private double _phase;
        public double Phase
        {
            get { return _phase; }
            set
            {
                var p = value;
                while (p < 0)
                {
                    p = 360 + p;
                }
                _phase = p % 360;
            }
        }
    }
}
