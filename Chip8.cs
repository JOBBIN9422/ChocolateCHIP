using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ChocolateCHIP
{
    class Chip8
    {
        private ushort opcode;
        private ushort index;
        private ushort programCounter;
        private ushort stackPointer;
        private ushort delayTimer;
        private ushort soundTimer;

        private uint clockSpeedHz;
        private uint timerInterval;
        private ulong cycleCount;

        private bool drawFlag;

        private Random random;

        private string currFileName;

        private List<byte> ROM;

        private ushort[] stack;
        private byte[] memory;
        private byte[] vRegisters;
        private byte[] frame;
        private byte[] keyStates;
        private byte[] fontSet =
        {
              0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
              0x20, 0x60, 0x20, 0x20, 0x70, // 1
              0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
              0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
              0x90, 0x90, 0xF0, 0x10, 0x10, // 4
              0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
              0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
              0xF0, 0x10, 0x20, 0x40, 0x40, // 7
              0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
              0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
              0xF0, 0x90, 0xF0, 0x90, 0x90, // A
              0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
              0xF0, 0x80, 0x80, 0x80, 0xF0, // C
              0xE0, 0x90, 0x90, 0x90, 0xE0, // D
              0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
              0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        private Key[] keyBindings =
        {
              Key.X,
              Key.D1,
              Key.D2,
              Key.D3,
              Key.Q,
              Key.W,
              Key.E,
              Key.A,
              Key.S,
              Key.D,
              Key.Z,
              Key.C,
              Key.D4,
              Key.R,
              Key.F,
              Key.V
        };

        public Chip8()
        {
            random = new Random();
            currFileName = String.Empty;
            ROM = new List<byte>();
            this.clockSpeedHz = 500;
            this.cycleCount = 0;
            this.timerInterval = clockSpeedHz / 60;

            //4K of memory
            this.memory = new byte[4096];

            //stack has 16 16-bit values 
            this.stack = new ushort[16];

            //16 8-bit registers 
            this.vRegisters = new byte[16];

            //64x32 resolution
            this.frame = new byte[64 * 32];

            this.keyStates = new byte[16];
        }

        public void Initialize()
        {   
            opcode = 0;
            index = 0;
            stackPointer = 0;
            delayTimer = 0;
            soundTimer = 0;

            //load font set into memory
            for (int i = 0; i < 80; i++)
            {
                memory[i] = fontSet[i];
            }

            programCounter = 0x200; //start PC at ROM start address
            drawFlag = true; 
        }

        public void Reset()
        {
            this.memory = new byte[4096];
            this.stack = new ushort[16]; 
            this.vRegisters = new byte[16];
            this.frame = new byte[64 * 32];
            this.keyStates = new byte[16];

            //reload font set
            for (int i = 0; i < 80; i++)
            {
                memory[i] = fontSet[i];
            }

            opcode = 0;
            index = 0;
            stackPointer = 0;
            delayTimer = 0;
            soundTimer = 0;

            programCounter = 0x200; 
            drawFlag = true;
        }

        public void LoadROM(string fileName)
        {
            currFileName = fileName;
            MemoryStream ROMStream = new MemoryStream();

            using (var inputStream = File.Open(fileName, FileMode.Open))
            {
                inputStream.CopyTo(ROMStream);
            }

            //copy rom byte-wise into array
            byte[] ROMDump = ROMStream.ToArray();
            ROM.Clear();

            for (int i = 0; i < ROMDump.Count(); i++)
            {
                //copy ROM into memory at program starting address
                memory[i + 0x200] = ROMDump[i];
                ROM.Add(ROMDump[i]); //add byte to ROM list for disassembly reference (not read by interpreter at runtime)
                Console.WriteLine("ROM[{0:X4}] = {1:X4}\tMEM[{2:X4}] = {3:X4}", i, ROMDump[i], i + 0x200, memory[i + 0x200]);
            }
        }

        public List<string> GetROMInstructions()
        {
            List<string> ROMInstructions = new List<string>();
            for (int i = 0; i < ROM.Count - 1; i++)
            {
                //disassemble each instruction from ROM (2 bytes each)
                ushort currOpCode = (ushort)((ROM[i] << 8) | ROM[i + 1]);

                //calculate the mem address of each instruction 
                string currInstrAddr = String.Format("{0:X4} - ", i + 0x200);
                string currInstr = Disassemble(currOpCode);

                if (currInstr.Equals(String.Empty))
                {
                    ROMInstructions.Add(String.Empty);
                }
                else
                {
                    ROMInstructions.Add(currInstrAddr + Disassemble(currOpCode));
                } 
            }

            return ROMInstructions;
        }

        public uint GetClockSpeedHz()
        {
            return clockSpeedHz;
        }

        public ushort GetPC()
        {
            return programCounter;
        }

        public void SetClockSpeedHz(uint clockSpeed)
        {
            this.clockSpeedHz = clockSpeed;
        }


        public bool GetDrawFlag()
        {
            return drawFlag;
        }

        public void SetDrawFlag(bool drawFlag)
        {
            this.drawFlag = drawFlag;
        }

        public string GetFileName()
        {
            return currFileName;
        }


        public byte[] GetFrameBuffer()
        {
            return frame;
        }

        //dump the contents of memory to the console 
        public void printMemory()
        {
            for (int i = 0; i < memory.Count(); i++)
            {
                Console.WriteLine("mem [{0:X4}] = {1:X4}", i, memory[i]);
            }
        }

        //fetch data at the requested address in memory 
        public byte GetByteAtAddr(ushort addr)
        {
            return Convert.ToByte(memory[addr]);
        }

        //return a string containing information about the CPU/memory 
        public string GetDebugInfo()
        {
            string debugInfo = String.Empty;
            debugInfo += String.Format("PC = {0:X4}{1}SP = {2:X4}{3}I  = {4:X4}{5}", programCounter, Environment.NewLine, stackPointer, Environment.NewLine, index, Environment.NewLine);

            ushort currOpcode = (ushort)((memory[programCounter] << 8) | memory[programCounter + 1]);
            //Console.WriteLine(Disassemble(currOpcode));
            debugInfo += String.Format("OP = {0:X4}{1}", currOpcode, Environment.NewLine);

            debugInfo += String.Format("{0}DT = {1:X4}{2}ST = {3:X4}{4}", Environment.NewLine, delayTimer, Environment.NewLine, soundTimer, Environment.NewLine);

            debugInfo += String.Format("{0}Registers | Stack{1}-----------------------{2}", Environment.NewLine, Environment.NewLine, Environment.NewLine);

            string stackPoint = String.Empty;
            for (int i = 0; i < 16; i++)
            {
                if (i == stackPointer)
                {
                    stackPoint = "<-";
                }
                else
                {
                    stackPoint = String.Empty;
                }
                debugInfo += String.Format("V[{0:X}] = {1:X2} | S[{2:X}] = {3:X4}{4}{5}", i, vRegisters[i], i, stack[i], stackPoint, Environment.NewLine);
            }
            return debugInfo;
        }

        //Update the key state registers based on key presses
        public void PollKeys()
        {
            for (int i = 0; i < keyBindings.Count(); i++)
            {
                if (Keyboard.IsKeyDown(keyBindings[i]))
                {
                    keyStates[i] = 1;
                }
                else
                {
                    keyStates[i] = 0;
                }
            }
        }

        public void clearScreen()
        {
            for (int i = 0; i < 2048; i++)
            {
                frame[i] = 0x0;
            }
            drawFlag = true;
        }

        //pre-decrements the stack and sets the PC to the address at the top of stack
        public void Return()
        {
            stackPointer--;
            programCounter = stack[stackPointer];
            
        }

        //Overwrite the PC with the requested address
        public void JumpToAddr(ushort jumpAddr)
        {
            this.programCounter = jumpAddr;
        }

        //jump to address offset + regs[0]
        public void JumpToRegOffsetAddr(ushort offset)
        {
            this.programCounter = (ushort)(vRegisters[0] + offset);
        }

        //stores current PC on stack and jumps to subroutine address
        public void CallSubroutine(ushort subAddr)
        {

            
            stack[stackPointer] = programCounter; //store current addr on stack
            stackPointer++;
            programCounter = subAddr; //set program counter to subroutine address
        }

        //Skips next instruction if registers[N] == XX
        //Where:
        //      N  == upper 4 bits of data
        //      XX == lower 8 bits of data
        public void SkipEqual(ushort data, byte XData)
        {
            ushort compareValue =   (ushort) (data & 0x00FF);
            if (vRegisters[XData] == compareValue)
            {
                //skip next instruction (each instr is 2 bytes long)
                programCounter += 2;
            }
        }

        //Skips next instruction if registers[N] != XX
        //Where:
        //      N  == upper 4 bits of data
        //      XX == lower 8 bits of data
        public void SkipNotEqual(ushort data, byte XData)
        {
            ushort compareValue = (ushort)(data & 0x00FF);
            if (vRegisters[XData] != compareValue)
            {
                //skip next instruction (each instr is 2 bytes long)
                programCounter += 2;
            }
        }

        //Skips next instruction if data at register X == data at register Y
        public void SkipRegistersEqual(ushort data, byte XData, byte YData)
        {
            if (vRegisters[XData] == vRegisters[YData])
            {
                programCounter += 2;
            }
        }

        //Skips next instruction if data at register X != data at register Y
        public void SkipRegistersNotEqual(ushort data, byte XData, byte YData)
        {
            if (vRegisters[XData] != vRegisters[YData])
            {
                programCounter += 2;
            }
        }

        //Set regs[X] to NN 
        //  Where:
        //      X == upper 4 bits of data
        //      NN == lower 8 bits of data
        public void SetRegisterValue(ushort data, byte XData)
        {
            byte value = ((byte)(data & 0x00FF));
            vRegisters[XData] = value;
        }

        //sets the index register to the requested address
        public void SetIndexAddr(ushort addr)
        {
            index = addr;
        }

        //Add NN to regs[X]
        //  Where:
        //      X == upper 4 bits of data
        //      NN == lower 8 bits of data
        public void RegisterAddConst(ushort data, byte XData)
        {
            byte value = ((byte)(data & 0x00FF));
            vRegisters[XData] += value;
        }

        //Assign value in regs[Y] to regs[X]
        public void SetRegFromReg(ushort data, byte XData, byte YData)
        {
            vRegisters[XData] = vRegisters[YData];
        }

        //assign regs[X] | regs[Y] to regs[X]
        public void RegisterOR(ushort data, byte XData, byte YData)
        {
            vRegisters[XData] = (byte)(vRegisters[XData] | vRegisters[YData]);
        }

        //assign regs[X] & regs[Y] to regs[X]
        public void RegisterAND(ushort data, byte XData, byte YData)
        {
            vRegisters[XData] = (byte)(vRegisters[XData] & vRegisters[YData]);
        }

        //assign random number bitwise-ANDed with value to regs[X] 
        public void RegisterANDRandom(ushort data, byte XData)
        {
            byte value     = (byte) (data & 0x00FF);
            byte randValue = (byte)random.Next(255);

            vRegisters[XData] = (byte)(value & randValue);
        }

        //assign regs[X] ^ regs[Y] to regs[X]
        public void RegisterXOR(ushort data, byte XData, byte YData)
        {
            vRegisters[XData] = (byte)(vRegisters[XData] ^ vRegisters[YData]);
        }

        //assign regs[X] + regs[Y] to regs [X], set carry flag @ regs[16] appropriately
        public void RegisterAdd(ushort data, byte XData, byte YData)
        {
            short sum = (short)(vRegisters[XData] + vRegisters[YData]);

            //check for carry-out
            if (sum > 255)
            {
                vRegisters[15] = 1;
                sum -= 256;
            }
            else
            {
                vRegisters[15] = 0;
            }

            vRegisters[XData] = (byte)sum;
        }

        //assign regs[X] - regs[Y] to regs [X], set borrow flag @ regs[16] appropriately
        public void RegisterSub(ushort data, byte XData, byte YData)
        {
            //short must be signed to prevent underflow
            short diff = (short)(vRegisters[XData] - vRegisters[YData]);

            //check for borrow
            if (diff < 0)
            {
                vRegisters[15] = 0;
                diff += 256;
            }
            else
            {
                vRegisters[15] = 1;
            }

            vRegisters[XData] = (byte)diff;
        }

        //assign regs[Y] - regs[X] to regs[X], set borrow flag appropriately
        public void RegisterSub2(ushort data, byte XData, byte YData)
        {
            short diff = (short)(vRegisters[YData] - vRegisters[XData]);

            if (diff < 0)
            {
                vRegisters[15] = 0;
                diff += 256;
            }
            else
            {
                vRegisters[15] = 1;
            }

            vRegisters[XData] = (byte)diff;
        }

        //***FIX*** - some modern interpreters store X shift in X instead of Y - verify this later!!

        //shift contents of register X right by 1, store to reg X
        public void RegisterLSR(ushort data, byte XData)
        {
            vRegisters[15] = (byte)(vRegisters[XData] & 0x1); //store LSB of reg X prior to shift

            //shift contents of X right by 1 and store result to reg X (reg X is ignored)
            vRegisters[XData] = (byte)(vRegisters[XData] >> 1);
        }

        public void RegisterLSL(ushort data, byte XData)
        {
            vRegisters[15] = (byte)(vRegisters[XData] >> 7); //store MSB of register X prior to shift

            //shift contents of X left by 1 and store result to reg X (reg X is ignored)
            vRegisters[XData] = (byte)(vRegisters[XData] << 1);
        }

        //store the value of the delay timer to the requested register
        public void SetRegToDelayTimer(ushort data, byte XData)
        {
            vRegisters[XData] = (byte)delayTimer;
        }

        //add the value in the requested register to the index register
        public void AddToIndex(ushort data, byte XData)
        {
            index += vRegisters[XData];
        }

        //set the index register's value to the value in the requested register
        public void SetIndex(ushort data, byte XData)
        {
            index = (ushort)(vRegisters[XData] * 0x5);
        }

        //set the delay timer's value to the value in the requested register
        public void SetDelayTimer(ushort data, byte XData)
        {
            delayTimer = vRegisters[XData];
        }

        //set the sound timer's value to the value in the requested register
        public void SetSoundTimer(ushort data, byte XData)
        {
            soundTimer = vRegisters[XData];
        }

        //store BCD representation of value at requested register to memory starting at index address
        public void StoreBCD(ushort data, byte XData)
        {
            byte regXValue = vRegisters[XData];

            for (int i = 3; i > 0; i--)
            {
                memory[index + i - 1] = (byte)(regXValue % 10);
                regXValue /= 10;
            }
        }

        //store registers from index 0 to requested index in memory starting at index address
        public void StoreRegisters(ushort data, byte XData)
        {
            for (int i = 0; i <= XData; i++)
            {
                memory[index + i] = vRegisters[i];
            }

            //index += (ushort)(regXIndex + 1);
        }

        //load from memory into registers in range [index, index + requested value]
        public void LoadIntoRegisters(ushort data, byte XData)
        {
            for (int i = 0; i <= XData; i++)
            {
                vRegisters[i] = memory[index + i];
            }
        }

        //Draws a sprite of with 8 and variable height at coordinates (reg[X], reg[Y])
        //  Each 8-pixel row is bit-coded starting from memory[index]
        //  regs[15] is set if any pixels flip from set to unset when sprite is drawn
        public void DrawSprite(ushort data, byte XData, byte YData)
        {
            byte xCoord = (byte)(vRegisters[XData] % 64);
            byte yCoord = (byte)(vRegisters[YData] % 32);
            ushort height = (ushort)(data & 0x000F);
            byte pixel;

            vRegisters[15] = 0;
            for (int y = 0; y < height; y++)
            {
                pixel = memory[index + y];

                for (int x = 0; x < 8; x++)
                {
                    if ((pixel & (0x80 >> x)) != 0)
                    {
                        ushort position = (ushort)((xCoord + x + ((yCoord + y) * 64)) % 2048);
                        vRegisters[15] |= (frame[position] == 1) ? (byte)1 : (byte)0;
                        frame[position] ^= 1;
                    }
                }
            }

            drawFlag = true;
        }

        //skip the next instruction if the key[regs[X]] is pressed
        public void SkipOnKeyPress(ushort data, byte XData)
        {
            byte keyIndex = vRegisters[XData];
            if (keyStates[keyIndex] != 0)
            {
                programCounter += 2;
            }
        }

        //skip the next instruction if the key[regs[X]] is not pressed
        public void SkipKeyNotPressed(ushort data, byte XData)
        {
            byte keyIndex = vRegisters[XData];

            if (keyStates[keyIndex] == 0)
            {
                programCounter += 2;
            }
        }

        //suspend execution until any key is pressed 
        public void WaitForKey(ushort data, byte XData)
        {
            bool keyPressed = false;

            //poll key state registers 
            for (int i = 0; i < 16; i++)
            {
                if (keyStates[i] != 0)
                {
                    vRegisters[XData] = (byte)i;
                    keyPressed = true;
                }
            }

            if (!keyPressed)
            {
                //suspend by rolling back the program counter on each cycle
                programCounter -= 2;
                return;
            }
        }

        public string Disassemble(ushort currOpCode)
        {
            string instructionStr = String.Empty;
            byte x     = (byte)  ((currOpCode & 0x0F00) >> 8);
            byte y     = (byte)  ((currOpCode & 0x00F0) >> 4);
            ushort nnn = (ushort) (currOpCode & 0x0FFF);
            byte kk    = (byte)   (currOpCode & 0x00FF);
            byte n     = (byte)   (currOpCode & 0x000F);

            switch (currOpCode & 0xF000)
            {
                case 0x0000:
                    switch (currOpCode & 0x000F)
                    {
                        case 0x0000:
                            instructionStr = "CLS";
                            break;

                        case 0x000E:
                            instructionStr = "RET";
                            break;
                    }
                    break;

                case 0x1000:
                    instructionStr = String.Format("JP {0:X3}", nnn);
                    break;

                case 0x2000:
                    instructionStr = String.Format("CALL {0:X3}", nnn);
                    break;

                case 0x3000:
                    instructionStr = String.Format("SE V[{0:X1}], {1:X2}", x, kk);
                    break;

                case 0x4000:
                    instructionStr = String.Format("SNE V[{0:X1}], {1:X2}", x, kk);
                    break;

                case 0x5000:
                    instructionStr = String.Format("SE V[{0:X1}], V[{1:X1}]", x, y);
                    break;

                case 0x6000:
                    instructionStr = String.Format("LD V[{0:X1}], {1:X2}", x, kk);
                    break;

                case 0x7000:
                    instructionStr = String.Format("ADD V[{0:X1}], {1:X2}", x, kk);
                    break;

                case 0x8000:
                    //instructionStr = String.Format("LD V[{0:X1}], V[{1:X1}]", x, y);
                    switch (currOpCode & 0x000F)
                    {
                        case 0x0000:
                            instructionStr = String.Format("LD V[{0:X1}], V[{1:X1}]", x, y);
                            break;
                            
                        case 0x0001:
                            instructionStr = String.Format("OR V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x0002:
                            instructionStr = String.Format("AND V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x0003:
                            instructionStr = String.Format("XOR V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x0004:
                            instructionStr = String.Format("ADD V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x0005:
                            instructionStr = String.Format("SUB V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x0006:
                            instructionStr = String.Format("SHR V[{0:X1}]", x);
                            break;

                        case 0x0007:
                            instructionStr = String.Format("SUBN V[{0:X1}], V[{1:X1}]", x, y);
                            break;

                        case 0x000E:
                            instructionStr = String.Format("SHL V[{0:X1}]", x);
                            break;
                    }
                    break;

                case 0x9000:
                    instructionStr = String.Format("SNE V[{0:X1}], V[{1:X1}]", x, y);
                    break;

                case 0xA000:
                    instructionStr = String.Format("LD I, {0:X3}", nnn);
                    break;

                case 0xB000:
                    instructionStr = String.Format("JP V[0], {0:X3}", nnn);
                    break;

                case 0xC000:
                    instructionStr = String.Format("RND V[{0:X1}], {1:X2}", x, kk);
                    break;

                case 0xD000:
                    instructionStr = String.Format("DRW V[{0:X1}], V[{1:X1}], {2:X1}", x, y, n);
                    break;

                case 0xE000:
                    //instructionStr = String.Format("SKP V[{0:X1}]", x);
                    switch (currOpCode & 0x00FF)
                    {
                        case 0x009E:
                            instructionStr = String.Format("SKP V[{0:X1}]", x);
                            break;

                        case 0x00A1:
                            instructionStr = String.Format("SKNP V[{0:X1}]", x);
                            break;
                    }
                    break;

                case 0xF000:
                    switch (currOpCode & 0x00FF)
                    {
                        case 0x0007:
                            instructionStr = String.Format("LD V[{0:X1}], DT", x);
                            break;

                        case 0x000A:
                            instructionStr = String.Format("LD V[{0:X1}], K", x);
                            break;

                        case 0x0015:
                            instructionStr = String.Format("LD DT, V[{0:X1}]", x);
                            break;

                        case 0x0018:
                            instructionStr = String.Format("LD ST, V[{0:X1}]", x);
                            break;

                        case 0x001E:
                            instructionStr = String.Format("ADD I, V[{0:X1}]", x);
                            break;

                        case 0x0029:
                            instructionStr = String.Format("LD F, V[{0:X1}]", x);
                            break;

                        case 0x0033:
                            instructionStr = String.Format("LD B, V[{0:X1}]", x);
                            break;

                        case 0x0055:
                            instructionStr = String.Format("LD MEM[I], V[{0:X1}]", x);
                            break;

                        case 0x0065:
                            instructionStr = String.Format("LD V[{0:X1}], MEM[I]", x);
                            break;
                    }
                    break;
            }
            return instructionStr;
        }

        public void EmulateCycle()
        {
            PollKeys();
            //fetch the current opcode indicated by the PC (opcodes are 2 bytes long so we must fetch 2 values)
            opcode = (ushort)((memory[programCounter] << 8) | memory[programCounter + 1]);
            //Console.WriteLine("opcode = {0:X4}", opcode);
            ushort instrData = (ushort)(opcode & 0x0FFF);
            byte XData = (byte)((instrData & 0x0F00) >> 8);
            byte YData = (byte)((instrData & 0x00F0) >> 4);

            //prepare to read next instruction
            programCounter += 2;

            //decode/execute the current opcode 
            switch (opcode & 0xF000) //upper 4 bits determine type of operation
            {
                //OPCODE RANGE 0x0___
                case 0x0000:

                    //OPCODE RANGE 0x___0
                    switch (opcode & 0x000F)
                    {
                        case 0x0000: //0x00E0 - clears the screen
                            clearScreen();
                            break;

                        case 0x000E: //0x00EE - return from subroutine
                            Return();
                            break;

                        default:
                            Console.WriteLine("ERROR: unknown opcode: {0:X4}", opcode);
                            break;
                    }
                    break;

                //OPCODE RANGE 0x1___
                case 0x1000: //0x1NNN - jump to address NNN
                    JumpToAddr(instrData);
                    break;

                //OPCODE RANGE 0x2___
                case 0x2000: // 0x2NNN - call subroutine at address NNN
                    CallSubroutine(instrData);
                    break;

                //OPCODE RANGE 0x3___
                case 0x3000: //0x3XNN - skip next instr if VX == NN
                    SkipEqual(instrData, XData);
                    break;

                //OPCODE RANGE 0x4___
                case 0x4000: //0x4XNN - skip next instr if VX != NN
                    SkipNotEqual(instrData, XData);
                    break;

                //OPCODE RANGE 0x5___
                case 0x5000: //0x5XY0 - skip next instr if regs[X] == regs[y]
                    SkipRegistersEqual(instrData, XData, YData);
                    break;

                //OPCODE RANGE 0x6___
                case 0x6000: //0x6XNN - sets regs[X] to NN
                    SetRegisterValue(instrData, XData);
                    break;

                //OPCODE RANGE 0x7___
                case 0x7000:
                    RegisterAddConst(instrData, XData);
                    break;

                //OPCODE RANGE 0x8___
                case 0x8000:
                    switch (opcode & 0x000F)
                    {
                        case 0x0000: //0x8XY0 - regs[X] = regs[Y]
                            SetRegFromReg(instrData, XData, YData);
                            break;

                        case 0x0001: //0x8XY1 - regs[X] = regs[X] | regs[Y]
                            RegisterOR(instrData, XData, YData);
                            break;

                        case 0x0002: //0x8XY2 - regs[X] = regs[X] | regs[Y]
                            RegisterAND(instrData, XData, YData);
                            break;

                        case 0x0003: //0x8XY3 - regs[X] = regs[X] ^ regs[Y]
                            RegisterXOR(instrData, XData, YData);
                            break;

                        case 0x0004: //0x8XY4 - regs[X] = regs[X] + regs[Y], set VF (carry flag) appropriately
                            RegisterAdd(instrData, XData, YData);
                            break;

                        case 0x0005: //0x8XY5 - regs[X] = regs[X] + regs[Y], set VF (borrow flag) appropriately
                            RegisterSub(instrData, XData, YData);
                            break;

                        case 0x0006: //0x8XY6 - regs[X] = regs[X] >> 1, set VF to LSB before shift
                            RegisterLSR(instrData, XData);
                            break;

                        case 0x0007: //0x8XY7 - regs[X] = regs[Y] - regs[X], set VF (borrow flag) appropriately
                            RegisterSub2(instrData, XData, YData);
                            break;

                        case 0x000E: //0x8XYE - regs[X] = regs[X] << 1, set VF to MSB before shift
                            RegisterLSL(instrData, XData);
                            break;
                        default:
                            Console.WriteLine("ERROR: unknown opcode: {0:X4}", opcode);
                            break;
                    }
                    break;

                //OPCODE RANGE 0x9___
                case 0x9000:
                    SkipRegistersNotEqual(instrData, XData, YData);
                    break;

                //OPCODE RANGE 0xA___
                case 0xA000: // 0xANNN - sets index to address NNN
                    SetIndexAddr(instrData);
                    break;

                //OPCODE RANGE 0xB___
                case 0xB000: // 0xBNNN - jumps to address NNN + regs[0]
                    JumpToRegOffsetAddr(instrData);
                    break;

                //OPCODE RANGE 0xC___
                case 0xC000:
                    RegisterANDRandom(instrData, XData);
                    break;

                //OPCODE RANGE 0xD___
                case 0xD000:
                    DrawSprite(instrData, XData, YData);
                    break;

                //OPCODE RANGE 0xE___
                case 0xE000:
                    switch (opcode & 0xFF)
                    {
                        case 0x009E: //0xEX9E - skip next instruction if key in regs[X] is pressed
                            SkipOnKeyPress(instrData, XData);
                            break;

                        case 0xA1:
                            SkipKeyNotPressed(instrData, XData);
                            break;

                        default:
                            Console.WriteLine("ERROR: unknown opcode: {0:X4}", opcode);
                            break;
                    }
                    break;

                //OPCODE RANGE 0xF___
                case 0xF000:
                    switch (opcode & 0xFF)
                    {
                        case 0x0007: //0xFX07 - regs[X] = delayTimer
                            SetRegToDelayTimer(instrData, XData);
                            break;

                        case 0x000A: //0xFX0A - wait for key press, store key in regs[X] (blocking)
                            WaitForKey(instrData, XData);
                            break;

                        case 0x0015: //0xFX15 - set delay timer to value in regs[X]
                            SetDelayTimer(instrData, XData);
                            break;

                        case 0x0018: //0xFX18 - set the sound timer to value in regs[X]
                            SetSoundTimer(instrData, XData);
                            break;

                        case 0x001E: //0xFX1E - index += regs[X]
                            AddToIndex(instrData, XData);
                            break;

                        case 0x0029: //0xFX29 - set index to memory address of sprite data in regs[X]
                            SetIndex(instrData, XData);
                            break;

                        case 0x0033: //0xFX33 - stores BCD representation of regs[X] starting ad address index
                            StoreBCD(instrData, XData);
                            break;

                        case 0x0055: //0xFX55 - store values from regs[0] to regs[X] in memory starting at index
                            StoreRegisters(instrData, XData);
                            break;

                        case 0x0065: //0xFX65 - read from memory into regs[0] to regs[X] starting at index
                            LoadIntoRegisters(instrData, XData);
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("ERROR: unknown opcode: {0:X4}", opcode);
                    break;
            }

            //make sure timers decrement at 60Hz regardless of clock speed
            if (++cycleCount % timerInterval == 0)
            {   
                TickDelayTimer();
                TickSoundTimer();
            }
        }

        public void TickDelayTimer()
        {
            if (delayTimer > 0)
            {
                delayTimer--;
            }
        }

        public void TickSoundTimer()
        {
            if (soundTimer > 0)
            {
                soundTimer--;
            }
        }
    }
}
