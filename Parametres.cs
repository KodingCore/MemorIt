using System;
using System.IO;
using System.Windows.Forms;

namespace MemorIt_by_IMLL
{

    public partial class Parametres : Form
    {

        public Parametres()
        {
            InitializeComponent();
        }

        private void Parametres_Load(object sender, EventArgs e) //Chargement des sonneries
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Sonneries\\", "*.mp3");
            for (int i = 0; i < files.Length; i++)
            {
                int lastDotIndex = files[i].LastIndexOf('\\');
                string fileName = files[i].Substring(lastDotIndex + 1);
                if(fileName != "BasicSon.mp3")
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //AJOUTER UNE SONNERIE (LA COPIE DANS LE REPERTOIRE "Sonneries" DU PROGRAMME)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int lastDotIndex = openFileDialog1.FileName.LastIndexOf('.');
                string extension = openFileDialog1.FileName.Substring(lastDotIndex + 1);

                if (extension == "mp3")
                {
                    listBox1.Items.Add(openFileDialog1.SafeFileName);
                    File.Copy(openFileDialog1.FileName, Application.StartupPath + "\\Sonneries\\" + openFileDialog1.SafeFileName);
                }
                else
                {
                    MessageBox.Show("Seul les fichier MP3 sont compatibles.");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e) //ACCES AU REPERTOIRE "Sonneries" DU PROGRAMME
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\Sonneries\\");
        }

        private void button2_Click(object sender, EventArgs e) //SUPPRESSION DE LA SONNERIE SELECTIONNEE DANS LISTBOX1
        {
            String sonnerieSelected = listBox1.Text;
            DialogResult dialogResult = MessageBox.Show("Voulez-vous vraiment supprimer " + listBox1.Text + "?", "Suppression...", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(Application.StartupPath + "\\Sonneries\\" + listBox1.Text);
                listBox1.Items.Remove(listBox1.Text);
            }
        }

    }
}
