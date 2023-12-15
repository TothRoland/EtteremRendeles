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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        #region missclick
        private void Label1_Click(object sender, EventArgs e)
        {

        }
        private void Label4_Click(object sender, EventArgs e)
        {

        }
        #endregion

        public static List<adatok> lista = new List<adatok>();
        public static List<string> elfogyott = new List<string>();
        public static List<adatok> vasarlasLista = new List<adatok>();

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void beolvas()
        {
            lista.Clear();
            elfogyott.Clear();

            FileStream f = new FileStream($"arucikkek.txt", FileMode.Open);
            StreamReader sr = new StreamReader(f);

            string sor = sr.ReadLine();
            while (sor != "-")
            {
                string[] seged = sor.Split(';');
                string nev = seged[0];
                int menny = int.Parse(seged[1]);
                int ar = int.Parse(seged[2]);
                adatok s = new adatok(nev, menny, ar);
                lista.Add(s);
                sor = sr.ReadLine();
            }

            while (!sr.EndOfStream)
            {
                elfogyott.Add(sr.ReadLine());
            }

            sr.Close();
            f.Close();

            for (int i = 0; i < lista.Count; i++)
            {
                comboBox1.Items.Add($"{lista[i].nev}");
                comboBox2.Items.Add($"{lista[i].nev}");
            }

            for (int i = 0; i < elfogyott.Count; i++)
            {
                comboBox1.Items.Add($"{elfogyott[i].Split(';')[0]}");
                comboBox2.Items.Add($"{elfogyott[i].Split(';')[0]}");
            }
        }

        public void kiir()
        {
            FileStream f = new FileStream($"arucikkek.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(f);

            for (int i = 0; i < lista.Count; i++)
            {
                sw.WriteLine($"{lista[i].nev};{lista[i].mennyiseg};{lista[i].ar}");
            }

            sw.WriteLine("-");

            for (int i = 0; i < elfogyott.Count; i++)
            {
                sw.WriteLine(elfogyott[i]);
            }

            sw.Close();
            f.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            beolvas();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                beolvas();
                if (comboBox1.SelectedIndex != -1 && textBox1.Text != "")
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if(lista[i].nev == comboBox1.SelectedItem.ToString())
                        {
                            lista[i].mennyiseg += int.Parse(textBox1.Text);
                            kiir();
                            MessageBox.Show("Megtörtént az árufeltöltés!");
                            return;
                        }
                    }
                    for (int i = 0; i < elfogyott.Count; i++)
                    {
                        if (elfogyott[i].Split(';')[0] == comboBox1.SelectedItem.ToString())
                        {
                            lista.Add(new adatok(elfogyott[i].Split(';')[0], int.Parse(textBox1.Text), int.Parse(elfogyott[i].Split(';')[1])));
                            elfogyott.RemoveAt(i);
                            kiir();
                            MessageBox.Show("Megtörtént az árufeltöltés!");
                            return;
                        }
                    }
                } else
                {
                    if (comboBox1.SelectedIndex == -1)
                    {
                        MessageBox.Show("Nincs kiválasztott étel!");
                        comboBox1.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Nincs megadott darabszám!");
                        textBox1.Focus();
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                beolvas();
                if (comboBox2.SelectedIndex != -1 && textBox2.Text != "")
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].nev == comboBox2.SelectedItem.ToString())
                        {
                            lista[i].ar = int.Parse(textBox2.Text);
                            kiir();
                            MessageBox.Show("Megtörtént az árváltoztatás!");
                            return;
                        }
                    }
                    for (int i = 0; i < elfogyott.Count; i++)
                    {
                        if (elfogyott[i].Split(';')[0] == comboBox2.SelectedItem.ToString())
                        {
                            elfogyott[i] = elfogyott[i].Split(';')[0] + ";" + textBox2.Text;
                            kiir();
                            MessageBox.Show("Megtörtént az árváltoztatás!");
                            return;
                        }
                    }
                }
                else
                {
                    if (comboBox2.SelectedIndex == -1)
                    {
                        MessageBox.Show("Nincs kiválasztott étel!");
                        comboBox2.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Nincs megadott ár!");
                        textBox2.Focus();
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].nev == comboBox1.SelectedItem.ToString())
                {
                    textBox7.Text = lista[i].mennyiseg.ToString() + " db";
                    return;
                }
            }
            for (int i = 0; i < elfogyott.Count; i++)
            {
                if (elfogyott[i].Split(';')[0] == comboBox1.SelectedItem.ToString())
                {
                    textBox7.Text = "0 db";
                    return;
                }
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].nev == comboBox2.SelectedItem.ToString())
                {
                    textBox6.Text = lista[i].ar.ToString() + " Ft";
                    return;
                }
            }
            for (int i = 0; i < elfogyott.Count; i++)
            {
                if (elfogyott[i].Split(';')[0] == comboBox2.SelectedItem.ToString())
                {
                    textBox6.Text = elfogyott[i].Split(';')[1] + " Ft";
                    return;
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "" && textBox4.Text != "" && textBox4.Text != "")
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].nev == textBox3.Text)
                        {
                            MessageBox.Show("Ilyen árucikk már létezik!");
                            textBox3.Focus();
                            return;
                        }
                    }
                    for (int i = 0; i < elfogyott.Count; i++)
                    {
                        if (elfogyott[i].Split(';')[0] == textBox3.Text)
                        {
                            MessageBox.Show("Ilyen árucikk már létezik!");
                            textBox3.Focus();
                            return;
                        }
                    }

                    if (textBox4.Text == "0")
                    {
                        elfogyott.Add(textBox3.Text + ";" + textBox5.Text);
                    }
                    else
                    {
                        lista.Add(new adatok(textBox3.Text, int.Parse(textBox4.Text), int.Parse(textBox5.Text)));
                    }
                    kiir();
                    MessageBox.Show("Új árucikk felvétele megtörtént!");
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    return;
                }
                else
                {
                    if (textBox3.Text == "")
                    {
                        MessageBox.Show("Nincs megadott ételnév!");
                        textBox3.Focus();
                    }
                    else if (textBox4.Text == "")
                    {
                        MessageBox.Show("Nincs megadott darabszám!");
                        textBox4.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Nincs megadott ár!");
                        textBox5.Focus();
                    }
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
    }
}
