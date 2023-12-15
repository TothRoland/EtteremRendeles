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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listBox1.Enabled = false;
            radioButton1.Checked = true;

            for (int i = 0; i < Form1.vasarlasLista.Count; i++)
            {
                listBox1.Items.Add($"{Form1.vasarlasLista[i].nev} {Form1.vasarlasLista[i].mennyiseg} db");
            }

            int sum = 0;
            for (int i = 0; i < Form1.vasarlasLista.Count; i++)
            {
                sum += Form1.vasarlasLista[i].ar * Form1.vasarlasLista[i].mennyiseg;
            }
            label2.Text += sum + " Ft";
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if(radioButton1.Checked)
            {
                int fizetendo = int.Parse(label2.Text.Split(' ')[2]);
                int fizetett = int.Parse(textBox1.Text);
                int hianyzik = fizetendo - fizetett;

                if (hianyzik > 0)
                {
                    MessageBox.Show("Keveset fizettél! Hátralévő összeg: " + hianyzik + " Ft");
                    label2.Text = "Hátralévő összeg: " + hianyzik + " Ft";
                    textBox1.Text = "";
                    textBox1.Focus();
                }
                else
                {
                    this.Close();
                    MessageBox.Show("Visszajáró: " + hianyzik * (-1) + " Ft\nKöszönjük a vásárlást!");
                    kiir();
                }
            }
            else
            {
                MessageBox.Show("Köszönjük a vásárlást!");
                this.Close();
                kiir();
            }

        }

        public void kiir()
        {
            FileStream f = new FileStream($"arucikkek.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(f);

            for (int i = 0; i < Form1.lista.Count; i++)
            {
                sw.WriteLine($"{Form1.lista[i].nev};{Form1.lista[i].mennyiseg};{Form1.lista[i].ar}");
            }

            sw.WriteLine("-");

            for (int i = 0; i < Form1.elfogyott.Count; i++)
            {
                sw.WriteLine(Form1.elfogyott[i]);
            }

            sw.Close();
            f.Close();
        }

        #region Missclick :)
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                textBox1.Enabled = true;
            } else
            {
                textBox1.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}