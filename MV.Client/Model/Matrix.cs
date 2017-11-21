using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MV.Client.Model
{
    public class Matrix : Device
    {
        public Matrix(string ip, int portNum) : base(ip, portNum)
        {

        }

        public double AttenuationStep { get; set; }
        public double PhaseStep { get; set; }
        public int PhaseStepShiftDirection { get; set; }

        private string SetAttCmd(int id, int value)
        {
            Cmd = $"ATT:SHIFt:{id},{value}";
            return Send(Cmd);
        }

        private string SetPhaCmd(int id, int value)
        {
            Cmd = $"PHASe:SHIFt:{id},{value}";
            return Send(Cmd);
        }

        private string SetPhaAndAttCmd(int id, int pha, int att)
        {
            Cmd = $"SETM:{id},{pha},{att}";
            return Send(Cmd);
        }

        private string ReadIDNCmd()
        {
            Cmd = "*IDN?";
            return Send(Cmd);
        }

        internal int CurrentPha(int id)
        {
            Cmd = $"READM:{id}";
            return Send(Cmd).Replace("\r\n", "").Split(',')[1].ToInt32();
        }

        internal int CurrentAtt(int id)
        {
            Cmd = $"READM:{id}";
            return Send(Cmd).Replace("\r\n", "").Split(',')[2].ToInt32();
        }

        internal string ReadIDN()
        {
            return ReadIDNCmd();
        }

        internal void LoadOffsets(string filePath, out List<Channel> channels)
        {
            try
            {
                channels = new List<Channel>();

                if (filePath.ToLower().EndsWith(".txt"))
                {
                    var rows = File.ReadAllLines(filePath);
                    for (int r = 0; r < rows.Length; r++)
                    {
                        var columns = rows[r].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int c = 0; c < columns.Length; c++)
                        {
                            channels.Add(new Channel(r + 1, c + 1) { PhaOffset = columns[c].ToDouble() });
                        }
                    }
                }
                else if (filePath.ToLower().EndsWith(".csv"))
                {
                    var rows = File.ReadAllLines(filePath);
                    for (int r = 0; r < rows.Length; r++)
                    {
                        var columns = rows[r].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int c = 0; c < columns.Length; c++)
                        {
                            channels.Add(new Channel(r + 1, c + 1) { PhaOffset = columns[c].ToDouble() });
                        }
                    }
                }
                else
                {
                    throw new Exception("文件格式错误！");
                }
            }
            catch (Exception ex)
            {
                channels = null;
                throw ex;
            }
        }

        internal void SetPhaAndAtt(int id, int pha, int att)
        {
            SetPhaAndAttCmd(id, pha, att);
        }

        internal void SetPhaOffsets()
        {
            throw new NotImplementedException();
        }

        internal void SetPha(List<Channel> Channels)
        {
            try
            {
                foreach (var channel in Channels)
                {
                    var currentPha = CurrentPha(this[channel.APortID, channel.BPortID]);
                    var offset = (int)Math.Round(channel.PhaOffset / PhaseStep);
                    var x = SetPhaCmd(this[channel.APortID, channel.BPortID], (currentPha + offset) % (int)(360 / PhaseStep));
                    if ((!x.Contains("OK")))
                    {
                        Log.log.ErrorFormat("Signal Path ID : A{0}B{1} Set Value Error!", channel.APortID, channel.BPortID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void ResetAttAndPha()
        {
            try
            {
                for (int b = 1; b <= BPortConnectNum; b++)
                {
                    for (int a = 1; a <= APortNum; a++)
                    {
                        var x = SetPhaAndAttCmd(this[a, b], 0, 0);
                        if (!x.Contains("OK"))
                        {
                            Log.log.ErrorFormat("Signal Path ID : {2} A{0}B{1} Reset Error!", a, b, this[a, b]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal void SetAtt(List<Channel> Channels)
        {
            try
            {
                foreach (var channel in Channels)
                {
                    int currentAtt = CurrentAtt(this[channel.APortID, channel.BPortID]);
                    int offset = (int)(Math.Round(channel.AttOffset / AttenuationStep) % 240);
                    var x = SetAttCmd(this[channel.APortID, channel.BPortID], (currentAtt + offset));
                    if (!x.Contains("OK"))
                    {
                        Log.log.ErrorFormat("Signal Path ID : A{0}B{1} Set Attenuation Value Error!", channel.APortID, channel.BPortID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
