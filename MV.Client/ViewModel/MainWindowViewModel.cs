using Microsoft.Win32;
using MV.Client.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using TopYoung.MV.Core;

namespace MV.Client.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand ConnectMatrixCommand { get { return new DelegateCommand(ConnectMatrix); } }
        public DelegateCommand ConnectVertexCommand { get { return new DelegateCommand(ConnectVertex); } }

        private void ConnectVertex()
        {
            try
            {
                for (int i = 0; i < VertexArray.Length; i++)
                {
                    VertexArray[i] = new Vertex(VertexArray[i].IP, 3000);
                    VertexArray[i].Connect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DelegateCommand ConnectAllDevicesCommand { get { return new DelegateCommand(ConnectAllDevices); } }

        private void ConnectAllDevices()
        {

        }

        public DelegateCommand CalibrateCommand { get { return new DelegateCommand(Calibrate); } }
        public DelegateCommand LoadOffsetCommand { get { return new DelegateCommand(LoadOffset); } }

        private void LoadOffset()
        {
            try
            {
                OpenFileDialog loadPhaseOffset = new OpenFileDialog()
                {
                    Filter = "Text File (*.txt)|*.txt|CSV File (*.csv)|*.csv",
                    Title = "Import Calibration Offset",
                    RestoreDirectory = true,
                    FilterIndex = 1
                };
                if (loadPhaseOffset.ShowDialog() != true)
                {
                    return;
                }
                //Matrix matrix = FrmDevice.GetFrmDevice().matrix;
                //Matrix matrix = SingletonFactory<FrmDevice>.CreateInstance().Matrix;
                var offsets = File.ReadAllLines(loadPhaseOffset.FileName);
                for (int i = 0; i < offsets.Length; i++)
                {
                    var offset = offsets[i].Split(':');
                    int id = int.Parse(offset[0]);
                    int pha = int.Parse(offset[1]);
                    int att = int.Parse(offset[2]);
                    Matrix.SetPhaAndAtt(id, pha, att);
                }
                MessageBox.Show("Calibrate successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Log.log.ErrorFormat("{0}", ex);
            }
        }

        public DelegateCommand LoadPhaseCommand { get { return new DelegateCommand(LoadPhase); } }
        public DelegateCommand OutputOffsetCommand { get { return new DelegateCommand(OutputOffset); } }

        private void OutputOffset()
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog()
                {
                    Filter = "Text File (*.txt)|*.txt|CSV File (*.csv)|*.csv",
                    Title = "Export Calibration Offset",
                    RestoreDirectory = true,
                    FilterIndex = 1,
                    FileName = "CalibrationData"
                };
                if (saveFile.ShowDialog() != true)
                {
                    return;
                }
                string savePath = saveFile.FileName;
                SaveTxtOrCsv(savePath);
                MessageBox.Show("Output Completed！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Log.log.ErrorFormat("{0}", ex);
            }
        }

        private void SaveTxtOrCsv(string savePath)
        {
            for (int a = 1; a <= Matrix.APortConnectNum; a++)
            {
                for (int b = 1; b <= Matrix.BPortConnectNum; b++)
                {
                    string[] offset = new string[1];
                    offset[0] = $"{Matrix[a, b]}:{Matrix.CurrentPha(Matrix[a, b])}:{Matrix.CurrentAtt(Matrix[a, b])}";
                    File.AppendAllLines(savePath, offset);
                }
            }
        }

        private void LoadPhase()
        {
            try
            {
                //SingletonFactory<FrmCalibration>.CreateInstance().InitViewInfo();

                OpenFileDialog loadPhase = new OpenFileDialog()
                {
                    Filter = "Text File (*.txt)|*.txt|CSV File (*.csv)|*.csv",
                    Title = "Load Phase Value",
                    RestoreDirectory = true,
                    FilterIndex = 1
                };
                if (loadPhase.ShowDialog() != true)
                {
                    return;
                }
                //Matrix matrix = FrmDevice.GetFrmDevice().matrix;
                //Matrix matrix = SingletonFactory<FrmDevice>.CreateInstance().Matrix;
                var channels = new List<Channel>();
                if (loadPhase.FileName.ToLower().EndsWith(".txt"))
                {
                    Matrix.LoadOffsets(loadPhase.FileName, out channels);
                }
                else if (loadPhase.FileName.ToLower().EndsWith(".csv"))
                {
                    Scene scene = new Scene();
                    scene.LoadSceneData(loadPhase.FileName);
                    foreach (var frame in scene.Frames)
                    {
                        foreach (var channel in frame.ChannelToMatrixCollection)
                        {
                            channels.Add(new Channel(channel.APortID, channel.BPortID)
                            {
                                PhaOffset = channel.Pha
                            });
                        }
                    }
                }

                if (MessageBox.Show("Load successfully! Play phase values now?", "Completed", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }
                Matrix.SetPha(channels);
                MessageBox.Show("Play successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Log.log.ErrorFormat("{0}", ex);
            }
        }

        private CalBoxToMatrix _calBoxToMatrix;

        public CalBoxToMatrix CalBoxToMatrix
        {
            get { return _calBoxToMatrix; }
            set
            {
                _calBoxToMatrix = value;
                RaisePropertyChanged("CalBoxToMatrix");
            }
        }

        private CalBoxToVertex _calBoxToVertex;

        public CalBoxToVertex CalBoxToVertex
        {
            get { return _calBoxToVertex; }
            set
            {
                _calBoxToVertex = value;
                RaisePropertyChanged("CalBoxToVertex");
            }
        }

        private Matrix _matrix;

        public Matrix Matrix
        {
            get { return _matrix; }
            set
            {
                _matrix = value;
                RaisePropertyChanged("Matrix");
            }
        }

        private List<SignalPath> _signalPathList;

        public List<SignalPath> SignalPathList
        {
            get { return _signalPathList; }
            set
            {
                _signalPathList = value;
                RaisePropertyChanged("SignalPathList");
            }
        }

        private IVectorNetworkAnalyzer _vna;

        public IVectorNetworkAnalyzer VNA
        {
            get { return _vna; }
            set
            {
                _vna = value;
                RaisePropertyChanged("VNA");
            }
        }

        private Vertex[] _vertexArray;

        public Vertex[] VertexArray
        {
            get { return _vertexArray; }
            set
            {
                _vertexArray = value;
                RaisePropertyChanged("VertexArray");
            }
        }

        private void ConnectMatrix()
        {
            try
            {
                //FrmDevice frmDevice = SingletonFactory<FrmDevice>.CreateInstance();//FrmDevice.GetFrmDevice();
                //string iP = cboIP.Text.ToString();

                //frmDevice.Matrix = new Matrix(iP, 3000);
                //Matrix matrix = frmDevice.Matrix;
                Matrix = new Matrix(Matrix.IP, 3000);
                Matrix.Connect();
                if (Matrix.Connected)
                {
                    //记录输入历史
                    ReMind.SetHistory(Matrix.IP);
                    var idn = Matrix.ReadIDN();
                    //SingletonFactory<FrmOutput>.CreateInstance().OutputLog = "Connect Successful! Device's IDN is: " + idn;
                    //frmDevice.DrawDevice("Connect Successful! Device's IDN is: " + idn);
                }
                else
                {
                    //frmDevice.DrawDevice("Connect Error!");
                }

                //frmDevice.Show((Owner as FrmMain).DockPanel);
            }
            catch (Exception ex)
            {
                Log.log.ErrorFormat("{0}", ex);
            }
        }

        public DelegateCommand ConnectVNACommand { get { return new DelegateCommand(ConnectVNA); } }

        private void ConnectVNA()
        {
            try
            {
                VNA = VNAFactory.GetVNA("vna ip", -1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public DelegateCommand ConnectCalBoxCommand { get { return new DelegateCommand(ConnectCalBox); } }

        private void ConnectCalBox()
        {
            CalBoxToMatrix = new CalBoxToMatrix(CalBoxToMatrix.IP, 3000);
            CalBoxToMatrix.Connect();
        }

        private void Calibrate()
        {
            try
            {
                Thread getCalBoxDatasThread = new Thread((calBoxToMatrix) =>
                {
                    (calBoxToMatrix as CalBoxToMatrix).GetCalBoxData();
                })
                {
                    CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
                    Name = "子线程：获取校准盒子数据。",
                    IsBackground = true
                };
                //获取校准盒子数据
                getCalBoxDatasThread.Start(CalBoxToMatrix);

                Thread resetMatrixThread = new Thread((matrix) =>
                {
                    var m = matrix as Matrix;
                    m.ResetAttAndPha();
                })
                {
                    CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
                    Name = "子线程：设置MCS衰减相位归零。",
                    IsBackground = true
                };
                //相位衰减置零
                resetMatrixThread.Start(Matrix);

                //等待子线程完成任务
                getCalBoxDatasThread.Join();
                resetMatrixThread.Join();


                int attCalFre = AppConfigInfo.AttCalFre;
                int phaCalFre = AppConfigInfo.PhaCalFre;
                int maxCalCount = attCalFre > phaCalFre ? attCalFre : phaCalFre;
                for (int i = 1; i <= maxCalCount; i++)
                {
                    if (i <= attCalFre)
                    {
                        //开始获取通道衰减
                        SignalPathList = GetAllSignalPathData(CalBoxToMatrix, CalBoxToVertex, VNA, Matrix, VertexArray);
                        if (Log.log.IsInfoEnabled)
                        {
                            Log.log.InfoFormat("通道总数量为{0}。Vertex台数为{1}。", SignalPathList.Count, VertexArray.Length);
                        }

                        //找到衰减最小值
                        SignalPath.AttStandard = SignalPathList.Select(s => s.Attenuation).Min();
                        //int indexMin = signalPaths.Select(s => s.Attenuation).ToList().IndexOf(SignalPath.AttStandard);

                        //得到衰减补偿值
                        //matrix.Channels = new List<Channel>();
                        //foreach (var signalPath in signalPaths)
                        //{
                        //    //signalPath.GetOffsetAttToMatrix();
                        //    matrix.Channels.Add(signalPath.ChannelToMatrix);
                        //}

                        Thread setMatrixAttThread = new Thread((matrix) =>
                        {
                            var m = matrix as Matrix;
                            m.SetAtt(SignalPathList.Select(s => s.ChannelToMatrix).ToList());
                        })
                        {
                            CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
                            Name = "子线程：设置MCS补偿衰减。",
                            IsBackground = true
                        };
                        setMatrixAttThread.Start(Matrix);
                        setMatrixAttThread.Join();

                        if (Log.log.IsInfoEnabled)
                        {
                            Log.log.InfoFormat("第{0}次衰减校准完成。", i);
                        }

                    }
                    if (i <= phaCalFre)
                    {
                        //matrix.Channels = new List<Channel>();
                        //取相位
                        for (int b = 1; b <= Matrix.BPortConnectNum; b++)
                        {
                            //下行
                            //vertex.OpenChannel(b, 1, UpDown.DOWN);
                            //vertexs[b / AppConfigInfo.VertexAConnectNum].OpenChannel(b % AppConfigInfo.VertexAConnectNum, 1, UpDown.DOWN);
                            var vertexID = (b - 1) / AppConfigInfo.VertexAConnectNum;
                            var inPortID = (b - 1) % AppConfigInfo.VertexAConnectNum + 1;
                            var outPortID = 1;

                            VertexArray[vertexID].OpenChannel(inPortID, outPortID, UpDown.DOWN);

                            if (Log.log.IsInfoEnabled)
                            {
                                Log.log.InfoFormat("第{0}台Vertex响应。打开通道{1}{2}，方向{3}。", vertexID, inPortID, outPortID, UpDown.DOWN);
                            }

                            for (int a = 1; a <= Matrix.APortConnectNum; a++)
                            {
                                var calBoxAPortID = a;
                                var calBoxBPortID = ((b - 1) / AppConfigInfo.VertexAConnectNum) * AppConfigInfo.VertexBConnectNum + 1;
                                CalBoxToMatrix.Set64B16Switch(calBoxAPortID, calBoxBPortID, 1, 1);
                                if (Log.log.IsInfoEnabled)
                                {
                                    Log.log.InfoFormat("相位校准阶段切开关 {0}{1} OK。", calBoxAPortID, calBoxBPortID);
                                }
                                SignalPathList.Find(s => s.Index.Equals($"{a}:{b}:1")).Phase = VNA.GetMarkerY(AppConfigInfo.PhaMarkPoint);
                            }
                            //vertex.CloseChannel(b, 1, UpDown.DOWN);
                            //vertexs[b / AppConfigInfo.VertexAConnectNum].CloseChannel(b % AppConfigInfo.VertexAConnectNum, 1, UpDown.DOWN);
                            VertexArray[vertexID].CloseChannel(inPortID, outPortID, UpDown.DOWN);

                            if (Log.log.IsInfoEnabled)
                            {
                                Log.log.InfoFormat("第{0}台Vertex响应。关闭通道{1}{2}，方向{3}。", vertexID, inPortID, outPortID, UpDown.DOWN);
                            }
                        }

                        Thread setMatrixPhaThread = new Thread((matrix) =>
                        {
                            var m = matrix as Matrix;
                            m.SetPha(SignalPathList.Select(s => s.ChannelToMatrix).ToList());
                        })
                        {
                            CurrentCulture = System.Globalization.CultureInfo.InvariantCulture,
                            Name = "子线程：设置MCS补偿相位。",
                            IsBackground = true,
                        };

                        setMatrixPhaThread.Start(Matrix);

                        setMatrixPhaThread.Join();

                        if (Log.log.IsInfoEnabled)
                        {
                            Log.log.InfoFormat("第{0}次相位校准完成。", i);
                        }
                    }
                }

                MessageBox.Show("MCS Calibrate Successfully! Please Start Vertex Self Calibrate!");
            }
            catch (Exception ex)
            {
                Log.log.ErrorFormat("{0}", ex);
                MessageBox.Show(ex.ToString());
            }
        }

        //校准方法，使用Vertex相位自校准
        private List<SignalPath> GetAllSignalPathData(CalBoxToMatrix calBoxToMatrix, CalBoxToVertex calBoxToVertex, IVectorNetworkAnalyzer VNA, Matrix matrix, Vertex[] vertex)
        {
            var signalPaths = new List<SignalPath>();
            //FrmOutput.GetFrmOutput().OutputLog = "Start The Calibration.";
            //SingletonFactory<FrmOutput>.CreateInstance().OutputLog = "Start The Calibration.";

            VNA.SetMarkerActive();
            VNA.SetMarkerX(matrix.Frequency * 1000000);
            //关闭Vertex所有通道，后面用哪个打开哪个
            foreach (var v in vertex)
            {
                v.CloseAllChannel(v.APortNum, v.BPortNum);
            }
            //for (int c = 1; c <= vertex.BPortNum; c++)
            //{
            for (int b = 1; b <= matrix.BPortConnectNum; b++)
            {
                //下行
                //var groupSignalPaths = new List<SignalPath>();
                var vertexID = (b - 1) / AppConfigInfo.VertexAConnectNum;
                var inPortID = (b - 1) % AppConfigInfo.VertexAConnectNum + 1;
                var outPortID = 1;

                vertex[vertexID].OpenChannel(inPortID, outPortID, UpDown.DOWN);

                if (Log.log.IsInfoEnabled)
                {
                    Log.log.InfoFormat("第{0}台Vertex响应。打开通道{1}{2}，方向{3}。", vertexID, inPortID, outPortID, UpDown.DOWN);
                }

                for (int a = 1; a <= matrix.APortConnectNum; a++)
                {
                    var calBoxAPortID = a;
                    var calBoxBPortID = ((b - 1) / AppConfigInfo.VertexAConnectNum) * AppConfigInfo.VertexBConnectNum + 1;

                    calBoxToMatrix.Set64B16Switch(calBoxAPortID, calBoxBPortID, 1, 1);

                    if (Log.log.IsInfoEnabled)
                    {
                        Log.log.InfoFormat("衰减校准阶段切开关 {0}{1} OK。", calBoxAPortID, calBoxBPortID);
                    }
                    //calBoxToMatrix.SetSwitch(a);
                    //calBoxToVertex.SetSwitch(c);

                    //vNA.SetMarkerActive();
                    //vNA.SetMarkerX(ViewConfigInfo.Frequency * 1000000);
                    var signalPath = new SignalPath(matrix, calBoxToMatrix.CalBoxData)
                    {
                        APortID = a,
                        BPortID = b,
                        CPortID = 1,
                        Attenuation = VNA.GetMarkerY(AppConfigInfo.AttMarkPoint),
                        //Phase = vNA.GetMarkerY(int.Parse(ConfigurationManager.AppSettings["Pha Mark Point"]))
                    };
                    //signalPath.GetOffsetAttToMatrix();
                    //signalPath.GetOffsetAttToVertex(UpDown.UP);
                    //signalPath.GetOffsetPhaToMatrix();
                    //不需要求Vertex补偿相位
                    //signalPath.GetOffsetPhaToVertex(UpDown.UP);
                    signalPaths.Add(signalPath);
                    //bgw.ReportProgress((1000 / 8192) * a * b * c);
                }
                vertex[vertexID].CloseChannel(inPortID, outPortID, UpDown.DOWN);

                if (Log.log.IsInfoEnabled)
                {
                    Log.log.InfoFormat("第{0}台Vertex响应。关闭通道{1}{2}，方向{3}。", vertexID, inPortID, outPortID, UpDown.DOWN);
                }
                //下行
                //vertex.OpenChannel(b, c, UpDown.DOWN);
                //foreach (var signalPath in groupSignalPaths)
                //{
                //    vNA.SetMarkerActive();
                //    vNA.SetMarkerX((long.Parse(CalConfigInfo.Frequency) * 1000000));
                //    //signalPath.Attenuation = Math.Abs(vNA.GetMarkerY(int.Parse(ConfigurationManager.AppSettings["Att Mark Point"])));
                //    signalPath.Phase = r.Next(-360, 360);//vNA.GetMarkerY(int.Parse(ConfigurationManager.AppSettings["Pha Mark Point"]));
                //    signalPath.GetOffsetPhaToVertex(UpDown.DOWN);
                //}
                //vertex.CloseChannel(b, c, UpDown.DOWN);
                //signalPaths.AddRange(groupSignalPaths);
            }
            //}
            //};
            //FrmProgressBar frmProgressBar = new FrmProgressBar("Connecting... ", ref bgw);
            //frmProgressBar.ShowDialog();
            return signalPaths;
        }

    }
}
