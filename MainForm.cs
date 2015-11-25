using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Media;
using System.Xml;

namespace ClassCheck
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;//该变量表示将向Windows发送的消息类型 
        public const int SC_MOVE = 0xF010;//该变量表示发送消息的附加消息       
        public const int HTCAPTION = 0x0002;//该变量表示发送消息的附加消息
        // 图片名称
        public const String IMG_MIN = "btn_min";
        public const String IMG_MAX = "btn_max";
        public const String IMG_RESTORE = "btn_restore";
        public const String IMG_CLOSE = "btn_close";
        public const String IMG_BG = "img_bg";

        // 图片缓存
        private Bitmap closeBmp = null;
        private Bitmap minBmp = null;
        private Bitmap maxBmp = null;
        private Bitmap restoreBmp = null;

        // 初始化界面的方法
        private void initForm()
        {
            // 获取最大化、最小化、关闭的背景图片
            this.minBmp = Module.GetResAsImage(IMG_MIN);
            this.maxBmp = Module.GetResAsImage(IMG_MAX);
            this.closeBmp = Module.GetResAsImage(IMG_CLOSE);
            this.restoreBmp = Module.GetResAsImage(IMG_RESTORE);

            // 设置Tip提示信息
            this.TipMain.SetToolTip(this.BtnClose, "关闭");
            this.TipMain.SetToolTip(this.BtnMin, "最小化");


        }
        public Form1()
        {
            InitializeComponent();
            initForm();
        }


        public string errinfo = "614BAE1C5BC06A5CC655A1843B4A4004";
        public string buzou = "1";
        public string wl = null;
        public string pv = null;
        public string hv = null;
        public string ty = null;
        public string ip = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey location = Registry.LocalMachine;
                RegistryKey soft = location.OpenSubKey("SOFTWARE", false);
                RegistryKey myPass = soft.OpenSubKey("FTLiang", false);
                if (myPass.GetValue("ly").ToString().Trim() != null)
                {
                    ly.Text = myPass.GetValue("ly").ToString().Trim();
                }
                if (myPass.GetValue("js").ToString().Trim() != null)
                {
                    js.Text = myPass.GetValue("js").ToString().Trim();
                }
                if (myPass.GetValue("xinhao").ToString().Trim() != null)
                {
                    Module.xinhao = myPass.GetValue("xinhao").ToString().Trim();
                }
            }
            catch (Exception)
            {

            }
              openty();
            wltest();
        }


        private void ly_SelectedIndexChanged(object sender, EventArgs e)
        {
              ClassUp.ClassDataUpSoapClient ws = new ClassUp.ClassDataUpSoapClient();
             js.DataSource=ws.ReaderClass(ly.Text, errinfo);
             this.js.DisplayMember = "ClassRoom.Js";
             this.js.ValueMember = "ClassRoom.Js";
             ws.Close();

        }


        private void ping()
        {
            wl1.Enabled = true;
            wl2.Enabled = true;
            wltest();
            Process p = null;
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string strOutput = "ping 100.0.0.1"; //这里就是你的 telnet ip 命令
            p.StandardInput.WriteLine(strOutput);
            p.StandardInput.WriteLine("exit");
            MessageBox.Show(p.StandardOutput.ReadToEnd());
        }

        public void wltest()
        {
            ShowIP();
            try
            {
                Ping pi = new Ping();
                PingReply reply = pi.Send("100.0.0.100");
                if (reply.Status == IPStatus.Success)
                {
                    wl1.Checked = true;
                    wl2.Checked = false;
                    wl1.Enabled = false;
                    wl2.Enabled = false;
                    wl = "正常";
                }
                else
                {
                    wl1.Checked = false;
                    wl2.Checked = true;
                    wl1.Enabled = false;
                    wl2.Enabled = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private void pv1_CheckedChanged(object sender, EventArgs e)
        {
            if (pv1.Checked)
            {
                pv1.Checked = true;
                pv2.Checked = false;
                pv = "正常";
            }

        }

        private void pv2_CheckedChanged(object sender, EventArgs e)
        {
            if (pv2.Checked)
            {
                pv1.Checked = false;
                pv2.Checked = true;
                pv = "不正常";
            }
        }

        private void hv1_CheckedChanged(object sender, EventArgs e)
        {
            if (hv1.Checked)
            {
                hv1.Checked = true;
                hv2.Checked = false;
                hv = "正常";
            }
        }

        private void hv2_CheckedChanged(object sender, EventArgs e)
        {
            if (hv2.Checked)
            {
                hv2.Checked = true;
                hv1.Checked = false;
                hv = "不正常";
            }
        }

        private void ty1_CheckedChanged(object sender, EventArgs e)
        {
            if (ty1.Checked)
            {
                ty1.Checked = true;
                ty2.Checked = false;
                ty = "正常";
            }
        }

        private void ty2_CheckedChanged(object sender, EventArgs e)
        {
            if (ty2.Checked)
            {
                ty2.Checked = true;
                ty1.Checked = false;
                ty = "不正常";
            }
        }

        public void ShowIP()
        {
            ip = null;
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        if ("10" == IpEntry.AddressList[i].ToString().Substring(0, 2) && ip == null)
                        {
                            ip = IpEntry.AddressList[i].ToString();
                        }
                        else if ("10" == IpEntry.AddressList[i].ToString().Substring(0, 2) && ip != null)
                        {
                            ip = ip + "," + IpEntry.AddressList[i].ToString();
                        }
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取本机IP出错:" + ex.Message);
                return;
            }
        }

        private void sytest()
        {

            SoundPlayer sp = new SoundPlayer(ClassCheck.Properties.Resources.music);
            sp.Play();
        }

        public void openty()
        {
            try
            {
                int i = Convert.ToInt32(Module.xinhao);
                SerialPort serialPort = new SerialPort();
                serialPort.PortName = Module.com;
                if (0 == i)//SONY-VGA
                {
                    serialPort.BaudRate = 38400;
                    serialPort.Parity = Parity.Even;//偶
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferopen0, 0, Module.bufferopen0.Length);
                }
                if (1 == i)//NEC-H
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferopen1, 0, Module.bufferopen1.Length);
                }
                if (2 == i)//NEC-VGA
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferopen2, 0, Module.bufferopen2.Length);
                }
                if (3 == i)//NP361-H
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferopen3, 0, Module.bufferopen3.Length);
                }
                if (4 == i)//激光-H2
                {
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferopen4, 0, Module.bufferopen4.Length);
                }
                serialPort.Close();
            }
            catch
            {
            }
        }

        public void closety()
        {
            try
            {
                int i = Convert.ToInt32(Module.xinhao);
                SerialPort serialPort = new SerialPort();
                serialPort.PortName = Module.com;
                if (0 == i)//SONY-VGA
                {
                    serialPort.BaudRate = 38400;
                    serialPort.Parity = Parity.Even;//偶
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferclose0, 0, Module.bufferclose0.Length);
                    serialPort.Write(Module.bufferclose0, 0, Module.bufferclose0.Length);
                }
                if (1 == i)//NEC-H
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferclose1, 0, Module.bufferclose1.Length);
                    serialPort.Write(Module.bufferclose1, 0, Module.bufferclose1.Length);
                }
                if (2 == i)//NEC-VGA
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferclose2, 0, Module.bufferclose2.Length);
                    serialPort.Write(Module.bufferclose2, 0, Module.bufferclose2.Length);
                }
                if (3 == i)//NP-M361X-H1
                {
                    serialPort.BaudRate = 19200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferclose3, 0, Module.bufferclose3.Length);
                    serialPort.Write(Module.bufferclose3, 0, Module.bufferclose3.Length);
                }
                if (4 == i)//激光-H2
                {
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = Parity.None;//无
                    serialPort.DataBits = 8;
                    serialPort.Open();
                    serialPort.Write(Module.bufferclose4, 0, Module.bufferclose4.Length);
                    serialPort.Write(Module.bufferclose4, 0, Module.bufferclose4.Length);
                }
                serialPort.Close();
            }
            catch
            {
            }
        }

        public void shutdown()
        {
            int time = 5;
            var startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            var myProcess = new System.Diagnostics.Process();
            myProcess.StartInfo = startInfo;
            myProcess.Start();
            myProcess.StandardInput.WriteLine("shutdown -s -t " + time);

        }

        private void BtnWnd_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender == this.BtnClose)
                {
                    this.Close();
                }
                else if (sender == this.BtnMin)
                {
                    if (!this.ShowInTaskbar)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        // 鼠标进入
        private void BtnWnd_MouseEnter(object sender, EventArgs e)
        {
            Bitmap backImage = null;
            if (sender == this.BtnClose)
            {
                backImage = Module.GetResWithState(IMG_CLOSE, ControlState.MouseOver);
            }
            else if (sender == this.BtnMin)
            {
                backImage = Module.GetResWithState(IMG_MIN, ControlState.MouseOver);
            }
            else
            {
                return;
            }
            Control control = (Control)sender;
            control.BackgroundImage = backImage;
            control.Invalidate();
        }

        // 鼠标移开
        private void BtnWnd_MouseLeave(object sender, EventArgs e)
        {
            Bitmap backImage = null;
            if (sender == this.BtnClose)
            {
                backImage = closeBmp;
            }
            else if (sender == this.BtnMin)
            {
                backImage = minBmp;
            }
            else
            {
                return;
            }
            Control control = (Control)sender;
            control.BackgroundImage = backImage;
            control.Invalidate();
        }

        // 鼠标按下
        private void BtnWnd_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap backImage = null;
            if (sender == this.BtnClose)
            {
                backImage = Module.GetResWithState(IMG_CLOSE, ControlState.MouseDown);
            }
            else if (sender == this.BtnMin)
            {
                backImage = Module.GetResWithState(IMG_MIN, ControlState.MouseDown);
            }
            else
            {
                return;
            }
            Control control = (Control)sender;
            control.BackgroundImage = backImage;
            control.Invalidate();
        }

        // 鼠标弹起
        private void BtnWnd_MouseUp(object sender, MouseEventArgs e)
        {
            Bitmap backImage = null;
            if (sender == this.BtnClose)
            {
                backImage = closeBmp;
            }
            else if (sender == this.BtnMin)
            {
                backImage = minBmp;
            }
            else
            {
                return;
            }
            Control control = (Control)sender;
            control.BackgroundImage = backImage;
            control.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);

        }

 
        private void panel2_MouseClick(object sender, MouseEventArgs e)     
        {

            if (this.ly.SelectedIndex == -1 || this.js.SelectedIndex == -1 || wl == null || pv == null || hv == null || ty == null)
            {
                MessageBox.Show("请检查完毕再提交！");
                return;
            }
            else
            {
                ClassUp.ClassDataUpSoapClient ws = new ClassUp.ClassDataUpSoapClient();
                RegistryKey location = Registry.LocalMachine;
                RegistryKey soft = location.OpenSubKey("SOFTWARE", true);//可写 
                RegistryKey myPass = soft.CreateSubKey("FTLiang");
                myPass.SetValue("ly", ly.Text.Trim());
                myPass.SetValue("js", js.Text.Trim());
                string re = ws.Up(errinfo, ly.Text, js.Text, wl, pv, hv, ty, ip);
                if ("检查结果上传成功！" == re)
                {
                    msg.Visible = true;
                    msg.Text = "检查结果上传成功,5秒后自动关机关投影！";
                    closety();
                    //shutdown();
                }
                else if ("检查结果上传失败！" == re)
                {
                    MessageBox.Show("检查结果上传失败！");
                }
                else if ("此教室今天检查已在存在！" == re)
                {
                    MessageBox.Show("此教室今天检查已在存在!");
                }
                else
                {
                    MessageBox.Show("服务器异常！");
                }
                ws.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (this.ly.SelectedIndex == -1 || this.js.SelectedIndex == -1 || wl == null || pv == null || hv == null || ty == null)
            {
                MessageBox.Show("请检查完毕再提交！");
                return;
            }
            else
            {
                ClassUp.ClassDataUpSoapClient ws = new ClassUp.ClassDataUpSoapClient();
                RegistryKey location = Registry.LocalMachine;
                RegistryKey soft = location.OpenSubKey("SOFTWARE", true);//可写 
                RegistryKey myPass = soft.CreateSubKey("FTLiang");
                myPass.SetValue("ly", ly.Text.Trim());
                myPass.SetValue("js", js.Text.Trim());
                string re = ws.Up(errinfo, ly.Text, js.Text, wl, pv, hv, ty, ip);
                if ("检查结果上传成功！" == re)
                {
                    msg.Visible = true;
                    msg.Text = "检查结果上传成功,5秒后自动关机关投影！";
                    closety();
                    //shutdown();
                }
                else if ("检查结果上传失败！" == re)
                {
                    MessageBox.Show("检查结果上传失败！");
                }
                else if ("此教室今天检查已在存在！" == re)
                {
                    MessageBox.Show("此教室今天检查已在存在!");
                }
                else
                {
                    MessageBox.Show("服务器异常！");
                }
                ws.Close();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            sytest();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            sytest();
        }

        private void label6_MouseClick(object sender, MouseEventArgs e)
        {
            ping();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            ping();
        }
    }
}
