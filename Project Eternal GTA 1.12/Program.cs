using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using PS3Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.Skins;
using DevExpress.XtraSplashScreen;
using authguard;

namespace Project_Eternal_GTA_1._12
{
    static class Program
    {
        public static API auth_sample = new API(
             "1.0",
             "6nccc2yih7UB5CSnfql1n1uCjP5iFwunNrK",
             "cd567639d9f77ef608b37b4014eb1a5a",
             show_messages: true
         );
        static void Main()
        {
            // auth_sample.init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Dev:
            BonusSkins.Register();
            Assembly asm = typeof(DevExpress.UserSkins.BlackTheme).Assembly;
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(asm);
            UserLookAndFeel.Default.SkinName = "My Visual Studio 2013 Dark";
            UserLookAndFeel.Default.SkinMaskColor = Color.Red;
            //Config Appdata:
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\RTM Tool GTA V 1.12 Freeze\\";
            Directory.CreateDirectory(folderPath);
            TMAPI.ExtractDLL("ps3tmapi.dll");//Load PS3Tmapi.Dll

            SkinManager.EnableFormSkins();

            Application.Run(new MainForm());
        }
        public class SkinRegistration : Component
        {
            public SkinRegistration()
            {
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BlackTheme).Assembly);
            }
        }

        public static readonly NotifyIcon _notifyIcon = new NotifyIcon
        {
            Icon = Properties.Resources.PE___Com_Fundo,
            BalloonTipTitle = "Eternal RTM Tool (GTA v1.12) v" + Application.ProductVersion,
            BalloonTipIcon = ToolTipIcon.Info,
            Text = "Eternal"
        };

        public static void Notify(string message, int duration = 3000)
        {
            _notifyIcon.Visible = true;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.ShowBalloonTip(duration);
        }
    }
}
