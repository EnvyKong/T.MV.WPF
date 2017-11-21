using System;

namespace MV.Client.Model
{
    public abstract class CalBox : Device
    {
        public CalBox(string ip, int portNum) : base(ip, portNum)
        {

        }

        public CalBoxData CalBoxData { get; set; }

        public void SetSwitch(int portNumID)
        {
            var sw1 = (portNumID / 8) + 1;
            var swx = portNumID % 8;
            if (swx == 0)
            {
                swx = 8;
            }
            if (Connected)
            {
                CutSwitch(1, sw1);
                CutSwitch(sw1, swx);
            }
            else
            {
                throw new Exception("错误：开关盒未连接！");
            }
        }

        private string CutSwitch(int switchID, int pin)
        {
            //string[] strMsg = new string[1];
            //strMsg[0] = "SETSW:" + switchID.ToString() + ":" + pin.ToString();
            //SendCommand((int)UserCmd.SET, strMsg);
            ////Thread.Sleep(200);
            //if (_setValueInterval < 0)
            //{
            //    if (WaitCmdResponse((int)UserCmd.SET, 1000))
            //    {
            //        return CommonDataArr[(int)UserCmd.SET].GetData();
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            //    Thread.Sleep(_setValueInterval);
            return "";
            //}
        }

        internal string RouteChangeTo(int v1, int v2)
        {
            Cmd = $"SET:{v1}:{v2}";
            return Send(Cmd);
        }

        internal string GetCalBoxDataCmd(int frequency)
        {
            Cmd = $"READcb:{frequency}";
            return Send(Cmd);
        }

        public void GetCalBoxData()
        {
            CalBoxData = new CalBoxData();
            string result = GetCalBoxDataCmd((int)Frequency * 1000);
            result.Replace("\r\n", "");
            string[] calBoxVal = result.Split(':')[2].Split(';');
            for (int n = 1; n <= calBoxVal.Length; n++)
            {
                if (n >= 1 && n <= 64)
                {
                    if (calBoxVal[n - 1].Contains(","))
                    {
                        CalBoxData.APortDataList.Add(new PortData()
                        {
                            Phase = calBoxVal[n - 1].Split(',')[0].ToDouble(),
                            Attenuation = calBoxVal[n - 1].Split(',')[1].ToDouble()
                        });
                    }
                    else
                    {
                        CalBoxData.APortDataList.Add(new PortData()
                        {
                            Phase = Convert.ToDouble(calBoxVal[n - 1]),
                            Attenuation = 0
                        });
                    }
                }
                else if (n >= 65 && n <= 80)
                {
                    if (calBoxVal[n - 1].Contains(","))
                    {
                        CalBoxData.BPortDataList.Add(new PortData()
                        {
                            Phase = calBoxVal[n - 1].Split(',')[0].ToDouble(),
                            Attenuation = calBoxVal[n - 1].Split(',')[1].ToDouble()
                        });
                    }
                    else
                    {
                        CalBoxData.BPortDataList.Add(new PortData()
                        {
                            Phase = Convert.ToDouble(calBoxVal[n - 1]),
                            Attenuation = 0
                        });
                    }
                }
            }
        }
        //result.TrimEnd(new char[] { '\'});
        //Random r = new Random();
        //for (int n = 1; n <= 80; n++)
        //{
        //    if (n >= 1 && n <= 64)
        //    {
        //        calBoxData.LeftPortDatas.Add(new PortData()
        //        {
        //            PortID = n,
        //            Phase = r.Next(-180, 180),
        //            Attenuation = -1
        //        });
        //    }
        //    else if (n >= 65 && n <= 80)
        //    {
        //        calBoxData.RightPortDatas.Add(new PortData()
        //        {
        //            PortID = n,
        //            Phase = r.Next(-180, 180),
        //            Attenuation = -1
        //        });
        //    }
        //}
        //return calBoxData;

        //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en", true);
        //calBoxToMatrix.ReadCB(out string result);
        //string result = File.ReadAllText("1.txt");
        //result.Replace("\r\n", "");
        #region MyRegion
        //if (result != "FAIL" && result != "")
        //{
        //    if (result.Contains(','))
        //    {
        //        //CalBoxIsOld = false;
        //        string[] calBoxVal = result.Split(';');
        //        for (int n = 1; n <= calBoxVal.Length; n++)
        //        {
        //            //calBoxData.Add(new CalBoxData
        //            //{
        //            //    Index = index,
        //            //    Phase = Convert.ToDouble(calBoxVal[i]),
        //            //    Attenuation = -1
        //            //});
        //            if (n >= 1 && n <= 16)
        //            {
        //                CalBoxData.APortsDatas.Add(new PortData()
        //                {
        //                    Phase = Convert.ToDouble(calBoxVal[n - 1].Split(',')[2]),
        //                    Attenuation = -1
        //                });
        //            }
        //            else if (n >= 17 && n <= 24)
        //            {
        //                CalBoxData.BPortsDatas.Add(new PortData()
        //                {
        //                    Phase = Convert.ToDouble(calBoxVal[n - 1].Split(',')[2]),
        //                    Attenuation = -1
        //                });
        //            }
        //        }
        //        //for (int i = 0; i < calBoxVal.Length; i++)
        //        //{
        //        //calBoxData.Add(new CalBoxData
        //        //{
        //        //    Index = index,
        //        //    Phase = Convert.ToDouble(calBoxVal[i].Split(',')[0]),
        //        //    Attenuation = -1
        //        //});
        //        //_calBoxData[i + portabNum].Data = Convert.ToDouble(calBoxVal[i].Split(',')[1]);
        //        //}
        //    }
        //    else
        //    {
        //        //CalBoxIsOld = true;
        //        string[] calBoxVal = result.Split(';');
        //        for (int n = 1; n <= calBoxVal.Length; n++)
        //        {
        //            //calBoxData.Add(new CalBoxData
        //            //{
        //            //    Index = index,
        //            //    Phase = Convert.ToDouble(calBoxVal[i]),
        //            //    Attenuation = -1
        //            //});
        //            if (n >= 1 && n <= 64)
        //            {
        //                CalBoxData.APortsDatas.Add(new PortData()
        //                {
        //                    Phase = Convert.ToDouble(calBoxVal[n]),
        //                    Attenuation = -1
        //                });
        //            }
        //            else if (n >= 65 && n <= 80)
        //            {
        //                CalBoxData.BPortsDatas.Add(new PortData()
        //                {
        //                    Phase = Convert.ToDouble(calBoxVal[n]),
        //                    Attenuation = -1
        //                });
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
