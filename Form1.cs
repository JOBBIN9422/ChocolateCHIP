﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ChocolateCHIP
{
    public partial class Form1 : Form
    {
        //Event loop helper functions/structs
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        private Chip8 chip8;
        private Thread SoundTimerThread;
        private Thread DelayTimerThread;

        //Scale a bitmap to a given size (used to scale CHIP-8's 64 x 32 frame to a viewable size)
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor; //NO FILTERING!!
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Form1()
        {
            chip8 = new Chip8();
            chip8.Initialize();

            Application.Idle += HandleApplicationIdle;
            InitializeComponent();
            clockSpdUpDown.Value = chip8.GetClockSpeedHz();
        }

        public void HandleApplicationIdle(object sender, EventArgs e)
        {
            //Winforms event loop wrapper
            while (IsApplicationIdle())
            {
                if (!debugCheckBox.Checked)
                {
                    RunCPU();
                    //simulate clock tick by suspending the main thread 
                    int sleepTimeMS = (int)(((double)1 / chip8.GetClockSpeedHz()) * 1000);
                    Thread.Sleep(sleepTimeMS);
                }
            }
        }

        //check if the winforms app is currently idle (used for CHIP8 event loop)
        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        //Execute 1 CPU cycle and render the result
        public void RunCPU()
        {
            chip8.EmulateCycle();
            if (printDebugCheckBox.Checked)
            {
                debugTextBox.Text = chip8.GetDebugInfo();
            }
            else
            {
                debugTextBox.Clear();
            }
            
            if (chip8.GetDrawFlag())
            {
                chip8.RenderToBitmap();
                frameBox.Image = Form1.ResizeImage(chip8.GetFrame(), 640, 320);
                chip8.SetDrawFlag(false);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            chip8.Reset();
            chip8.LoadROM(chip8.GetFileName());
            if (printDebugCheckBox.Checked)
            {
                debugTextBox.Text = chip8.GetDebugInfo();
            }
            else
            {
                debugTextBox.Clear();
            }
        }

        //Step through one CPU cycle for debug purposes
        private void StepButton_Click(object sender, EventArgs e)
        {
            RunCPU();
        }

        //view CHIP8 memory at a user-requested address for debug purposes
        private void ViewMemoryButton_Click(object sender, EventArgs e)
        {
            ushort addr = Convert.ToUInt16(memoryTextBox.Text, 16);
            string hexVal = "0x" + String.Format("{0:X4}", Convert.ToString(chip8.GetByteAtAddr(addr), 16));
            memoryTextBox.Text = hexVal;
        }

        private void SetClockSpeedButton_Click(object sender, EventArgs e)
        {
            uint newClockSpeed = (uint)clockSpdUpDown.Value;
            chip8.SetClockSpeedHz(newClockSpeed);
        }

        private void openROMButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openROMDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string ROMName = openROMDialog.FileName;
                chip8.Reset();
                try
                {
                    chip8.LoadROM(ROMName);
                    debugCheckBox.Checked = false;
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Please select a valid CHIP-8 ROM.", "ROM Load Error");
                }
            }
        }
    }
}
