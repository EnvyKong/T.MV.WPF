using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MV.Client.Model
{
    public class Scene
    {
        private List<Frame> _frameList;
        public ReadOnlyCollection<Frame> Frames
        {
            get { return _frameList.AsReadOnly(); }
        }

        public void LoadSceneData(string path)
        {
            _frameList = new List<Frame>();
            var allFrames = File.ReadAllLines(path);
            var frameLength = allFrames[0].Split(',').Length;

            var channelsToMatrix = new List<Channel>();
            for (int row = 1; row < allFrames.Length; row++)
            {
                var frame = new Frame(channelsToMatrix);
                for (int column = 1; column < frameLength; column++)
                {
                    var abPort = allFrames[0].Split(',')[column].Split('A', 'B');
                    var aID = abPort[1].ToInt32();
                    var bID = abPort[2].ToInt32();
                    channelsToMatrix.Add(new Channel(aID, bID)
                    {
                        Pha = allFrames[row].Split(',')[column].ToDouble()
                    });
                }
                _frameList.Add(frame);
            }
        }
    }
}
