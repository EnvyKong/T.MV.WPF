using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MV.Client.Model
{
    public class Frame
    {
        public Frame(List<Channel> channelToMatrixList)
        {
            _channelsToMatrix = channelToMatrixList;
        }

        private List<Channel> _channelsToMatrix;
        public ReadOnlyCollection<Channel> ChannelToMatrixCollection
        {
            get { return _channelsToMatrix.AsReadOnly(); }
        }
    }
}
