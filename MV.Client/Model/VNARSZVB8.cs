﻿using Ivi.Visa.Interop;
using System;
using System.Windows;
using TopYoung.MV.Core;

namespace MV.Client.Model
{
    class VNARSZVB8 : VNA, IVectorNetworkAnalyzer
    {
        public VNARSZVB8(string ip, int portNum) : base(ip, portNum)
        {

        }

        public override bool Connected { get; set; }

        public override void Connect()
        {
            try
            {
                messageBased = new FormattedIO488();
                ResourceManager grm = new ResourceManager();//TCPIP0::192.168.8.219::inst0::INSTR
                messageBased.IO = (IMessage)grm.Open("TCPIP0::" + IP + "::inst0::INSTR", AccessMode.NO_LOCK, 2000, "");
                messageBased.IO.Timeout = 200000;
                messageBased.IO.SendEndEnabled = !messageBased.IO.SendEndEnabled;
                messageBased.IO.TerminationCharacterEnabled = !messageBased.IO.TerminationCharacterEnabled;
                if (ReadIDN(out string IDN))
                {
                    Connected = true;
                }
                else
                {
                    Connected = false;
                }
            }
            catch (Exception ex)
            {
                Connected = false;
                Close();
                MessageBox.Show(ex.ToString());
            }
        }

        public void ActiveSegmentFreq()
        {
            throw new NotImplementedException();
        }

        public double GetFREQMAX()
        {
            throw new NotImplementedException();
        }

        public double GetFREQMIN()
        {
            throw new NotImplementedException();
        }

        public double GetMarkerY(int trace)
        {
            string strCmd = "";
            string sY;
            double dY;
            try
            {
                //Write("INIT1:CONT ON");
                ////Set the trigger source to Bus Trigger.
                //Write(":TRIG:SOUR BUS");
                ////Trigger the instrument to start a sweep cycle.
                //Write(":TRIG:SING");
                ////Execute the *OPC? command and wait until the command
                //QueryString("*OPC?");
                //strCmd = "CALC1:MARK1:Y?";2016-9-27,修改为设置Trace
                SelTrace("Trc" + trace);
                SetSingleSweepMode();
                strCmd = string.Format("CALC:MARK:Y?", trace);//读取trace1的marker数值  //测试命令CALC{0}:MARK:Y?
                sY = QueryString(strCmd);
                //Common.OutputForm.WriteMessage(sY);
                dY = Convert.ToDouble(sY.Split(',')[0]);
                return dY;
            }
            catch
            {
                Connected = false;
            }
            return 0;
        }

        private void SelTrace(string trace)
        {
            //CALC2:PAR:SDEF 'Trc2', 'S12'
            //string strCmd = ":CALC1:PAR1" + ":DEF " + trace;
            string strCmd = "CALC1:PAR:SEL '" + trace + "'";
            messageBased.WriteString(strCmd);
        }

        public double GetMarkerY()
        {
            throw new NotImplementedException();
        }

        public double[] GetMarkerY(double[] dy)
        {
            throw new NotImplementedException();
        }

        public void LoadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public double[] ReadFrq()
        {
            throw new NotImplementedException();
        }

        public string ReadIFBW(string channel)
        {
            throw new NotImplementedException();
        }

        public string ReadPower(string channel)
        {
            throw new NotImplementedException();
        }

        public string[] ReadStimulus()
        {
            throw new NotImplementedException();
        }

        public string[] ReadTraces(string trace, string format)
        {
            throw new NotImplementedException();
        }

        public void ReSetAnalyzer()
        {
            throw new NotImplementedException();
        }

        public void SelectFormat(string format)
        {
            throw new NotImplementedException();
        }

        public void SetAGC_Auto()
        {
            throw new NotImplementedException();
        }

        public void SetAGC_LNO()
        {
            throw new NotImplementedException();
        }

        public void SetAGC_MANual()
        {
            throw new NotImplementedException();
        }

        public void SetIFBW(int IfbwValue)
        {
            throw new NotImplementedException();
        }

        public void SetMarkerActive()
        {
            //激活后自动display marker点
            try
            {
                string strCmd = "CALC:MARK:ON";
                Write(strCmd);
            }
            catch
            {
                Connected = false;
            }
        }

        public void SetMarkerState(bool display)
        {
            throw new NotImplementedException();
        }

        public void SetMarkerX(int trace, long x)
        {
            throw new NotImplementedException();
        }

        public void SetMarkerX(long x)
        {
            string strCmd = "";
            try
            {
                strCmd = "CALC:MARK:X " + x.ToString();
                Write(strCmd);
            }
            catch
            {
                Connected = false;
            }
        }

        public void SetPower(string power)
        {
            throw new NotImplementedException();
        }

        public void SetSegmentFreqIns(string StartFreq, string StopFreq, int Points, string Power, string SegmentTime, string Unused, string MeasBandwidth)
        {
            throw new NotImplementedException();
        }

        public void SetSegmentPoint(int Points)
        {
            throw new NotImplementedException();
        }

        public string SetSingleSweepMode()
        {
            if (messageBased != null)
            {
                string strCmd = "INIT:CONT OFF; :INIT; *OPC?";
                messageBased.WriteString(strCmd);
                string value = messageBased.ReadString();
                return value;
            }
            return null;
        }

        public void SetStartFreq(string freq)
        {
            throw new NotImplementedException();
        }

        public void SetStopFreq(string freq)
        {
            throw new NotImplementedException();
        }

        public void SetTrace(string trace, string sParameter)
        {
            throw new NotImplementedException();
        }

        public void SetTraceNumber(int channel, int traceNum)
        {
            throw new NotImplementedException();
        }

        public void StoreFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
