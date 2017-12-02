using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
namespace DragDrop
{
    public partial class Form1 : Form
    {
        MainWindow back;
        System.Windows.Controls.Button a;
        public Form1(MainWindow back, System.Windows.Controls.Button rename)
        {
            InitializeComponent();
            this.back = back;
            this.a = rename;
            textBox1.Text = back.btn_import.Content.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            back.explorerString = textBox1.Text;
            a.Content = textBox1.Text;
            this.Close();
        }
    }
}
