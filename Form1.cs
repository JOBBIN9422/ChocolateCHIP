using System;
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

namespace ChocolateCHIP
{
    public partial class Form1 : Form
    {
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

        Chip8 chip8;
        Thread SoundTimerThread;
        Thread DelayTimerThread;
        public Form1()
        {
            chip8 = new Chip8();
            chip8.Initialize();
            chip8.LoadROM("puzzle.ch8");
            chip8.printMemory();

            SoundTimerThread = new Thread(()=>chip8.TickSoundTimer());
            DelayTimerThread = new Thread(()=>chip8.TickDelayTimer());

            Application.Idle += HandleApplicationIdle;
            InitializeComponent();
        }

        public void HandleApplicationIdle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                RunCPU();
            }
        }

        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        public void RunCPU()
        {
                chip8.EmulateCycle();
                chip8.DebugRender();
                if (chip8.GetDrawFlag())
            {
                frameBox.Image = chip8.GetFrame();
                chip8.SetDrawFlag(false);
            }
                
                System.Threading.Thread.Sleep(10);
                //Console.Clear();
        }
    }
}
