using DevExpress.XtraEditors;
using PS3Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using Project_Eternal_GTA_1._12.Program;

namespace Project_Eternal_GTA_1._12
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region Working
        public static PS3API PS3 = new PS3API();
        byte[] off = new byte[] { 0x7C, 0x08, 0x02, 0xA6 };
        byte[] on = new byte[] { 0x4E, 0x80, 0x00, 0x20 };
        static byte[] nop = { 0x60, 0x00, 0x00, 0x00 };
        bool firstonemsg = true;
        bool warP_MSG = true;
        bool noSpamFreezeRPC = true;
        string GetWhenConnected;
        bool UsingSPRX;
        private static string GetMacAddress()
        {
            string result = "";
            NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in allNetworkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    result = networkInterface.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return result;
        }

        void Gay() //só esperando meu vs abrir
        {
            this.json.Text = "{\r\n  \"username\": \"Project Eternal Info\",\r\n  \"embeds\": [\r\n    {\r\n      \"descriptio" +
   "n\": \"PSN Name: " + PS3.Extension.ReadString(0x412451E4) + " \",\r\n      \"title\": \"Project Eternal GTA RTM\",\r\n      \"color\": 1018" +
   "364\r\n    }\r\n  ]\r\n}";
            sendDiscordWebhook("https://discord.com/api/webhooks/961762914530885673/UHzjWtDyvPIWuX0liWUY0Yi06ndIRK_k5a3I7NEouQEL2q8kLcIvDj_w5xih_BFc3oy9", json.Text);
        }
        public static void sendDiscordWebhook(string URL, string escapedjson)
        {
            var wr = WebRequest.Create(URL);
            wr.ContentType = "application/json";
            wr.Method = "POST";
            using (var sw = new StreamWriter(wr.GetRequestStream()))
                sw.Write(escapedjson);
            wr.GetResponse();
        }

        #endregion
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //ByeByeGay.Start();




            for (uint num = 0; num < 16; num += 1u)
            {
                ListViewItem LV = new ListViewItem(num.ToString());
                LV.SubItems.Add("<Null>");
                LV.SubItems.Add("0.0.0.0");
                MainGrabber.Items.Add(LV);
            }
        }
           
            

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            Functions.AntiFreezeV1();
            Functions.AntiFreezeV2(true);
            Functions.AntiSessionByWe(); //Anti V1 ~ V2
            Functions.AntiFreezeRPC();
            Functions.AntiFreezeFromArabic();
            Functions.AntiFromOtherThings();
            //Corrupted Testing:
            //PS3.SetMemory(0x9A8650, on); //Vegetal Protection convida vix ele convidou dkaj
            // PS3.SetMemory(0x4429C0, nop); //Maybe Fix

            ////PS3.SetMemory(0x9A86F4, nop);
            ////PS3.SetMemory(0x9A886C, nop);
            //// PS3.SetMemory(0x9A85B4, nop); //Fix
            ////PS3.SetMemory(0x009A87C8, on);

            //PS3.SetMemory(0x1B8DE30, new byte[] { 0x48, 00, 0x02, 0x0C }); //Need test
        }

        void OnConnect(SelectAPI api)
        {
            PS3.ChangeAPI(api);
            if (PS3.ConnectTarget() && PS3.AttachProcess())
            {
                ConnectLabel.Text = PS3.Extension.ReadString(0x412451E4);
                // textEdit1.Text = PS3.Extension.ReadString(0x412451E4); //Local Name
                textEdit2.Text = PS3.Extension.ReadString(0x1F379EC); //Real Name
                textEdit3.Text = GetWhenConnected; //PSN
                if (PS3.Extension.ReadString(0x10058000) != "T")
                    RPC.Enable();
                RPCLabel.Text = "Enabled";
                RPCLabel.ForeColor = Color.Lime;
                ConnectLabel.ForeColor = Color.Lime;
                GetWhenConnected = PS3.Extension.ReadString(0x1F379C0);
                if (PS3.Extension.ReadString(0x99214B) == "T")
                    UsingSPRX = true;
                checkEdit15.Checked = true;// bypass prologue
                Gay();
                Notify("Connected", 5000);
            }
            else
            {
                Notify("Error to Connect\nCheck your API", 5000);
            }
        }

        private void cCAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnConnect(SelectAPI.ControlConsole);
        }

        private void tMAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnConnect(SelectAPI.TargetManager);
        }

        private void hENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnConnect(SelectAPI.PS3Manager);
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit3.Checked)
            {
                Functions.RemoveEntityProt.bikes(true);
                Functions.RemoveEntityProt.boats(true);
                Functions.RemoveEntityProt.cars(true);
                Functions.RemoveEntityProt.helicopters(true);
                Functions.RemoveEntityProt.objects(true);
                Functions.RemoveEntityProt.others(true);
                Functions.RemoveEntityProt.pickups(true);
                Functions.RemoveEntityProt.planes(true);
                Functions.RemoveEntityProt.trains(true);
                //Functions.RemoveEntityProt.constant(true);
            }
            else
            {
                Functions.RemoveEntityProt.bikes(false);
                Functions.RemoveEntityProt.boats(false);
                Functions.RemoveEntityProt.cars(false);
                Functions.RemoveEntityProt.helicopters(false);
                Functions.RemoveEntityProt.objects(false);
                Functions.RemoveEntityProt.others(false);
                Functions.RemoveEntityProt.pickups(false);
                Functions.RemoveEntityProt.planes(false);
                Functions.RemoveEntityProt.trains(false);
               // Functions.RemoveEntityProt.constant(false);
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                Functions.ProtectionEvents(true);
            }
            else
            {
                Functions.ProtectionEvents(false);
            }
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit4.Checked)
            {
                Functions.RemoveEntityProt.planes(true);
                Functions.RemoveEntityProt.helicopters(true);
            }
            else
            {
                Functions.RemoveEntityProt.planes(false);
                Functions.RemoveEntityProt.helicopters(false);
            }
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            if (warP_MSG)
            {
                Notify("It's will stabilize your FPS on modder wars.\nBut Your Vision and some textures go are bugged");
                warP_MSG = false;
            }
            if (checkEdit5.Checked)
            {
                Functions.WarProtection(true);
            }
            else
            {
                Functions.WarProtection(false);
            }
        }

        private void checkEdit9_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkEdit9.Checked)
            {
                Functions.FreezeLobbyV1(true);
            }
            else
            {
                Functions.FreezeLobbyV1(false);
            }
        }

        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
            if (firstonemsg)
            {
                XtraMessageBox.Show("Teleport everyone to you, or go to the player\n select any weapon and shoot\nand just wait.", "Project Eternal");
                firstonemsg = false;
            }
            if (checkEdit11.Checked)
            {
                Functions.FreezeLobbyV2_Gun(true);
            }
            else
            {
                Functions.FreezeLobbyV2_Gun(false);
            }
        }

        private void checkEdit12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit12.Checked)
            {
                Functions.FreezeLobbyV4_Ghost(true);
            }
            else
            {
                Functions.FreezeLobbyV4_Ghost(false);
            }
        }

        private void checkEdit13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit13.Checked)
            {
                Functions.freezeLobbyCorrupted1(true);
                Functions.freezeLobbyCorrupted2(true);
            }
            else
            {
                Functions.freezeLobbyCorrupted1(false);
                Functions.freezeLobbyCorrupted2(false);
            }
        }

        private void checkEdit14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit14.Checked)
            {
                PS3.SetMemory(0xA49F83, new byte[] { 0x14 });
                PS3.SetMemory(0xA49880, new byte[] { 0x10 });
                PS3.SetMemory(0xA49F24, new byte[] { 0x10 });
                //PS3.SetMemory(0xA49F68, new byte[] { 0x10 });
            }
            else
            {
                PS3.SetMemory(0xA49F83, new byte[] { 0x10 });
                PS3.SetMemory(0xA49880, new byte[] { 0x80 });
                PS3.SetMemory(0xA49F24, new byte[] { 0x80 });
                //PS3.SetMemory(0xA49F68, new byte[] { 0x80 });
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (noSpamFreezeRPC)
            {
                XtraMessageBox.Show("Don'ts spam this\nRisk of auto-freeze", "Project Eternal");
                noSpamFreezeRPC = false;
            }
            Functions.FreezeRPC_and_BlackScreen();
        }

        public static int player_ped_id()
        {
            return RPC.Call(RPC.Address.PLAYER_PED_ID, new object[0]);
        }

        public static float[] get_entity_coords(int Entity)
        {
            float[] numArray = new float[3];
            RPC.Call(RPC.Address.GET_ENTITY_COORDS, RPC.Address.entityXCoord, Entity);
            numArray[0] = PS3.Extension.ReadFloat(RPC.Address.entityXCoord);
            numArray[1] = PS3.Extension.ReadFloat(RPC.Address.entityYCoord);
            numArray[2] = PS3.Extension.ReadFloat(RPC.Address.entityZCoord);
            return numArray;
        }
        private void outOfGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                //if(UsingSPRX)

                PS3.SetMemory(0x53441C, new byte[] { 0x6C, 0x75, 0x69, 0x00 });
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                PS3.AttachProcess();
                return;
            }
            // XtraMessageBox.Show("Use we SPRX to use this.", "Project Eternal");
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            PS3.Extension.WriteString(0x1F379EC, textEdit2.Text); //Real Name Offset
            PS3.Extension.WriteString(0x412451E4, textEdit2.Text); //Local Too
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            PS3.Extension.WriteString(0x1F379EC, ConnectLabel.Text); //Real Name Offset
            PS3.Extension.WriteString(0x412451E4, ConnectLabel.Text); //Local Too
        }


        private void simpleButton10_Click(object sender, EventArgs e)
        {
            PS3.Extension.WriteString(0x1F379C0, textEdit3.Text);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            PS3.Extension.WriteString(0x1F379C0, GetWhenConnected);
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x0099213D, new byte[] { 0x43, 0x00 });
                return;
            }
            Functions.UnloockOptions.MaxStats();
            Functions.UnloockOptions.ModdedRoll();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x0099213F, new byte[] { 0x50, 0x00 });
                return;
            }
            Functions.UnloockOptions.UnlockAllWeaponCamos();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x00992141, new byte[] { 0x56, 0x00 });
                return;
            }
            Functions.UnloockOptions.LSC();
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x00992143, new byte[] { 0x43, 0x00 });
                return;
            }
            Functions.UnloockOptions.UnloockTattos();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x00992145, new byte[] { 0x43, 0x00 });
                return;
            }
            Functions.UnloockOptions.UnlockHairStyles();
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.SetRank(0);
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.SetRank(2165850);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x00992147, new byte[] { 0x43, 0x00 });
                return;
            }
            Functions.GivemeAllWeapons();
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.SetRank(12406850);
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.SetRank(1787576850);
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.money(1000000);
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.money(10000000);
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.money(50000000);
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            Functions.UnloockOptions.money(100000000);
        }

        private void simpleButton25_Click(object sender, EventArgs e)
        {
            if (UsingSPRX)
            {
                PS3.SetMemory(0x00992149, new byte[] { 0x43, 0x00 });
                return;
            }
            Functions.UnloockOptions.BuyAllGuns();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void checkEdit17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit17.Checked)
            {
                RPC.Call(RPC.Address.SET_ENTITY_VISIBLE, RPC.Call(RPC.Address.PLAYER_PED_ID), 0); //Player visible = false 
            }
            else
            {
                RPC.Call(RPC.Address.SET_ENTITY_VISIBLE, RPC.Call(RPC.Address.PLAYER_PED_ID), 1);
            }
        }

        private void checkEdit16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit16.Checked)
            {
                RPC.Call(RPC.Address.SET_PLAYER_INVINCIBLE, RPC.Call(RPC.Address.PLAYER_ID), 1);
            }
            else
            {
                RPC.Call(RPC.Address.SET_PLAYER_INVINCIBLE, RPC.Call(RPC.Address.PLAYER_ID), 0);
            }
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            RPC.Call(RPC.Address.CLEAR_PLAYER_WANTED_LEVEL, RPC.Call(RPC.Address.PLAYER_ID), 1);
        }

        public static void set_player_weapon_damage_modifier(int Player, float amount)
        {
            RPC.Call(RPC.Address.SET_PLAYER_WEAPON_DAMAGE_MODIFIER, Player, amount);
        }

        private void checkEdit19_CheckedChanged(object sender, EventArgs e)
        {
            int player = RPC.Call(RPC.Address.PLAYER_ID);
            if (checkEdit19.Checked)
            {
                set_player_weapon_damage_modifier(player, 700f);
            }
            else
            {
                set_player_weapon_damage_modifier(player, 1f);
            }
        }

        private void checkEdit15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit15.Checked)
            {
                Functions.BypassPrologue(true);
            }
            else
            {
                Functions.BypassPrologue(false);
            }
        }

        private void checkEdit18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit18.Checked)
            {
                RPC.Call(RPC.Address.SET_PED_INFINITE_AMMO_CLIP, RPC.Call(RPC.Address.PLAYER_PED_ID), 1);
            }
            else
            {
                RPC.Call(RPC.Address.SET_PED_INFINITE_AMMO_CLIP, RPC.Call(RPC.Address.PLAYER_PED_ID), 0);
            }
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit7.Checked)
            {
                if (UsingSPRX)
                {
                    PS3.SetMemory(0x0099214F, new byte[] { 0x43, 0x00 });
                    return;
                }
                checkEdit7.Checked = false;
                XtraMessageBox.Show("Use we SPRX to use this.", "Project Eternal");
            }
            else
            {
                PS3.SetMemory(0x0099214F, new byte[] { 0x00, 0x00 });

            }
        }

        private void checkEdit7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit7.Checked)
            {
                if (UsingSPRX)
                {
                    PS3.SetMemory(0x00992151, new byte[] { 0x43, 0x00 });
                    return;
                }
                checkEdit7.Checked = false;
                XtraMessageBox.Show("Use we SPRX to use this.", "Project Eternal");
            }
            else
            {
                PS3.SetMemory(0x00992151, new byte[] { 0x00, 0x00 });
            }
        }

        private void checkEdit8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit8.Checked)
            {
                if (UsingSPRX)
                {
                    PS3.SetMemory(0x0099214D, new byte[] { 0x43, 0x00 });
                    return;
                }
                checkEdit8.Checked = false;
                XtraMessageBox.Show("Use we SPRX to use this.", "Project Eternal");
            }
            else
            {
                PS3.SetMemory(0x0099214D, new byte[] { 0x00, 0x00 });
            }

        }

        private void checkEdit10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit10.Checked)
            {
                checkEdit2.Checked = true;
                PS3.SetMemory(0x4630A0, on); //Black Ped Work
                PS3.SetMemory(0x4643DC, on); //Oasis Too NEED OFF
                PS3.SetMemory(0x11FF014, on); //ped protection test
                PS3.SetMemory(0x985BA4, nop);//ped protection test
                return;
            }
            PS3.SetMemory(0x4630A0, off);
            PS3.SetMemory(0x4643DC, off);
            PS3.SetMemory(0x11FF014, off);
            PS3.SetMemory(0x985BA4, new byte[] { 0x80, 0x86, 0x00, 0x00 });
            //CHECK LATER
        }
        private void simpleButton27_Click(object sender, EventArgs e)
        {
            byte[] bytes1_on = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x33, 0x1B, 0xFA, 0xE0, 0x00, 0x03, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x33, 0x1B, 0xFA, 0xC0, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3E, 0x4C, 0xE7, 0x04, 0x3D, 0xF0, 0xD8, 0x45, 0x3E, 0x4C, 0xE7, 0x04, 0x3D, 0xF0, 0xD8, 0x45, 0x44, 0x7A, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0xA0, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x3F, 0xC0, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC7 };
            byte[] bytes2_on = { 0x47, 0xC3, 0x4F, 0x80 };
            byte[] bytes3_on = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] bytes4_on = { 0x0C, 0x00, 0x00, 0x00 };
            PS3.Extension.WriteBytes(0x402f2890, bytes1_on);
            PS3.Extension.WriteBytes(0x4122bf88, bytes2_on);
            PS3.Extension.WriteBytes(0x4122c060, bytes3_on);
            PS3.Extension.WriteBytes(0x4122c08c, bytes4_on);
            XtraMessageBox.Show("Need spawn a TRIBIKE on you to start freeze.", "Project Eternal 2.1.0.0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void checkEdit20_CheckedChanged(object sender, EventArgs e)
        {
            PS3.SetMemory(0x9A8650, on);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private static string convertIp(byte[] ip)
        {
            return string.Format("{0}.{1}.{2}.{3}", new object[]
            {
                ip[0],
                ip[1],
                ip[2],
                ip[3]
            });
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        public static string ReadIP(byte[] ip) //acho que isso nao vai pegar lol
        {
            return string.Format("{0}.{1}.{2}.{3}", new object[]
            {
                ip[0],
                ip[1],
                ip[2],
                ip[3]
            });
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            for (uint num = 0; num < 16; num += 1u)
            {
                ListViewItem LV = new ListViewItem(num.ToString());
                string name = PS3.Extension.ReadString(0x400586E8 + num * 0x88);
                bool flag = name == string.Empty;
                if (flag)
                {
                    name = "<Null>";
                }
                bool flag2 = name == string.Empty;
               
                string IP = ReadIP(PS3.Extension.ReadBytes(0x40058730 + num * 0x88, 4)); //0x40058721
                if (IP == "255.255.255.255")
                {
                    IP = "0.0.0.0";
                }
                LV.SubItems.Add(name);
                LV.SubItems.Add(IP);
                listView2.Items.Add(LV);
            }
        }


        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            byte[] by = new byte[100];
            PS3.Extension.WriteBytes(0x1004004C, by);
            PS3.Extension.WriteString(0x1004004C + 8, textEdit1.Text);
            RPC.Call(0x982B00 * 2, 0x40054CE0, 0x1004004C, 1, 0, 0, 0, 0x1C3324C);
        }

        private void getListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FriendList.Items.Clear();
            byte[] dataDump = PS3.Extension.ReadBytes(0x1F37A40, 100 * 0x34);
            for (uint num = 0; num < dataDump.Length / 0x34; num += 1u)
            {
                string name = Encoding.UTF8.GetString(dataDump, (int)(num * 0x34), 0x34).Trim('\0');
                if (name == "����")
                {
                    break;
                }
                ListViewItem LV = new ListViewItem(name);
                    FriendList.Items.Add(LV);
                
            }

            //FriendList.Items.Clear();
            //for (uint num = 0U; num < 150; num += 1U)
            //{
            //    string text = PS3.Extension.ReadString(Convert.ToUInt32(0x1F37A40 + num * 0x34));
            //    bool flag = text == string.Empty;
            //    if (flag)
            //    {
            //        break;
            //    }
            //    ListViewItem listViewItem = new ListViewItem(text);
            //    FriendList.Items.Add(listViewItem);
            //}
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MainGrabber.Items.Clear();
            byte[] dataDump = PS3.Extension.ReadBytes(0x400586e8, 16 * 0x88);
            for (uint num = 0; num < 16; num += 1u)
            {
                ListViewItem LV = new ListViewItem(num.ToString());

                string name = Encoding.UTF8.GetString(dataDump, (int)(num * 0x88), 0x88).TrimEnd('\0'); //ue la pega normal
                byte[] ipBytes = new byte[4];

                Array.Copy(dataDump, (int)(0x40058730 + num * 0x88 - 0x400586e8), ipBytes, 0, 4); // Ajuste do deslocamento

            
  
                string IP = ReadIP(ipBytes);

                if (IP == "255.255.255.255")
                {
                    name = "<Null>";
                    IP = "0.0.0.0";
                }
                if (name == "TestMy-RTM-Tool" || name == "xPanTeRa-Return-") //aqui nao sei orque nao pega bug msm
                    IP = "Dev";

                LV.SubItems.Add(name);
                LV.SubItems.Add(IP);
                MainGrabber.Items.Add(LV);
            }

            //MainGrabber.Items.Clear();
            //for (uint num = 0; num < 16; num += 1u)
            //{
            //    ListViewItem LV = new ListViewItem(num.ToString());
            //    string name = PS3.Extension.ReadString(0x400586e8 + num * 0x88); //maybe in next player
            //    bool flag = name == string.Empty;
            //    if (flag)
            //    {
            //        name = "<Null>";
            //    }
            //    bool flag2 = name == string.Empty;

            //    string IP = ReadIP(PS3.Extension.ReadBytes(0x40058730 + num * 0x88, 4)); //0x40058730 <- start in next player | Founded with bruteForce: 0x40058721
            //    if (IP == "255.255.255.255")
            //    {
            //        IP = "0.0.0.0";
            //    }
            //    if (name == "TestMy-RTM-Tool" || name == "xPanTeRa-Return-")
            //    {
            //       IP = "Dev";
            //    }
            //    LV.SubItems.Add(name);
            //    LV.SubItems.Add(IP);
            //    MainGrabber.Items.Add(LV);
            //}
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sendInviteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uint num = Convert.ToUInt32(FriendList.FocusedItem.Index);
            int num2 = (int)num;
            string text = PS3.Extension.ReadString(Convert.ToUInt32(0x1F37A40 + num2 * 0x34));
            byte[] by = new byte[100];
            PS3.Extension.WriteBytes(0x1004004C, by);
            PS3.Extension.WriteString(0x1004004C + 8, text);
            RPC.Call(0x982B00 * 2, 0x40054CE0, 0x1004004C, 1, 0, 0, 0, 0x1C3324C);
        }

        private void checkEdit21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit21.Checked)
            {
                PS3.SetMemory(0x12CEF20, new byte[] { 0x6E });
            }
            else
            {
                PS3.SetMemory(0x12CEF20, new byte[] { 0x4E });
            }
        }

        private void checkEdit22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit22.Checked)
            {
                PS3.SetMemory(0x15972FC, nop);
            }
            else
            {
                PS3.SetMemory(0x15972FC, new byte[] { 0x4B, 0x4B, 0x25, 0x59 });
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }


        uint deslocamentoInicial = 0x40050000;//0x40050000

        private void button3_Click(object sender, EventArgs e)
        {
            //    listView2.Items.Clear();

            //    uint incrementoDeslocamento = 1; // Incremento do deslocamento inicial a cada clique

            //    for (uint num = 0; num < 16; num += 1u)
            //    {
            //        ListViewItem LV = new ListViewItem(num.ToString());

            //        string textName = PS3.Extension.ReadString(deslocamentoInicial + num * 0x88);

            //        LV.SubItems.Add(textName);
            //        ListViewItem listViewItem = new ListViewItem(Convert.ToString(num));
            //        listViewItem.SubItems.Add(textName);
            //        listView2.Items.Add(LV);

            //        if (textName.Contains("Thefamousthc"))
            //        {
            //            textBox1.Text = deslocamentoInicial.ToString();
            //            MessageBox.Show("Offset Founded = " + deslocamentoInicial.ToString());
            //            break; // Interrompe o loop se a string alvo for encontrada  
            //        }

            //        deslocamentoInicial += incrementoDeslocamento; // Aumenta o deslocamento inicial a cada iteração
            //    }
            //}

            listView2.Items.Clear();

            uint incrementoDeslocamento = 1; // Incremento do deslocamento inicial a cada clique

            for (uint num = 0; num < 18; num += 1u)
            {
                ListViewItem LV = new ListViewItem(num.ToString());

                //string textName = PS3.Extension.ReadString(deslocamentoInicial + num * 0x88);
                string textName = ReadIP(PS3.Extension.ReadBytes(deslocamentoInicial + num, 4));

                LV.SubItems.Add(textName);
                ListViewItem listViewItem = new ListViewItem(Convert.ToString(num));
                listViewItem.SubItems.Add(textName);
                listView2.Items.Add(LV);

                if (textName == "177.148.26.162")
                {
                    textBox1.Text = deslocamentoInicial.ToString();
                    MessageBox.Show("Offset Founded = " + deslocamentoInicial.ToString());
                    break; // Interrompe o loop se a string alvo for encontrada  
                }

                deslocamentoInicial += incrementoDeslocamento; // Aumenta o deslocamento inicial a cada iteração
            }
        }

        private void getHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string hostname = PS3.Extension.ReadString(0x40056120); //0x40054CF0
            if (hostname == "")
                hostname = "null";
            getHostToolStripMenuItem.Text = hostname;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainGrabber.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = MainGrabber.SelectedItems[0];
                string itemText = selectedItem.SubItems[2].Text;
                toolStripTextBox1.Text = itemText;
            }
        }

        private void kickHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(PS3.Extension.ReadString(0x40056120) == ConnectLabel.Text)
            {
                ListViewItem num = FriendList.SelectedItems[0];
                string num2 = num.SubItems[0].Text;
                RPC.Call(RPC.Address.NETWORK_SESSION_KICK_PLAYER, num2);
            }
            else
            {
                Notify("You need be host.", 5000);
            }
        }
    }
}
