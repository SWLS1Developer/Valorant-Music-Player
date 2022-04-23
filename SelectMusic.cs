 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMP___Valorant_Music_Player
{
    public partial class SelectMusic : Form
    {
        public SelectMusic()
        {
            InitializeComponent();
        }

        private void SelectMusic_Load(object sender, EventArgs e)
        {
            string[] files =
    Directory.GetFiles(Application.StartupPath + "\\music", "*", SearchOption.AllDirectories);
            foreach (string item in files)
            {
                var lvItem = listView1.Items.Add(item.Replace("/", "\\").Split("\\".ToCharArray())[item.Replace("/", "\\").Split("\\".ToCharArray()).Length - 1]);
                lvItem.SubItems.Add(item);
            }
        }

        private void lv_DoubleClick(object sender, EventArgs e)
        {
           if(listView1.SelectedItems.Count > 0)
            {
                 if (listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".mp4") || listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".mp3"))
                {
                    Form1.RunVideo(listView1.SelectedItems[0].SubItems[1].Text);                  
                    this.Close();
                }
            } 
        }
 
    }
} 