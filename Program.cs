using System;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Chip8 chip8 = new Chip8();
            chip8.Initialize();
            chip8.LoadROM("particles.ch8");
            chip8.printMemory();

            while (true)
            {
                chip8.EmulateCycle();
                chip8.DebugRender();
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }

            Console.Read();
        }
    }
}
