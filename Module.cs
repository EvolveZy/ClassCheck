using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassCheck
{
    class Module
    {
        public static string com = "COM1";
        public static string xinhao = null;
        public static string ServerPage = "http://100.120.0.112/Default.asmx";

        //SONY
        public static byte[] bufferopen0 = new byte[] { 0xA9, 0x17, 0x2E, 0x00, 0x00, 0x00, 0x3F, 0x9A }; //开
        public static byte[] bufferv0 = new byte[] { 0xA9, 0x17, 0x2B, 0x00, 0x00, 0x00, 0x3F, 0x9A };//信号源VGA
        public static byte[] bufferclose0 = new byte[] { 0xA9, 0x17, 0x2F, 0x00, 0x00, 0x00, 0x3F, 0x9A }; //关

        //NEC-H
        public static byte[] bufferopen1 = new byte[] { 0x02, 0x0F, 0x00, 0x00, 0x02, 0x02, 0x00, 0x15 }; //开
        public static byte[] bufferv1 = new byte[] { 0x02, 0x03, 0x00, 0x00, 0x02, 0x01, 0x1A, 0x22 };//信号源
        public static byte[] bufferclose1 = new byte[] { 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16, 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16 }; //关

        //NEC-V
        public static byte[] bufferopen2 = new byte[] { 0x02, 0x0F, 0x00, 0x00, 0x02, 0x02, 0x00, 0x15 }; //开
        public static byte[] bufferv2 = new byte[] { 0x03, 0xB0, 0x00, 0x00, 0x01, 0x2C, 0xE0, 0x02, 0x0F, 0x00, 0x00, 0x02, 0x4B, 0x00, 0x5E };//信号源VGA
        public static byte[] bufferclose2 = new byte[] { 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16, 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16 }; //关

        //NP-M361X
        public static byte[] bufferopen3 = new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00, 0x02 }; //开
        public static byte[] bufferv3 = new byte[] { 0x02, 0x03, 0x00, 0x00, 0x02, 0x01, 0x1A, 0x22 };//信号源HDMI
        public static byte[] bufferclose3 = new byte[] { 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16, 0x02, 0x0F, 0x00, 0x00, 0x02, 0x03, 0x00, 0x16 }; //关

        //激光
        public static byte[] bufferopen4 = new byte[] { 0x5A, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5A }; //开
        public static byte[] bufferv4 = new byte[] { 0x5A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5A };//信号源HDMI2
        public static byte[] bufferclose4 = new byte[] { 0x5A, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5A }; //关



        /// <summary>
        /// 根据资源名称获取图像
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>图像</returns>
        public static Bitmap GetResAsImage(string name)
        {
            if (name == null || name == "")
            {
                return null;
            }
            return (Bitmap)Properties.Resources.ResourceManager.GetObject(name);
        }

        /// <summary>
        /// 图片按钮的背景图是4个,根据状态获取其中背景图
        /// </summary>
        /// <param name="name">图片名称</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public static Bitmap GetResWithState(String name, ControlState state)
        {
            Bitmap bitmap = (Bitmap)GetResAsImage(name);
            if (bitmap == null)
            {
                return null;
            }
            int block = 0;
            switch (state)
            {
                case ControlState.Normal: block = 0; break;
                case ControlState.MouseOver: block = 1; break;
                case ControlState.MouseDown: block = 2; break;
                case ControlState.Disable: block = 3; break;
            }
            int width = bitmap.Width / 4;
            Rectangle rect = new Rectangle(block * width, 0, width, bitmap.Height);
            return bitmap.Clone(rect, bitmap.PixelFormat);
        }
    }
}
