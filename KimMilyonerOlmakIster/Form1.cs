using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace KimMilyonerOlmakIster
{

    public partial class FrmMenu : System.Windows.Forms.Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void button1_Resize(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmOyunEkrani frmOyunEkrani = new FrmOyunEkrani();
            frmOyunEkrani.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NasilOynanir nasilOynanir = new NasilOynanir();
            nasilOynanir.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
