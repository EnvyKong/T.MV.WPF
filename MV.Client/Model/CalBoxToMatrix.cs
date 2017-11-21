using System;
using System.Windows;

namespace MV.Client.Model
{
    public class CalBoxToMatrix : CalBox
    {
        public CalBoxToMatrix(string ip, int portNum) : base(ip, portNum)
        {

        }

        public void Set64B16Switch(int a, int b)
        {
            if (!RouteChangeTo(1, Convert.ToInt32(a)).Contains("OK"))
            {
                MessageBox.Show("失败");
            }
            if (!RouteChangeTo(2, Convert.ToInt32(b)).Contains("OK"))
            {
                MessageBox.Show("失败");
            }
        }

        //internal void Set64B16Switch(int aPortID, int cPortID, int v1, int v2)
        //{
        //    throw new NotImplementedException();
        //}

        public void Set64B16Switch(int portanum, int portbnum, int switchD, int switchB)
        {
            //此处为什么强行切  开关9，和，10  ==》开关示意图如下
            /*
            ---
            |1|
            ---                                            ----
            ---                                            |11|
            |2|                                            ----
            ---
            ---                   -----       ----
            |3|                   | 9 |       |10|
            ---                   -----       ----
             :
             :                                             ----
            ---                                            |12|
            |8|                                            ----
            ---

             */
            #region 选择单刀8开关
            if (portanum > 8 && portanum <= 16)
            {
                switchD = 2;
            }
            if (portanum > 16 && portanum <= 24)
            {
                switchD = 3;
            }
            if (portanum > 24 && portanum <= 32)
            {
                switchD = 4;
            }
            if (portanum > 32 && portanum <= 40)
            {
                switchD = 5;
            }
            if (portanum > 40 && portanum <= 48)
            {
                switchD = 6;
            }
            if (portanum > 48 && portanum <= 56)
            {
                switchD = 7;
            }
            if (portanum > 56 && portanum <= 64)
            {
                switchD = 8;
            }
            if (portbnum > 8)
            {
                switchB = 2;
            }
            #endregion 选择单刀8开关
            #region 切单刀8开关
            if (switchD == 1)
            {
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum)).Contains("OK"))
                {
                    MessageBox.Show("Set Value Failed!");
                    return;
                }
                //两个八选一开关选择
            }
            if (switchD == 2)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 8));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 8)).Contains("OK"))
                {
                    MessageBox.Show("Set Value Failed!");
                    return;
                }
                //两个八选一开关选择
            }
            if (switchD == 3)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 16));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 16)).Contains("OK"))
                {
                    MessageBox.Show("Set Value Failed!");
                    return;
                }
                //两个八选一开关选择
            }
            if (switchD == 4)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 24));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 24)).Contains("OK"))
                {
                    MessageBox.Show("Set Value Failed!");
                    return;
                }
                //两个八选一开关选择
            }
            if (switchD == 5)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 32));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 32)).Contains("OK"))
                {
                    if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 32)).Contains("OK"))
                    {
                        if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 32)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
                //两个八选一开关选择
            }
            if (switchD == 6)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 40));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 40)).Contains("OK"))
                {
                    if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 40)).Contains("OK"))
                    {
                        if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 40)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
                //两个八选一开关选择
            }
            if (switchD == 7)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 48));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 48)).Contains("OK"))
                {
                    if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 48)).Contains("OK"))
                    {
                        if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 48)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
                //两个八选一开关选择
            }
            if (switchD == 8)
            {
                //RouteChangeTo(switchD, Convert.ToInt32(portanum - 56));
                if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 56)).Contains("OK"))
                {
                    if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 56)).Contains("OK"))
                    {
                        if (!RouteChangeTo(switchD, Convert.ToInt32(portanum - 56)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
                //两个八选一开关选择
            }
            #endregion 切单刀8开关
            if (!RouteChangeTo(9, Convert.ToInt32(switchD)).Contains("OK"))
            {
                if (!RouteChangeTo(9, Convert.ToInt32(switchD)).Contains("OK"))
                {
                    if (!RouteChangeTo(9, Convert.ToInt32(switchD)).Contains("OK"))
                    {
                        MessageBox.Show("Set Value Failed!");
                        return;
                    }
                }
            }
            if (!RouteChangeTo(10, Convert.ToInt32(switchB)).Contains("OK"))
            {
                if (!RouteChangeTo(10, Convert.ToInt32(switchB)).Contains("OK"))
                {
                    if (!RouteChangeTo(10, Convert.ToInt32(switchB)).Contains("OK"))
                    {
                        MessageBox.Show("Set Value Failed!");
                        return;
                    }
                }
            }
            if (switchB == 1)
            {
                if (!RouteChangeTo(11, Convert.ToInt32(portbnum)).Contains("OK"))
                {
                    if (!RouteChangeTo(11, Convert.ToInt32(portbnum)).Contains("OK"))
                    {
                        if (!RouteChangeTo(11, Convert.ToInt32(portbnum)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
            }
            else
            {
                if (!RouteChangeTo(12, Convert.ToInt32(portbnum - 8)).Contains("OK"))
                {
                    if (!RouteChangeTo(12, Convert.ToInt32(portbnum - 8)).Contains("OK"))
                    {
                        if (!RouteChangeTo(12, Convert.ToInt32(portbnum - 8)).Contains("OK"))
                        {
                            MessageBox.Show("Set Value Failed!");
                            return;
                        }
                    }
                }
            }
        }
    }
}
