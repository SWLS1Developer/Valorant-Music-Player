using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GlobalLowLevelHooks;
using System.Runtime.InteropServices;

namespace VMP___Valorant_Music_Player
{
    public partial class Form1 : Form
    {
   static Form1 frm;
        string Pttkey;
        string PUkey;

        public Form1()
        {
            InitializeComponent();
            frm = this; 
        }
      public  List<string> pathList = new List<string>();
      public  List<int> nLyricsINT = new List<int>();
      public  List<string> nLyricsSTR = new List<string>();
        public static void RunVideo(string videopath)
        {
            frm.axWindowsMediaPlayer1.URL = videopath;

            string[] files =
    Directory.GetFiles(Application.StartupPath + "\\cfg", "*", SearchOption.AllDirectories);
         
            foreach (string item in files)
            {
               if(!item.EndsWith("gcnf.txt"))
                {
                    if (File.ReadAllLines(item).Length > 0)
                        //MessageBox.Show(File.ReadAllLines(item)[0].Replace("file-path:", "").ToString().Contains( videopath) ? "true" : "false"); 
                     if (File.ReadAllLines(item)[0].Replace("file-path:", "").ToString() .Contains(videopath))
                    {
                     frm. pathList.Add(item);                     
                    } 
                }
            }
        if (frm.pathList.Count == 0)
            {
               frm. label2.Visible = true;
                frm.checkBox2.Enabled = false;
            } else
            {
                frm.label2.Visible = false;
                frm.checkBox2.Enabled = true;
            }
        } 

        private void Form1_Load(object sender, EventArgs e)
        {
          
            this.MaximumSize = this.Size; 
            this.MinimumSize = this.Size;

            if (!Directory.Exists(Application.StartupPath + "\\music")) { Directory.CreateDirectory(Application.StartupPath + "\\music"); }
            if (!Directory.Exists(Application.StartupPath + "\\cfg")) { Directory.CreateDirectory(Application.StartupPath + "\\cfg"); } 
            if (!File.Exists(Application.StartupPath + "\\cfg\\gcnf.txt")) { File.CreateText(Application.StartupPath + "\\cfg\\gcnf.txt"); }

        }
        private void sendChatMessage(string message, bool toAll = false)
        {
            //Image img = null;

            //string txt = null;
            //if (Clipboard.ContainsImage() == true) { img = Clipboard.GetImage(); }
            //if (Clipboard.ContainsText() == true) { txt = Clipboard.GetText(); }

            Clipboard.SetText(message);
            SendKeys.Send("{ENTER}");
            SendKeys.Send("^(v)");
            SendKeys.Send("{ENTER}");
            //System.Threading.Thread.Sleep(10);
            //SendKeys.Send("{ESCAPE}");

            //if (img != null) { Clipboard.SetImage(img); }
            //if (txt != null) { Clipboard.SetText(txt); }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            SelectMusic frm = new SelectMusic();
            frm.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\music");
        }

        private void label6_Click(object sender, EventArgs e)
        {
        
        }

        int pauseState = 0;
        KeyboardHook keyboardHook = new KeyboardHook();
        private void keyboardHook_KeyDown(KeyboardHook.VKeys key)
        { 
         if (key.ToString() == PUkey)
            {
             if (pauseState == 0)
                {
                    pauseState++;
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    if (checkBox1.Checked == true) {/* sendChatMessage("Music Paused"); SendKeys.Send("U"); System.Threading.Thread.Sleep(300);*/ Releasekey((byte)Keys.U);  }
                }    else 
                {
                    pauseState--;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    if (checkBox1.Checked == true) { /*sendChatMessage("Music Unpaused"); SendKeys.Send("U");  System.Threading.Thread.Sleep(300); */ HoldKey((byte)Keys.U);   }
                }  
            }
            System.Threading.Thread.Sleep(10);
        }

 

        KeyboardHook keyboardHook2 = new KeyboardHook();
        private void keyboardHook2_KeyDown(KeyboardHook.VKeys key)
        {

            if (key.ToString() != "ESCAPE")
            {
                label5.Text = key.ToString();
                PUkey = key.ToString(); 
                keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
                keyboardHook.Install();
            }
            keyboardHook2.KeyDown -= new KeyboardHook.KeyboardHookCallback(keyboardHook2_KeyDown);
            keyboardHook2.Uninstall();
        }
  

        private void label4_Click(object sender, EventArgs e)
        {
            keyboardHook2.KeyDown += new KeyboardHook.KeyboardHookCallback(keyboardHook2_KeyDown);
            keyboardHook2.Install();
        }

       

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const int KEY_DOWN_EVENT = 0x0001; 
        const int KEY_UP_EVENT = 0x0002; 

        public static void HoldKey(byte key)
        { 
            keybd_event(key, 0, KEY_DOWN_EVENT, 0);
        }
        public static void Releasekey(byte key)
        {
            keybd_event(key, 0, KEY_UP_EVENT, 0);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                nLyricsINT.Clear(); 
                label3.Visible = true;
                timer1.Start();
                string[] lyricsLines = File.ReadAllLines(pathList[0]);
                for (int i = 0; i <= lyricsLines.Length - 1; i++)
                {
                    if (lyricsLines[i].Contains("^"))
                    {
                        char[] chSp = "^".ToCharArray();
                       //MessageBox.Show(lyricsLines[i].Split(chSp)[0]);
                        nLyricsINT.Add(int.Parse(lyricsLines[i].Split(chSp)[0]));
                    }
                }
            } else { timer1.Stop(); label3.Visible = false; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

           int currentPos = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
          
            string[] lyricsLines = File.ReadAllLines(pathList[0]);
            for (int i = 0; i <= lyricsLines.Length - 1; i++)
            {
                if (lyricsLines[i].Contains("^"))
                {
                    char[] chSp = "^".ToCharArray();
                    if (currentPos == int.Parse(lyricsLines[i].Split(chSp)[0]))
                    {
                        sendChatMessage(lyricsLines[i].Split(chSp)[1]);
                        if (checkBox1.Checked == true) { System.Threading.Thread.Sleep(150); SendKeys.Send("U"); HoldKey((byte)Keys.U); }
                    }
           
                }
            }
         
        }

         
        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Text = "VMP - Valorant Music Player" + "  (" + Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition).ToString() + ") (" + axWindowsMediaPlayer1.Ctlcontrols.currentPositionString + ")";

            int currentPos = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
      if (checkBox2.Checked == true && nLyricsINT.Count > 0)
            {
                label3.Visible = true;
                if (nLyricsINT.Min() > currentPos)
                {
                    label3.Text = "Next: " + (((int)nLyricsINT.Min() - (int)currentPos) - 1) + "s";

                }
                else
                {
                    nLyricsINT.Remove(nLyricsINT.Min()); 
                }
            }   
       
        } 
    }
}
