using System;
using System.Windows;

namespace MV.Client.Model
{
    public class Vertex : Device
    {
        internal void OpenChannel(int inPortID, int outPortID, UpDown linkUpDown)
        {
            try
            {
                switch (linkUpDown)
                {
                    case UpDown.UP:
                        Cmd = $"SYST:RLINK:BA{inPortID}{outPortID}:STATe ON";
                        break;
                    case UpDown.DOWN:
                        Cmd = $"SYST:RLINK:AB{inPortID}{outPortID}:STATe ON";
                        break;
                    default:
                        Cmd = "";
                        break;
                }
                Send(Cmd).WaitCompleted(Send, Cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Vertex(string ip, int portNum) : base(ip, portNum)
        {

        }

        internal void CloseChannel(int inPortID, int outPortID, UpDown linkUpDown)
        {
            try
            {
                switch (linkUpDown)
                {
                    case UpDown.UP:
                        Cmd = $"SYST:RLINK:BA{inPortID}{outPortID}:STATe OFF";
                        break;
                    case UpDown.DOWN:
                        Cmd = $"SYST:RLINK:AB{inPortID}{outPortID}:STATe OFF";
                        break;
                    default:
                        Cmd = "";
                        break;
                }
                Send(Cmd).WaitCompleted(Send, Cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal void CloseAllChannel(int aPortNum, int bPortNum)
        {
            try
            {
                for (int a = 1; a <= aPortNum; a++)
                {
                    for (int b = 1; b <= bPortNum; b++)
                    {
                        Cmd = $"SYST:RLINK:BA{b}{a}:STATe OFF";
                        Send(Cmd).WaitCompleted(Send, Cmd);
                        Cmd = $"SYST:RLINK:AB{a}{b}:STATe OFF";
                        Send(Cmd).WaitCompleted(Send, Cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void ReSet()
        {
            try
            {
                Cmd = "*RST";
                Send(Cmd).WaitCompleted(Send, Cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void SetAtt()
        {
            throw new NotImplementedException();
        }

        internal void SetPha(String index, double upLinkValue, double downLinkValue)
        {
            try
            {
                var port = index.Split(':');
                Cmd = $"RLINK:BA{port[1]}{port[0]}:PHAse {upLinkValue}";
                Send(Cmd).WaitCompleted(Send, Cmd);
                Cmd = $"RLINK:AB{port[0]}{port[1]}:PHAse {downLinkValue}";
                Send(Cmd).WaitCompleted(Send, Cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal void SelfCalibrate()
        {
            Cmd = "CONnection:IPCAL:BEGin";
            Send(Cmd);
        }

        internal bool IsSelfCalibrateComplete()
        {
            Cmd = "CONnection:IPCAL:STATus?";
            return Send(Cmd).Contains("Completed");
        }
    }

    public enum UpDown
    {
        UP,
        DOWN
    }
}
