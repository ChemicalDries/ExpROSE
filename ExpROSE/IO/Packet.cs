using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpROSE.IO
{
    public class Packet
    {
        public ushort Size;
        public ushort Command;
        public ushort Unused;
        public byte[] PacketBuffer = new byte[4096];
        GCHandle _gch;

        public Packet(ushort mycommand, ushort mySize, ushort myunused)// No need "StartPacket" or "ResetPacket" it can be do by "new Packet();"
        {
            Command = mycommand;
            Unused = myunused;
            Size = mySize;
        }

        public Packet(ushort mycommand)
        {
            Command = mycommand;
            Unused = 0;
            Size = 6;
        }

        public Packet()
        {
            Command = 0;
            Unused = 0;
            Size = 6;
        }

        public void EndPacket()
        {
            Handle();
            SetWord(0, Size);
            SetWord(2, Command);
            SetWord(4, Unused);
        }

        private void Dipose()
        {
            _gch.Free();
        }

        public void Handle()
        {
            _gch = GCHandle.Alloc(PacketBuffer, GCHandleType.Pinned);
        }

        public void AddByte(byte value)
        {
            PacketBuffer[Size] = value;
            Size += 1;
        }

        public void AddBytes(byte[] value)
        {
            for (int i = 0; i < value.Length; i++) AddByte(value[i]);
        }

        public void AddWord(ushort value)
        {
            Handle();
            Marshal.WriteInt16(_gch.AddrOfPinnedObject(), Size, unchecked((short)value));
            Size += 2;
        }

        public void AddDword(uint value)
        {
            Handle();
            Marshal.WriteInt32(_gch.AddrOfPinnedObject(), Size, unchecked((int)value));
            Size += 4;
        }

        public void AddQword(ulong value)
        {
            Handle();
            Marshal.WriteInt64(_gch.AddrOfPinnedObject(), Size, unchecked((long)value));
            Size += 8;
        }

        public unsafe void AddFloat(float value)
        {
            fixed (byte* pbuffer = PacketBuffer)
            {
                *((float*)&pbuffer[Size]) = value;
                Size += 4;
            }
        }

        public void AddString(string value)
        {
            byte[] charvalue = System.Text.Encoding.Default.GetBytes(value);
            for (int i = 0; i < charvalue.Length; i++) AddByte(charvalue[i]);
        }

        public void SetByte(ushort position, byte value)
        {
            PacketBuffer[position] = value;
        }

        public void SetWord(ushort position, ushort value)
        {
            Handle();
            Marshal.WriteInt16(_gch.AddrOfPinnedObject(), position, unchecked((short)value));
        }

        public void SetDword(ushort position, uint value)
        {
            Handle();
            Marshal.WriteInt32(_gch.AddrOfPinnedObject(), position, unchecked((int)value));
        }

        public void SetQword(ushort position, ulong value)
        {
            Handle();
            Marshal.WriteInt64(_gch.AddrOfPinnedObject(), position, unchecked((long)value));
        }

        public unsafe void SetFloat(short position, float value)
        {
            fixed (byte* pbuffer = PacketBuffer)
            {
                *((float*)&pbuffer[position]) = value;
            }
        }

        public byte GetByte(short position)
        {
            return PacketBuffer[position + 6];
        }

        public ushort GetWord(short position)
        {
            Handle();
            return (ushort)Marshal.ReadInt16(_gch.AddrOfPinnedObject(), position + 6);
        }

        public uint GetDword(short position)
        {
            Handle();
            return (uint)Marshal.ReadInt32(_gch.AddrOfPinnedObject(), position + 6);
        }

        public ulong GetQword(short position)
        {
            Handle();
            return (ulong)Marshal.ReadInt64(_gch.AddrOfPinnedObject(), position + 6);
        }

        public unsafe float GetFloat(short position)
        {
            fixed (byte* pbuffer = PacketBuffer)
            {
                return *((float*)&pbuffer[position + 6]);
            }
        }

        public string GetString(short position, bool nullable)
        {
            char[] charstring = new char[100];
            int i = 0;
            while (charstring[i] != 0)
            {
                charstring[i] = Convert.ToChar(PacketBuffer[position + 6 + i]);
                i++;
            }
            return charstring.ToString();
        }

        public string GetString(short position, ushort lenght)
        {
            byte[] charstring = new byte[lenght];
            for (int i = 0; i < lenght; i++)
            {
                charstring[i] = Convert.ToByte(PacketBuffer[position + 6 + i]);
            }
            return CleanString(Encoding.ASCII.GetString(charstring));
        }

        static public string CleanString(string s)
        {
            if (s != null && s.Length > 0)
            {
                StringBuilder sb = new StringBuilder(s.Length);
                foreach (char c in s)
                {
                    sb.Append(Char.IsControl(c) ? ' ' : c);
                }

                s = sb.ToString();
            }
            return s;
        }
    }
}
