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

namespace Étterem_rendelés
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream f = new FileStream("jelszavak.txt", FileMode.Open);
            StreamReader sr = new StreamReader(f);

            bool sikeres = false;

            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(';');
                string name = sor[0];
                string password = sor[1];

                if (textBox1.Text == name && textBox2.Text == password)
                {
                    sikeres = true;
                    MessageBox.Show("Sikeres bejelentkezés!");

                    //form4

                    Form4 form4 = new Form4();
                    this.Hide();
                    form4.ShowDialog();
                    this.Close();
                }
            }

            sr.Close();
            f.Close();


            if(!sikeres)
            {
                MessageBox.Show("Sikertelen bejelentkezés!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
