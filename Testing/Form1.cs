/*
 * Authors: Zach Wharton & Erik Thomas
 * Creation Date: 1/12/2017
 * TODO:
 *      Change if/else if to switch
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CW2_Main
{
    public partial class Form1 : Form
    {
        MainWindow back;
        System.Windows.Controls.Button a;
        public Form1(MainWindow back, System.Windows.Controls.Button rename, String buttonCheck)
        {
            InitializeComponent();
            this.back = back;
            this.a = rename;
            //change to work for var
            //EDITED
            if (back.buttonCheck == "import")
            {
                textBox1.Text = back.btn_import.Content.ToString();
            } else if (back.buttonCheck == "var")
            {
                textBox1.Text = back.btn_variable.Content.ToString();
            }    //Insert checks for other statements here, change to switch
        }

        private void button1_Click(object sender, EventArgs e)
        {
            back.explorerString = textBox1.Text;
            a.Content = textBox1.Text;
            back.windowShown = false;
            back.hasSelected = true;
            this.Close();
        }

        private void exitWindow(object sender, MouseEventArgs e)
        {
            //Closes the window on pressing 'decline'
            //EDITEd
            this.Close();
        }
    }
}