using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Eternal_GTA_1._12
{
    class Functions
    {
        static byte[] on = { 0x4E, 0x80, 0x00, 0x20 };
        static byte[] off = { 0x7C, 0x08, 0x02, 0xA6 };
        static byte[] nop = { 0x60, 0x00, 0x00, 0x00 };
        public static void STAT_SET_INT(uint statHash, int value)
        {
            RPC.Call(RPC.Address.SET_STAT_INT, statHash, value, 1);
        }
        public static void STAT_SET_BOOL(uint statHash, int value)
        {
            RPC.Call(RPC.Address.STAT_SET_BOOL, statHash, value, 1);
        }
        public static void BypassPrologue(bool enabled)
        {
            //Original Cod:
            //*(char*)0x01389F63 = 0;
            //*(char*)0x0008797F = 0;
            if (enabled)
            {
                MainForm.PS3.SetMemory(0x130422B, new byte[] { 0x01 });
                MainForm.PS3.SetMemory(0x826B7, new byte[] { 0x01 });
            }
            else
            {
                MainForm.PS3.SetMemory(0x130422B, new byte[] { 0x00 });
                MainForm.PS3.SetMemory(0x826B7, new byte[] { 0x00 });
            }
        }

        //Real Name Host: 0x40054CF0
        // Get Host Name 1.12: 0x1c3324c ( maybe: 0x1C33254)  + 0x10(check the args) | Real Name: 0x1F379EC | PSN Name: 0x1F379C0 | 
        public static void FreezeLobbyV1(bool enable)
        {
            if (enable)
            {
                MainForm.PS3.SetMemory(0x995070, new byte[] { 0x4E, 0x80, 0x00, 0x20, 0x4E, 0x80, 0x00, 0x20 });
                MainForm.PS3.SetMemory(0x9B2A18, on);
            }
            else
            {
                MainForm.PS3.SetMemory(0x995070, new byte[] { 0x7C, 0x08, 0x02, 0xA6, 0x48, 0xAD, 0x36, 0x21 });
                MainForm.PS3.SetMemory(0x9B2A18, new byte[] { 0x4B, 0xFE, 0x26, 0x59 });
            }
        }
        public static void FreezeLobbyV2_Gun(bool enable) //by xibryan e xPantera
        {
            if (enable)
            {
                //PS3.SetMemory(0xA49F8C, ONN); // F8 21 FF 91  
                //PS3.SetMemory(0xA49F90, ONN); //7C 08 02 A6
                //PS3.SetMemory(0x12D5F50, ONN); //Off: 7C0802A6
                MainForm.PS3.SetMemory(0xA49EEC, on); //Sync Crash new
                MainForm.PS3.SetMemory(0x99531C, nop); // : 48 0B 4B D1 //xPantera  
                MainForm.PS3.SetMemory(0x12D6278, on); //xBryan
                
            }
            else
            {
                MainForm.PS3.SetMemory(0x99531C, new byte[] { 0x48, 0x0B, 0x4B, 0xD1 }); //xPantera  
                MainForm.PS3.SetMemory(0x12D6278, off); //xBryan
                MainForm.PS3.SetMemory(0xA49EEC, off);
            }
        }
        public static void FreezeLobbyV4_Ghost(bool enable) //sub_A49EEC by xibryan
        {
            if (enable)
            {
                MainForm.PS3.SetMemory(0xA49858, on); //V4
                MainForm.PS3.SetMemory(0xA497A8, on); //Ghost Freeze (V5)
            }
            else
            {
                MainForm.PS3.SetMemory(0xA49858, off); //V4
                MainForm.PS3.SetMemory(0xA497A8, off); //Ghost Freeze (V5)
            }
        }

        public static void freezeLobbyCorrupted1(bool enable) //V6 by xibryan
        {
            if (enable)
            {
                MainForm.PS3.SetMemory(0x9DF478, new byte[] { 0x10 });
                MainForm.PS3.SetMemory(0x9Df47C, new byte[] { 0x10 });
            }
            else
            {
                MainForm.PS3.SetMemory(0x9DF478, new byte[] { 0x80 });
                MainForm.PS3.SetMemory(0x9Df47C, new byte[] { 0x80 });
            }
        }
        public static void freezeLobbyCorrupted2(bool enable) //V7 by xibryan
        {
            if (enable)
            {
                MainForm.PS3.SetMemory(0xA49880, new byte[] { 0x10 });
            }
            else
            {
                MainForm.PS3.SetMemory(0xA49880, new byte[] { 0x80 });
            }
        }
        public static void FreezeRPC_and_BlackScreen() //by gopro
        {
            RPC.Call(0x12449EC, 1, -1, MainForm.PS3.Extension.ReadInt32(0x207D654 + 0xD04), 0); //BlackScreen
            RPC.Call(0x12449EC, 1, 0xE, MainForm.PS3.Extension.ReadInt32(0x207D654 + 0xD04), 0); //Freeze       
        }

        public static void AntiFreezeV1()
        {
            //PS3.SetMemory(0x9974C0, NOP); //Anti from Random Player
            MainForm.PS3.SetMemory(0x995820, nop); //Protection Freeze Lobby V1 1 (session)
            MainForm.PS3.SetMemory(0x995824, nop); //Protection Freeze Lobby V1 2(session)
            //the #3 does not exist on 1.12
            MainForm.PS3.SetMemory(0x992138, on); //Disable Freeze Paradise Lobby All V3
            //PS3.SetMemory(0x9A3168, ONN); //Anti Crown Freeze
            //PS3.SetMemory(0x99770C, ONN); //Anti Crown Freeze crash here
            //PS3.SetMemory(0x9E42AC, ONN); //UnknowFreezeProtection #2 não testado
            //PS3.SetMemory(0x9E4188, ONN); //UnknowFreezeProtection #3 é um desses mas eu apertei no bang errado já daskjda tmnc não testado
            //Need use with NETWORK_IS_GAME_AND_PLAYER_READY:
            //PS3.SetMemory(0x1284F4C, NOP); // Anti Crown #1 (founded 13/05) 4B FB A4 71
            //PS3.SetMemory(0x1287B90, ONN); // UnknowFreezeProtection #1 (founded 13/05) 7C 08 02 A6
            //1.12: 0x9ABE2C 0x48000310 :  3C 80 01 84 //Risk AutoFreeze NOT TESTED!!!!!!
        }
        public static void AntiFreezeV2(bool Enable)
        {
            if (Enable)
            {
                MainForm.PS3.SetMemory(0x9B03A8, new byte[] { 0x60, 0x00, 0x00, 0x00 });
            }
            else
            {
                MainForm.PS3.SetMemory(0x9B03A8, new byte[] { 0x80, 0x7C, 0x00, 0x08 });
            }
        }
        public static void AntiFreezeFromArabic()
        {
            MainForm.PS3.SetMemory(0x9B03A8, nop); //Anti From xl_private off: 80 7C 00 08
            MainForm.PS3.SetMemory(0xE8C594, on); //Maybe anti from zHJDH
            MainForm.PS3.SetMemory(0x9A87FC, nop); //Anti From T3qiz #1
        }
        public static void AntiSessionByWe()
        {
            MainForm.PS3.SetMemory(0x9B2A14, nop); //TEST
            MainForm.PS3.SetMemory(0x995820, nop); //Dbg Alert 1
            MainForm.PS3.SetMemory(0x995818, nop); //Dbg Alert 2
            MainForm.PS3.SetMemory(0x996894, nop); //Dbg Alert 3 (destravou V1 aqui)
        }
        public static void AntiFreezeRPC()
        {
            MainForm.PS3.SetMemory(0xF01578, on);
        }

        public static void AntiFromOtherThings()
        {
            MainForm.PS3.SetMemory(0x1284F4C, nop); //Bug Crown Protect #1
            MainForm.PS3.SetMemory(0x9A3168, nop); //Bug Crown Protect #2
            MainForm.PS3.SetMemory(0x9E0720, nop); //Host Freeze Protection
            MainForm.PS3.SetMemory(0x1284B6C, new byte[] { 0x41, 0x80, 0xFF, 0x34 });//Host Freeze Protection(off:40 81 FF 34) 1.27: 0x1309EC4
            MainForm.PS3.SetMemory(0x125468, nop);//Cellphone freeze off:80 84 00 7C
            MainForm.PS3.SetMemory(0x12546CC, nop);//Cellphone freezeoff: 80 86 00 7C
        }

        public static void WarProtection(bool enabled)
        {
            if (enabled)
            {
                MainForm.PS3.SetMemory(0x1131D20, on); //Delete Texture Ped
                MainForm.PS3.SetMemory(0X11EDF30, on); //ultra protection #1 1.27: 126ABF0
                MainForm.PS3.SetMemory(0x4886C0, on); //ultra protection #2 1.27: 4BB9B0
                MainForm.PS3.SetMemory(0x488264, on); //ultra protection #3 1.27: 4BB558
                MainForm.PS3.SetMemory(0x4880EC, on); //ultra protection #4 1.27: 4BB3E0
                RPC.Call(RPC.Address.SET_STREAMING, 0);
                Thread.Sleep(1500);
                RPC.Call(RPC.Address.SET_GAME_PAUSES_FOR_STREAMING, 1);
            }
            else
            {
                MainForm.PS3.SetMemory(0x1131D20, off); //Delete Texture Ped
                MainForm.PS3.SetMemory(0X11EDF30, off); //ultra protection #1 1.27: 126ABF0
                MainForm.PS3.SetMemory(0x4886C0, off); //ultra protection #2 1.27: 4BB9B0
                MainForm.PS3.SetMemory(0x488264, new byte[] { 0xF8, 0x21, 0xFF, 0x91 }); //ultra protection #3 1.27: 4BB558
                MainForm.PS3.SetMemory(0x4880EC, off); //ultra protection #4 1.27: 4BB3E0
                RPC.Call(RPC.Address.SET_STREAMING, 1);
                Thread.Sleep(1500);
                RPC.Call(RPC.Address.SET_GAME_PAUSES_FOR_STREAMING, 0);
            }
        }
        static uint[] HashWeapons = { 0x99B507EA, 0x678B81B1, 0x4E875F73, 0xAB564B93, 0x63AB0442, 0x958A4A8F, 0x440E4788, 0x84BD7BFD, 0x1B06D571, 0x5EF9FEC4, 0x22D8FE39, 0x99AEEB3B, 0x13532244, 0x2BE6766B, 0xEFE7E2DF, 0xBFEFFF6D, 0x83BF0278, 0xAF113F99, 0x9D07F764, 0x7FD62962, 0x1D073A89, 0x7846A318, 0xE284C527, 0x9D61E50F, 0x3656C8C1, 0x05FC3C11, 0x0C472FE2, 0x33058E22, 0xA284510B, 0x4DD2DC56, 0xB1CA77B1, 0x687652CE, 0x42BF8A85, 0x93E220BD, 0x2C3731D9, 0xFDBC8A50, 0x24B17070, 0x060EC506, 0x34A67B97, 0xFDBADCED, 0x23C9F95C, 0x497FACC3, 0xF9E6AA4B, 0x61012683, 0xC0A3098D, 0xD205520E, 0xBFD21232, 0x7F229F94, 0x92A27487, 0x083839C4, 0x7F7497E5, 0xA89CB99E, 0x3AABBBAA, 0xC734385A, 0x787F0BB, 0x47757124, 0xD04C944D };
        public static void GivemeAllWeapons()
        {
            int me = RPC.Call(RPC.Address.PLAYER_PED_ID);
            for (int i = 0; i < (HashWeapons.Length); i++)
            {
                RPC.Call(RPC.Address.GIVE_DELAYED_WEAPON_TO_PED, new object[] { me, HashWeapons[i], 1 });
            }
            //C++ Code:
            //	uint Weapons[] = { 0x99B507EA, 0x678B81B1, 0x4E875F73, 0xAB564B93, 0x63AB0442, 0x958A4A8F, 0x440E4788, 0x84BD7BFD, 0x1B06D571, 0x5EF9FEC4, 0x22D8FE39, 0x99AEEB3B, 0x13532244, 0x2BE6766B, 0xEFE7E2DF, 0xBFEFFF6D, 0x83BF0278, 0xAF113F99, 0x9D07F764, 0x7FD62962, 0x1D073A89, 0x7846A318, 0xE284C527, 0x9D61E50F, 0x3656C8C1, 0x05FC3C11, 0x0C472FE2, 0x33058E22, 0xA284510B, 0x4DD2DC56, 0xB1CA77B1, 0x687652CE, 0x42BF8A85, 0x93E220BD, 0x2C3731D9, 0xFDBC8A50, 0x24B17070, 0x060EC506, 0x34A67B97, 0xFDBADCED, 0x23C9F95C, 0x497FACC3, 0xF9E6AA4B, 0x61012683, 0xC0A3098D, 0xD205520E, 0xBFD21232, 0x7F229F94, 0x92A27487, 0x083839C4, 0x7F7497E5, 0xA89CB99E, 0x3AABBBAA, 0xC734385A, 0x787F0BB, 0x47757124, 0xD04C944D };
            //for (int i = 0; i < (sizeof(Weapons) / 4); i++)
            //    GIVE_DELAYED_WEAPON_TO_PED(PLAYER_PED_ID(), Weapons[i], 9999, 1);
            //DRAW_MESSAGE("~b~All Weapons Received");
        }
        public static void GiveMecomponentWeapons()
        {
            int PLAYER_PED_ID = RPC.Call(RPC.Address.PLAYER_PED_ID);
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xAF113F99, 0x359B7AAE });
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xAF113F99, 0x359B7AAE }); //Advanced Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 0x7BC4CDDC }); //Combat PDW
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x13532244, 0x359B7AAE }); //Micro SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x22D8FE39, 0x359B7AAE }); //AP Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xD205520E, 0x359B7AAE }); //Heavy Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC0A3098D, 0x7BC4CDDC }); //Special Carbine
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7F229F94, 0x7BC4CDDC }); //Bullpump Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x1B06D571, 0x359B7AAE }); //Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x5EF9FEC4, 0x359B7AAE }); //Combat Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x99AEEB3B, 0x359B7AAE }); //.50 Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x2BE6766B, 0x7BC4CDDC }); //SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xEFE7E2DF, 0x7BC4CDDC }); //Assault SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 0x7BC4CDDC }); //Assault Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0x7BC4CDDC }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xAF113F99, 0x8EC1C979 }); //Extened Clip Advanced Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x05FC3C11, 0xBC54DA77 }); //Advanced scope Sniper Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x22D8FE39, 0x249A17D5 }); //Extended Clip AP Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x0C472FE2, 0xBC54DA77 }); //Advanced scope Heavy Sniper
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xD205520E, 0x64F9C62B }); //Heavy Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFD21232, 0x7B0033B3 }); //SNS Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC0A3098D, 0x7C8BD10E }); //Special Carbine
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xE284C527, 0x86BD7F72 }); //Assault Shotgun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7F229F94, 0xB3688B0F }); //Bullpump Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xD6C59CD6 }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x1B06D571, 0xED265A1C }); //Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x5EF9FEC4, 0xD67B4F2D }); //Combat Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x99AEEB3B, 0xD9D3AC92 }); //.50 Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x083839C4, 0x33BA12E8 }); //Vintage Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x13532244, 0x10E6BA2B }); //Micro SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x2BE6766B, 0x350966FB }); //SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xEFE7E2DF, 0xBB46E417 }); //Assault SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 0x334A5203 }); //Combat PDW
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x9D07F764, 0x82158B47 }); //MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x61012683, 0xEAC8C270 }); //Gusenberg
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 0xB1214F9B }); //Assault Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0x91109691 }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC734385A, 0xCCFD2AC5 }); //Marksman Rifle
            // RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, GET_HASH_KEY("PICKUP_WEAPON_HEAVYSHOTGUN"), 0x971CF6FD}); //Heavy Shotgun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xAF113F99, 0xAA2C45B4 }); //Extened Clip Advanced Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x05FC3C11, 0xD2443DDC }); //Advanced scope Sniper Rifle, 
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC0A3098D, 0xA0D89C42 }); //Special Carbine
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7F229F94, 0xAA2C45B4 }); //Bullpump Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x13532244, 0x9D2FBF29 }); //Micro SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x2BE6766B, 0x3CC6BA57 }); //SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xEFE7E2DF, 0x9D2FBF29 }); //Assault SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 0xAA2C45B4 }); //Combat PDW
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x3C00AFED, 0x82158B47 }); //MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 0x9D2FBF29 }); //Assault Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0xA0D89C42 }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xAF113F99, 0x8EC1C979 }); //Advanced Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x05FC3C11, 0xA73D4664 }); //Sniper Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x22D8FE39, 0xC304849A }); //AP Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xD205520E, 0xC304849A }); //Heavy Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC0A3098D, 0xA73D4664 }); //Special Carbine
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7F229F94, 0x837445AA }); //Bullpump Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x9D61E50F, 0xA73D4664 }); //Bullpump ShotGun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x1B06D571, 0x65EA7EBB }); //Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x5EF9FEC4, 0xC304849A }); //Combat Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x99AEEB3B, 0xA73D4664 }); //.50 Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x083839C4, 0xC304849A }); //Vintage Pistol
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x2BE6766B, 0xC304849A }); //SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xEFE7E2DF, 0xA73D4664 }); //Assault SMG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 0xA73D4664 }); //Assault Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0x837445AA }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC734385A, 0x837445AA }); //Marksman Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x1D073A89, 0xE608B35E }); //Pump Shotgun
            // RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, GET_HASH_KEY("PICKUP_WEAPON_HEAVYSHOTGUN"), 0xA73D4664}); //Heavy Shotgun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC0A3098D, 0xC164F53 }); //Special Carbine
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7F229F94, 0xC164F53 }); //Bullpump Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x9D61E50F, 0xC164F53 }); //Bullpump ShotGun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 0xC164F53 }); //Assault Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0xC164F53 }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xC734385A, 0xC164F53 }); //Marksman Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 0xC164F53 }); //Combat PDW
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xC164F53 }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0xE284C527, 0xC164F53 }); //Assault Shotgun
             //RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, GET_HASH_KEY("PICKUP_WEAPON_HEAVYSHOTGUN"), 0xC164F53}); //Heavy Shotgun
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x83BF0278, 0xD89B9658 }); //Carbine Rifle
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0x492B257C }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0x17DF42E9 }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xC34EF234 }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xB5E2575B }); //Combat MG
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xD6C59CD6 }); //Combat MG 
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xA0D89C42 });
            RPC.Call(RPC.Address.GIVE_WEAPON_COMPONENT_TO_PED, new object[] { PLAYER_PED_ID, 0x7FD62962, 0xE1FFB34A });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x1B06D571, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x5EF9FEC4, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x22D8FE39, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x99AEEB3B, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x13532244, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x2BE6766B, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xEFE7E2DF, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xBFEFFF6D, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x83BF0278, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xAF113F99, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x9D07F764, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x7FD62962, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x1D073A89, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x7846A318, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xE284C527, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x9D61E50F, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x05FC3C11, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x0C472FE2, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xA284510B, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xB1CA77B1, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x42BF8A85, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x687652CE, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x61012683, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xC0A3098D, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xD205520E, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xBFD21232, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x7F229F94, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x083839C4, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x7F7497E5, 2 }); 
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xA89CB99E, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x63AB0442, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xC734385A, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0xF9D04ADB, 2 });
            RPC.Call(RPC.Address.SET_PED_WEAPON_TINT_INDEX, new object[] { PLAYER_PED_ID, 0x0A3D4D34, 2 });
            //SET_PED_WEAPON_TINT_INDEX(PLAYER_PED_ID(), GET_HASH_KEY("PICKUP_WEAPON_HEAVYSHOTGUN"), 2);
        }

        public static void ProtectionEvents(bool Enable)
        {
            if (Enable)
            {
                MainForm.PS3.SetMemory(0x123F4A0, on); //REQUEST_CONTROL_EVENT (Anti Vehicle Control)
                MainForm.PS3.SetMemory(0x1245B6C, on);//VEHICLE_COMPONENT_CONTROL_EVENT
                MainForm.PS3.SetMemory(0x1245298, on); //GIVE_WEAPON_EVENT = Anti Give other player give weapons to you
                MainForm.PS3.SetMemory(0x1245578, on); //REMOVE_WEAPON_EVENT = Anti Give other player remove weapons to you
                MainForm.PS3.SetMemory(0x1245720, on); //REMOVE_ALL_WEAPONS_EVENT
                MainForm.PS3.SetMemory(0x12492D4, on); //ALTER_WANTED_LEVEL_EVENT
                MainForm.PS3.SetMemory(0x1252970, on); //NETWORK_INCREMENT_STAT_EVENT (Block Remote Stats #1)
                MainForm.PS3.SetMemory(0x124FCB0, on); //SCRIPT_ENTITY_STATE_CHANGE_EVENT (Block Remote Stats #2)
                MainForm.PS3.SetMemory(0x12533A0, on); //KICK_VOTES_EVENT
                MainForm.PS3.SetMemory(0x1256250, on); //NETWORK_PTFX_EVENT (Anti Particle)
                MainForm.PS3.SetMemory(0x124465C, on); //CLOCK_EVENT (Anti Change Wheater 1)
                MainForm.PS3.SetMemory(0x124494C, on); //WHEATER_EVENT (Anti Change Wheater 2)
                MainForm.PS3.SetMemory(0x124659C, on); //FIRE_EVENT
                MainForm.PS3.SetMemory(0x124C684, on); //CLEAR_AREA_EVENT
                MainForm.PS3.SetMemory(0x1240038, on); //GIVE_CONTROL_EVENT
                MainForm.PS3.SetMemory(0x1247174, on); //EXPLOSION_EVENT
                MainForm.PS3.SetMemory(0x124C988, on); //NETWORK_REQUEST_SYNCED_SCENE_EVENT(fix anti teleport #1)
                MainForm.PS3.SetMemory(0x124CBDC, on); //NETWORK_START_SYNCED_SCENE_EVENT (fix anti teleport #2)
                MainForm.PS3.SetMemory(0x124CFCC, on); //NETWORK_UPDATE_SYNCED_SCENE_EVENT (fix anti teleport #3)
                MainForm.PS3.SetMemory(0x124CD68, on); //NETWORK_STOP_SYNCED_SCENE_EVENT (fix anti teleport #4)
                MainForm.PS3.SetMemory(0x124DFF4, on); //NETWORK_CLEAR_PED_TASKS_EVENT (fix anti teleport #5)
                MainForm.PS3.SetMemory(0x124A674, on); //SCRIPTED_GAME_EVENT (Anti Kick Non Host #1)
                MainForm.PS3.SetMemory(0x124AB40, on); //REMOTE_SCRIPT_INFO_EVENT (Anti Kick Non Host #2)
                MainForm.PS3.SetMemory(0x124B000, on); //REMOTE_SCRIPT_LEAVE_EVENT (Anti Kick Non Host #3)
                MainForm.PS3.SetMemory(0x1252FC8, on); //REQUEST_DETACHMENT_EVENT = Anti Kick Non Host? or kick to detachment?
                MainForm.PS3.SetMemory(0x12485C8, on); //START_PROJECTILE_EVENT (Anti Projetil)
                MainForm.PS3.SetMemory(0x124372C, on); //PICKUP_EVENT
                MainForm.PS3.SetMemory(0x1241F0C, on); //WEAPON_DAMAGE_EVENT
            }
            else
            {
                MainForm.PS3.SetMemory(0x123F4A0, off);
                MainForm.PS3.SetMemory(0x1245B6C, off);
                MainForm.PS3.SetMemory(0x1245298, off);
                MainForm.PS3.SetMemory(0x1245578, off);
                MainForm.PS3.SetMemory(0x1245720, off);
                MainForm.PS3.SetMemory(0x12492D4, off);
                MainForm.PS3.SetMemory(0x124B000, off);
                MainForm.PS3.SetMemory(0x124C988, off);
                MainForm.PS3.SetMemory(0x124DFF4, off);
                MainForm.PS3.SetMemory(0x1252970, off);
                MainForm.PS3.SetMemory(0x12533A0, off);
                MainForm.PS3.SetMemory(0x1256250, off);
                MainForm.PS3.SetMemory(0x1252FC8, off);
                MainForm.PS3.SetMemory(0x1252FC8, off);
                MainForm.PS3.SetMemory(0x124465C, off);
                MainForm.PS3.SetMemory(0x124659C, off);
                MainForm.PS3.SetMemory(0x124C684, off);
                MainForm.PS3.SetMemory(0x1240038, off);
                MainForm.PS3.SetMemory(0x124494C, off);
                MainForm.PS3.SetMemory(0x124CD68, off);
                MainForm.PS3.SetMemory(0x124CFCC, off);
                MainForm.PS3.SetMemory(0x1247174, off);
                MainForm.PS3.SetMemory(0x124CBDC, off);
                MainForm.PS3.SetMemory(0x124A674, off);
                MainForm.PS3.SetMemory(0x124AB40, off);
                MainForm.PS3.SetMemory(0x124FCB0, off);
                MainForm.PS3.SetMemory(0x124372C, off);
                MainForm.PS3.SetMemory(0x1241F0C, off);
            }
        }
        public class RemoveEntityProt
        {
            static byte[] Enable = { 0x48, 00, 00, 0x44 };
            static byte[] Original = { 0x8B, 0xF2, 0x00, 0x29 };
            public static void cars(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B024C, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B024C, Original);
            }
            public static void objects(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B02BC, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B02BC, Original);
            }
            public static void pickups(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B032C, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B032C, Original);
            }
            public static void bikes(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B0408, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B0408, Original);
            }
            public static void helicopters(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B054C, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B054C, Original);
            }
            public static void planes(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B0624, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B0624, Original);
            }
            public static void boats(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B05B8, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B05B8, Original);
            }
            public static void trains(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x12B0474, Enable);
                else
                    MainForm.PS3.SetMemory(0x12B0474, Original);
            }
            public static void others(bool A)
            {
                if (A)
                    MainForm.PS3.SetMemory(0x101CD18, new byte[] { 0x48, 00, 00, 0x44 });
                else
                    MainForm.PS3.SetMemory(0x101CD18, new byte[] { 0x4B, 0xA8, 0x48, 0x8D });
            }
            //public static void constant(bool A) //From Paradise
            //{
            //    if (A)
            //        MainForm.PS3.SetMemory(, new byte[] { 01 69 6E 65 });
            //    else
            //        MainForm.PS3.SetMemory(, new byte[] { 00 69 6E 65 });
            //}
        }

       
        public static uint GET_HASH_KEY(string input) // Custom Hash Generator
        { // CLL 0xfa638911
            byte[] stingbytes = Encoding.UTF8.GetBytes(input.ToLower());
            uint num1 = 0U;
            for (int i = 0; i < stingbytes.Length; i++)
            {
                uint num2 = num1 + (uint)stingbytes[i];
                uint num3 = num2 + (num2 << 10);
                num1 = num3 ^ num3 >> 6;
            }
            uint num4 = num1 + (num1 << 3);
            uint num5 = num4 ^ num4 >> 11;
            return num5 + (num5 << 15);
        }

        public static int GetPlayerName(int PedID)
        {
            return RPC.Call(RPC.Address.GET_PLAYER_NAME, PedID);
        }
       
        public class UnloockOptions
        {
            public static void money(int dindin)
            {
                RPC.Call(RPC.Address.NETWORK_EARN_FROM_ROCKSTAR, dindin);
            }
            public static void MaxStats()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_STAM"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_STRN"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_LUNG"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_DRIV"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_FLY"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_SHO"), 100);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SCRIPT_INCREASE_STL"), 100);
                STAT_SET_INT(GET_HASH_KEY("SPECIAL_ABILITY"), 150);
                STAT_SET_INT(GET_HASH_KEY("STAMINA"), 150);
                STAT_SET_INT(GET_HASH_KEY("STRENGTH"), 150);
                STAT_SET_INT(GET_HASH_KEY("LUNG_CAPACITY"), 150);
                STAT_SET_INT(GET_HASH_KEY("WHEELIE_ABILITY"), 150);
                STAT_SET_INT(GET_HASH_KEY("FLYING_ABILITY"), 150);
                STAT_SET_INT(GET_HASH_KEY("SHOOTING_ABILITY"), 150);
                STAT_SET_INT(GET_HASH_KEY("STEALTH_ABILITY"), 150);
            }
            public static void ModdedRoll()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "SHOOTING_ABILITY"), 150);
            }
            public static void MaxArmour()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "MP_CHAR_ARMOUR_1_COUNT"), 2000000000);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "MP_CHAR_ARMOUR_2_COUNT"), 2000000000);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "MP_CHAR_ARMOUR_3_COUNT"), 2000000000);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "MP_CHAR_ARMOUR_4_COUNT"), 2000000000);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "MP_CHAR_ARMOUR_5_COUNT"), 2000000000);
            }
            public static void UnlockClothes()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMHORDWAVESSURVIVE"), 10);//GAMEPLAY::GET_HASH_KEY("MP0_")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMPICKUPDLCCRATE1ST"), 1);//GAMEPLAY::GET_HASH_KEY("MP0_A")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_WIN_CAPTURE_DONT_DYING"), 25);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_WIN_CAPTURE_DONT_DYING")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_DO_HEIST_AS_MEMBER"), 25);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_DO_HEIST_AS_MEMBER")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_PICKUP_CAP_PACKAGES"), 100);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_PICKUP_CAP_PACKAGES")
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FINISH_HEIST_NO_DAMAGE"), 1);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_FINISH_HEIST_NO_DAMAGE")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_WIN_GOLD_MEDAL_HEISTS"), 25);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_WIN_GOLD_MEDAL_HEISTS")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_KILL_TEAM_YOURSELF_LTS"), 25);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_KILL_TEAM_YOURSELF_LTS")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_KILL_PSYCHOPATHS"), 100);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_KILL_PSYCHOPATHS")
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_DO_HEIST_AS_THE_LEADER"), 25);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_DO_HEIST_AS_THE_LEADER")
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_STORE_20_CAR_IN_GARAGES"), 1);//GAMEPLAY::GET_HASH_KEY("MP0_AWD_STORE_20_CAR_IN_GARAGES")
            }
            public static void UnloockTattos()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FM_DM_WINS"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FM_TDM_MVP"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FM_DM_TOTALKILLS"), 500);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMATTGANGHQ"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMBBETWIN"), 50000);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMWINEVERYGAMEMODE"), 1);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMRACEWORLDRECHOLDER"), 1);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMFULLYMODDEDCAR"), 1);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMMOSTKILLSSURVIVE"), 1);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMKILL3ANDWINGTARACE"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMKILLBOUNTY"), 25);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMREVENGEKILLSDM"), 50);
                STAT_SET_BOOL(GET_HASH_KEY("MP0_" + "AWD_FMKILLSTREAKSDM"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_HOLD_UP_SHOPS"), 20);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_LAPDANCES"), 25);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_SECURITY_CARS_ROBBED"), 25);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_RACES_WON"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_CAR_BOMBS_ENEMY_KILLS"), 25);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "PLAYER_HEADSHOTS"), 500);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "DB_PLAYER_KILLS"), 1000);
            }
            public static void UnlockHairStyles()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_1"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_3"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_4"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_5"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_6"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CLTHS_AVAILABLE_HAIR_7"), -1);
            }

            public static void SetRank(int sdk)
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_XP_FM"), sdk);
            }

            public static void LSC()
            {

                STAT_SET_INT(GET_HASH_KEY("MP0_" + "RACES_WON"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_1_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_2_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_3_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_4_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_5_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_6_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "CHAR_FM_CARMOD_7_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMRALLYWONDRIVE"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMRALLYWONNAV"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMWINSEARACE"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FMWINAIRRACE"), 1);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "NUMBER_TURBO_STARTS_IN_RACE"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "USJS_COMPLETED"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "AWD_FM_RACES_FASTEST_LAP"), 50);
                STAT_SET_INT(GET_HASH_KEY("MP0_" + "NUMBER_SLIPSTREAMS_IN_RACE"), 100);
                STAT_SET_INT(GET_HASH_KEY("AWD_WIN_CAPTURES"), 50);                // Chrome Rims - Sport
                STAT_SET_INT(GET_HASH_KEY("AWD_DROPOFF_CAP_PACKAGES"), 100);       // Chrome Rims - Lowrider
                STAT_SET_INT(GET_HASH_KEY("AWD_KILL_CARRIER_CAPTURE"), 100);       // Chrome Rims - Offroad
                STAT_SET_INT(GET_HASH_KEY("AWD_FINISH_HEISTS"), 50);               // Chrome Rims - High End
                STAT_SET_INT(GET_HASH_KEY("AWD_FINISH_HEIST_SETUP_JOB"), 50);      // Chrome Rims - Tuner
                STAT_SET_INT(GET_HASH_KEY("AWD_NIGHTVISION_KILLS"), 100);          // Chrome Rims - Bike
                STAT_SET_INT(GET_HASH_KEY("AWD_WIN_LAST_TEAM_STANDINGS"), 50);     // Chrome Rims - SUV
                STAT_SET_INT(GET_HASH_KEY("AWD_ONLY_PLAYER_ALIVE_LTS"), 50);       // Chrome Rims - Muscle
            }

            public static void UnlockAllWeaponCamos()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_PISTOL_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CMBTPISTOL_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_APPISTOL_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MICROSMG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SMG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSMG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CRBNRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADVRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_HVYSNIPER_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SNIPERRFL_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSHTGN_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_PUMP_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_GRNLAUNCH_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_RPG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MINIGUNS_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSMG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CRBNRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADVRIFLE_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_HVYSNIPER_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SNIPERRFL_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CMBTMG_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_PISTOL_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CMBTPISTOL_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_APPISTOL_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MICROSMG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SMG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSMG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CRBNRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADVRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_HVYSNIPER_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SNIPERRFL_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSHTGN_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_PUMP_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_GRNLAUNCH_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_RPG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MINIGUNS_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTSMG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ASLTRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CRBNRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADVRIFLE_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_HVYSNIPER_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_SNIPERRFL_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_MG_ENEMY_KILLS"), 600);
                STAT_SET_INT(GET_HASH_KEY("MP0_CMBTMG_ENEMY_KILLS"), 600);
            }


            public static void BuyAllGuns()
            {
                STAT_SET_INT(GET_HASH_KEY("MP0_ADMIN_WEAPON_GV_BS_1"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADMIN_WEAPON_GV_BS_2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_ADMIN_WEAPON_GV_BS_3"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_BOTTLE_IN_POSSESSION"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_UNLOCKED"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_UNLOCKED2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_WEAP_FM_PURCHASE"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_WEAP_FM_PURCHASE2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_ADDON_1_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_ADDON_2_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_ADDON_3_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_ADDON_4_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_FM_WEAP_ADDON_5_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_WEAP_FM_ADDON_PURCH"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_WEAP_FM_ADDON_PURCH2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_WEAP_FM_ADDON_PURCH3"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_WEAP_FM_ADDON_PURCH4"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_WEAP_FM_ADDON_PURCH5"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_1_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_2_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_3_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_4_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_5_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_6_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_7_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_8_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_9_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_10_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_11_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_12_FM_UNLCK"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE2"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE3"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE4"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE5"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE6"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE7"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE8"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE9"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE10"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE11"), -1);
                STAT_SET_INT(GET_HASH_KEY("MP0_CHAR_KIT_FM_PURCHASE12"), -1);
            }

        }
    }
}