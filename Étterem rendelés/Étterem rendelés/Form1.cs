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

namespace Étterem_rendelés
{
    public partial class Form1 : Form
    {

        public static List<adatok> lista = new List<adatok>();
        public static List<string> elfogyott = new List<string>();
        public static List<adatok> vasarlasLista = new List<adatok>();
       
        public Form1()
        {
            InitializeComponent();
        }


        #region Program betöltése után
        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Text = "-->";
            button3.Text = "<--";
            button4.Text = "Tovább a fizetéshez";
            button4.Enabled = false;
            button5.Text = "Admin";

            beolvas();
        }
        #endregion

        #region txt beolvasás
        public void beolvas()
        {
            lista.Clear();
            elfogyott.Clear();
            vasarlasLista.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();

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
                listBox1.Items.Add($"{lista[i].nev} {lista[i].mennyiseg} db");
            }
        }
        #endregion

        #region txt-be kiírás
        public void kiir()
        {
            FileStream f = new FileStream($"arucikkek.txt", FileMode.Append);
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
        #endregion

        #region kosárba átadás
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int selected = listBox1.SelectedIndex;

                if (selected == -1)
                {
                    MessageBox.Show("Nincs kiválasztott elem!");
                    return;
                }

                List<string> kosarNev = new List<string>(); //A kosárban lévő ételek nevei
                for (int i = 0; i < vasarlasLista.Count; i++)
                {
                    kosarNev.Add(vasarlasLista[i].nev);
                }

                //Ha nincs még olyan nevű árucikk a kosarunkba amit a felhasználó szeretne hozzáadni akkor csak hozzáadjuk a vasarlasLista -hoz, ha már tartalmazza akkor a megadott árucikk darabszámát növeljük
                if (!kosarNev.Contains(lista[selected].nev))
                {
                    string nev = lista[selected].nev;
                    int db = 1;
                    int ar = lista[selected].ar;
                    adatok seged = new adatok(nev, db, ar);
                    vasarlasLista.Add(seged);
                    listBox2.Items.Add(vasarlasLista[vasarlasLista.Count - 1].nev + " " + vasarlasLista[vasarlasLista.Count - 1].mennyiseg + "db");
                }
                else
                {
                    int index = 0;

                    for (int i = 0; i < kosarNev.Count; i++)
                    {
                        if(kosarNev[i] == lista[selected].nev)
                        {
                            index = i;
                            break;
                        }
                    }

                    vasarlasLista[index].mennyiseg++;
                    listBox2.Items[index] = vasarlasLista[index].nev + " " + vasarlasLista[index].mennyiseg + "db";
                }

                lista[selected].mennyiseg--;

                if(lista[selected].mennyiseg == 0)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);
                    listBox1.ClearSelected();
                    elfogyott.Add(lista[selected].nev + ";" + lista[selected].ar);
                    lista.RemoveAt(selected);
                }
                else
                {
                    listBox1.Items[selected] = lista[selected].nev + " " + lista[selected].mennyiseg + "db";
                }

                button4.Enabled = true;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }
        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int selected = listBox2.SelectedIndex;

                if (selected == -1)
                {
                    MessageBox.Show("Nincs kiválasztott elem!");
                    return;
                }

                List<string> elF = new List<string>(); //Az elfogyott áruk nevei
                for (int i = 0; i < elfogyott.Count; i++)
                {
                    elF.Add(elfogyott[i].Split(';')[0]);
                }

                int index = 0;

                if (elF.Contains(vasarlasLista[selected].nev))
                {
                    string nev = vasarlasLista[selected].nev;
                    int db = 1;
                    int ar = vasarlasLista[selected].ar;
                    adatok seged = new adatok(nev, db, ar);
                    lista.Add(seged);

                    index = lista.Count - 1;

                    listBox1.Items.Add(lista[index].nev + " 1 db");

                    for (int i = 0; i < elfogyott.Count; i++)
                    {
                        if(elfogyott[i].Split(';')[0] == vasarlasLista[selected].nev)
                        {
                            elfogyott.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i].nev == vasarlasLista[selected].nev)
                        {
                            index = i;
                            break;
                        }
                    }

                    lista[index].mennyiseg++;
                    listBox1.Items[index] = lista[index].nev + " " + lista[index].mennyiseg + " db";
                }

                vasarlasLista[selected].mennyiseg--;

                if (vasarlasLista[selected].mennyiseg == 0)
                {
                    listBox2.Items.Remove(listBox2.SelectedItem);
                    listBox2.ClearSelected();

                    int rm = 0;

                    for (int i = 0; i < elfogyott.Count; i++)
                    {
                        if(elfogyott[i].Split(';')[0] == vasarlasLista[selected].nev)
                        {
                            rm = i;
                            break;
                        }
                    }

                    vasarlasLista.RemoveAt(selected);
                }
                else
                {
                    listBox2.Items[selected] = vasarlasLista[selected].nev + " " + vasarlasLista[selected].mennyiseg + "db";
                }

                if(vasarlasLista.Count == 0)
                {
                    button4.Enabled = false;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox2.ClearSelected();
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
            this.Show();

            button4.Enabled = false;
            beolvas();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox2.ClearSelected();
            Form3 form3 = new Form3();
            this.Hide();
            form3.ShowDialog();
            this.Show();

            beolvas();
        }

        #region Missclick :)
        private void Label1_Click(object sender, EventArgs e)
        {
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}