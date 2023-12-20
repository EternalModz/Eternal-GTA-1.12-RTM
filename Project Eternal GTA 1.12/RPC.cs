using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Eternal_GTA_1._12
{
    class RPC
    {
        public static int CallOther(uint func_address, params object[] parameters)
        {
            int length = parameters.Length;
            int index = 0;
            uint num3 = 0;
            uint num4 = 0;
            uint num5 = 0;
            uint num6 = 0;
            while (index < length)
            {
                if (parameters[index] is int)
                {
                    MainForm.PS3.Extension.WriteInt32(0x10020000 + (num3 * 4), (int)parameters[index]);
                    num3++;
                }
                else if (parameters[index] is uint)
                {
                    MainForm.PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), (uint)parameters[index]);
                    num3++;
                }
                else
                {
                    uint num7;
                    if (parameters[index] is string)
                    {
                        num7 = 0x10022000 + (num4 * 0x400);
                        MainForm.PS3.Extension.WriteString(num7, Convert.ToString(parameters[index]));
                        MainForm.PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), num7);
                        num3++;
                        num4++;
                    }
                    else if (parameters[index] is float)
                    {
                        WriteSingle(0x10020024 + (num5 * 4), (float)parameters[index]);
                        num5++;
                    }
                    else if (parameters[index] is float[])
                    {
                        float[] input = (float[])parameters[index];
                        num7 = 0x10021000 + (num6 * 4);
                        WriteSingle(num7, input);
                        MainForm.PS3.Extension.WriteUInt32(0x10020000 + (num3 * 4), num7);
                        num3++;
                        num6 += (uint)input.Length;
                    }
                }
                index++;
            }
            MainForm.PS3.Extension.WriteUInt32(0x1002004c, func_address);
            Thread.Sleep(50);
            return MainForm.PS3.Extension.ReadInt32(0x10020050);
        } //Try if don't work

        public static int Call(uint func_address, params object[] parameters)
        {
            uint address = func_address;
            int length = parameters.Length;
            int index = 0;
            uint num3 = 0;
            uint num4 = 0;
            uint num5 = 0;
            uint num6 = 0;
            while (index < length)
            {
                if (parameters[index] is int)
                {
                    MainForm.PS3.Extension.WriteInt32(0x10040000 + (num3 * 4), (int)parameters[index]);
                    num3++;
                }
                else if (parameters[index] is uint)
                {
                    MainForm.PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), (uint)parameters[index]);
                    num3++;
                }
                else
                {
                    uint num7;
                    if (parameters[index] is string)
                    {
                        num7 = 0x10042000 + (num4 * 0x400);
                        MainForm.PS3.Extension.WriteString(num7, Convert.ToString(parameters[index]));
                        MainForm.PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                        num3++;
                        num4++;
                    }
                    else if (parameters[index] is float)
                    {
                        WriteSingle(0x10040024 + (num5 * 4), (float)parameters[index]);
                        num5++;
                    }
                    else if (parameters[index] is float[])
                    {
                        float[] input = (float[])parameters[index];
                        num7 = 0x10041000 + (num6 * 4);
                        WriteSingle(num7, input);
                        MainForm.PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                        num3++;
                        num6 += (uint)input.Length;
                    }
                }
                index++;
            }
            MainForm.PS3.Extension.WriteUInt32(0x1004004C, address);
            while (MainForm.PS3.Extension.ReadUInt32(0x1004004C) != 0) ;

            return MainForm.PS3.Extension.ReadInt32(0x10040050);
        }

        public static uint CBAB(uint F, uint T) //CBAB Fix Addrse enablimg of empty address
        {
            if (F > T)
                return 0x4C000000 - (F - T);
            else if (F < T)
                return T - F + 0x48000000;
            else
                return 0x48000000;
        }

        public static uint SFA1 = 0x1B2CE80; //old address: 0x1bf5000 / 1.27: 0x1BE4C80
        //public static uint SFA2 = 0x; //old address:
        //public static uint SFA3 = 0x; //old address:
        public static uint EFA1 = 0x1B2CF08; //old address: 0x1bf5088 / 1.27: 0x1BE4D08
        //public static uint EFA2 = 0x; //old address:
        //public static uint EFA3 = 0x1//old address:
        public static uint BFA1 = 0x18324; //old address: 0x18614 
        //public static uint BFA2 = 0x; //old address: 
        public static uint BAB1 = 0x18330; //old address: 0x18620

        public static void Enable()
        {
            byte[] mem = new byte[] {
            0xF8, 0x21, 0xFF, 0x91,
            0x7C, 0x08, 0x02, 0xA6,
            0xF8, 0x01, 0x00, 0x80,
            0x3C, 0x60, 0x10, 0x04,
            0x81, 0x83, 0x00, 0x4C,
            0x2C, 0x0C, 0x00, 0x00,
            0x41, 0x82, 0x00, 0x64,
            0x80, 0x83, 0x00, 0x04,
            0x80, 0xA3, 0x00, 0x08,
            0x80, 0xC3, 0x00, 0x0C,
            0x80, 0xE3, 0x00, 0x10,
            0x81, 0x03, 0x00, 0x14,
            0x81, 0x23, 0x00, 0x18,
            0x81, 0x43, 0x00, 0x1C,
            0x81, 0x63, 0x00, 0x20,
            0xC0, 0x23, 0x00, 0x24,
            0xc0, 0x43, 0x00, 0x28,
            0xC0, 0x63, 0x00, 0x2C,
            0xC0, 0x83, 0x00, 0x30,
            0xC0, 0xA3, 0x00, 0x34,
            0xc0, 0xC3, 0x00, 0x38,
            0xC0, 0xE3, 0x00, 0x3C,
            0xC1, 0x03, 0x00, 0x40,
            0xC1, 0x23, 0x00, 0x48,
            0x80, 0x63, 0x00, 0x00,
            0x7D, 0x89, 0x03, 0xA6,
            0x4E, 0x80, 0x04, 0x21,
            0x3C, 0x80, 0x10, 0x04,
            0x38, 0xA0, 0x00, 0x00,
            0x90, 0xA4, 0x00, 0x4C,
            0x90, 0x64, 0x00, 0x50,
            0xE8, 0x01, 0x00, 0x80,
            0x7C, 0x08, 0x03, 0xA6,
            0x38, 0x21, 0x00, 0x70 };

            MainForm.PS3.SetMemory(SFA1, mem);
            MainForm.PS3.Extension.WriteUInt32(EFA1, CBAB(EFA1, BAB1));
            MainForm.PS3.Extension.WriteUInt32(BFA1, CBAB(BFA1, SFA1));
            MainForm.PS3.Extension.WriteString(0x10058000, "T"); //Pera
            //Full:
            //RPC.PS3.SetMemory(RPC.SFA1, buffer);
            //RPC.PS3.SetMemory(RPC.SFA2, buffer);
            //RPC.PS3.SetMemory(RPC.SFA3, buffer);
            //RPC.PS3.Extension.WriteUInt32(RPC.EFA1, RPC.CBAB(RPC.EFA1, RPC.BAB1));
            //RPC.PS3.Extension.WriteUInt32(RPC.BFA1, RPC.CBAB(RPC.BFA1, RPC.SFA1));
            //RPC.PS3.Extension.WriteUInt32(RPC.EFA2, RPC.CBAB(RPC.EFA2, RPC.BAB2));
            //RPC.PS3.Extension.WriteUInt32(RPC.BFA2, RPC.CBAB(RPC.BFA2, RPC.SFA2));
            //RPC.PS3.Extension.WriteUInt32(RPC.EFA3, RPC.CBAB(RPC.EFA3, RPC.BAB3));
            //RPC.PS3.Extension.WriteUInt32(RPC.BFA3, RPC.CBAB(RPC.BFA3, RPC.SFA3));
        }

        private static byte[] ReverseBytes(byte[] toReverse)
        {
            Array.Reverse((Array)toReverse);
            return toReverse;
        }

        private static void WriteSingle(uint address, float input)
        {
            byte[] Bytes = new byte[4];
            BitConverter.GetBytes(input).CopyTo((Array)Bytes, 0);
            Array.Reverse((Array)Bytes, 0, 4);
            MainForm.PS3.SetMemory(address, Bytes);
        }

        private static void WriteSingle(uint address, float[] input)
        {
            int length = input.Length;
            byte[] Bytes = new byte[length * 4];
            for (int index = 0; index < length; ++index)
                ReverseBytes(BitConverter.GetBytes(input[index])).CopyTo((Array)Bytes, index * 4);
            MainForm.PS3.SetMemory(address, Bytes);
        }

        public class Address //Founded by: Pantera
        {
            //Others:
            public static uint entityXCoord = 0x10030000;
            public static uint entityYCoord = 0x10030004;
            public static uint entityZCoord = 0x10030008;
            //Natives Hash to Address:
            public static uint PLAYER_ID = 0x40EA58;
            public static uint PLAYER_PED_ID = 0x40EA9C;
            public static uint GET_ENTITY_COORDS = 0x38D4F8;
            public static uint SET_PLAYER_INVINCIBLE = 0x40F160;
            public static uint SET_ENTITY_VISIBLE = 0x395868; //127: 3B4280
            public static uint SET_POLICE_IGNORE_PLAYER = 0x40D474; //127: 422B10
            public static uint CLEAR_PLAYER_WANTED_LEVEL = 0x40CCF4; //127: 0x422390
            public static uint GET_PLAYER_NAME = 0x40C5C8;
            public static uint CLEAR_AREA_OF_VEHICLES = 0x3B4CC8;
            public static uint CLEAR_AREA_OF_OBJECTS = 0x3B4F90;
            public static uint SET_STAT_INT = 0x4155C8; //1.27: 0x42BE0C
            public static uint STAT_SET_BOOL = 0x415A60; //1.27: 42C2A4
            public static uint NETWORK_EARN_FROM_ROCKSTAR = 0x3BC680; //1.27: 0x3DD408
            public static uint STAT_GET_INT = 0x4164DC;
            public static uint SET_GAME_PAUSES_FOR_STREAMING =  0x41DA28; //1.27: 0x4365C8
            public static uint SET_STREAMING = 0x41D9A0; //1.27: 0x436540
            public static uint SET_PED_INFINITE_AMMO_CLIP = 0x4368A4; //1.27: 
            public static uint SET_SUPER_JUMP_THIS_FRAME = 0x3B8928; //1.27: 0x3D97F0
            public static uint GIVE_DELAYED_WEAPON_TO_PED = 0x4364B8; //1.27: 0x466BF8
            public static uint SET_PLAYER_WEAPON_DAMAGE_MODIFIER = 0x410618; //1.27: 0x425E38
            public static uint NETWORK_SESSION_KICK_PLAYER = 0x3C212C;//1.27: 0x3E41B0
            public static uint GIVE_WEAPON_COMPONENT_TO_PED = 0x438C90; //1.27: 0x469500
            public static uint SET_PED_WEAPON_TINT_INDEX = 0x4398AC; //1.27: 0x46A128
        }
    }
}
   
