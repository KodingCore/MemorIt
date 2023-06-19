using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MemorIt_by_IMLL
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon1; //Icone de notification
        private ContextMenu contextMenu1; //Bulle de notification
        private MenuItem menuItem1; //Menu de notification
        private IContainer component; //Composant de notification

        public static string DescriptionSonnerie = null;
        public static string NameSonnerie = null;
        public List<string> descriptions = new List<string>(); // Liste de chaques descriptions de mémoire
        public List<string> BanqueMemoires = new List<string>();
        public List<string> ChaqueParametresDeLaMemoire = new List<string>();
        public List<string> ChaqueParametresDeLaMemoire2 = new List<string>();
        string[] sonneries = new string[15]; // Tableau des sonneries
        bool sonnerieActive = true;

        string nomSaveTherme = "SaveMemsLong.mems"; // Nom du fichier de sauvegarde utilisé

        public Form1()
        {
            /*
             * 
             * 
             * Initialisation de l'activité de notification
             * 
             * 
             */
            this.component = new Container();
            this.contextMenu1 = new ContextMenu();
            this.menuItem1 = new MenuItem();
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.menuItem1 });
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);
            this.ClientSize = new Size(160, 28);
            this.Text = "MemorIt\nLWP";
            this.notifyIcon1 = new NotifyIcon(this.component);
            notifyIcon1.Icon = new Icon(Application.StartupPath + "\\MemoritICO.ico");
            notifyIcon1.ContextMenu = this.contextMenu1;
            notifyIcon1.Text = "MemorIt\nLWP";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += new EventHandler(this.notifyIcon1_DoubleClick);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) // CHARGEMENT DU FORMULAIRE PRINCIPAL
        {
            this.Visible = false; //Visibilité du form nulle
            dateTimePicker1.MinDate = DateTime.Now; //Date minimal possible à aujourd'hui
            TimerControl(60000); //Lancement du timer de control toutes les 60 secondes (en ms)
            InitialisationMemorielle(); //Initialisation de la mémoire
            RefreshSonneriesForm1();  
        }

        private void InitialisationMemorielle() //Initialisation de la mémoire // Accès au fichier de sauvegarde
        {
            bool exist = false; //Le fihcier de sauvegarde existe-t-il?
            if (File.Exists(Application.StartupPath + "\\" + nomSaveTherme)) //Si oui
            {
                exist = true;
            }
            if (exist)
            {
                ChargerLesMemoires(); // On charge la mémoire
            }
        }

        private void Form1_Resize(object sender, EventArgs e) //Si l'on redimentionne la form
        {
            if (WindowState == FormWindowState.Minimized) //Si l'on redimentionne la form au minimum
            {
                this.Visible= false; //Alors on la rend invisible
            }
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e) //Si l'on double-click sur l'icone de notification
        {
            this.Visible = true; // Alors on rend la form visible
        }

        private void menuItem1_Click(object Sender, EventArgs e) // Si l'on click sur le bouton exit de la notification
        {
            this.Close(); //Ferme l'application
        }

        public void msg(string message) // Methode messageBox raccourcis pour le debug
        {
            MessageBox.Show(message);
        }

        public bool yesno(string message) // Methode raccourcis pour le yes/no dialog debug
        {
            bool result = false;
            DialogResult dialogResult = MessageBox.Show(message, "Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                result = true;
            }
            else if (dialogResult == DialogResult.No)
            {
                result = false;
            }
            return result;
        }

        /*
         * 
         * 
         * A chaques listbox on Reforme les couleurs des textes
         * 
         * 
         * 
         * */
        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            e.Graphics.DrawString(listBox2.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
        }
        private void listBox3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            e.Graphics.DrawString(listBox3.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
        }
        private void listBox4_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            e.Graphics.DrawString(listBox4.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
        }
        private void listBox5_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            e.Graphics.DrawString(listBox5.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
        }

        private void button1_Click(object sender, EventArgs e) // BOUTON CREER LA MEMOIRE
        {
            String sonnerie = "None"; //De base la sonnerie est sur None
            DateTime datelu = dateTimePicker1.Value; //Date lu sur le dateTimePicker1
            String date = datelu.ToString();
            DateTime timelu = dateTimePicker2.Value;
            TimeSpan heureLu = dateTimePicker2.Value.TimeOfDay;
            String time = timelu.TimeOfDay.ToString();
            String nom = textBox1.Text;
            RadioButton radyo = FindingRadioButton(listBox1.Items.Count+1);
            bool heureValide = false;
            if (listBox2.Items.Count < 13)
            {
                if (datelu.DayOfYear >= DateTime.Today.DayOfYear) //Si la date est valide
                {
                    if(datelu.DayOfYear == DateTime.Today.DayOfYear)
                    {
                        if (heureLu > DateTime.Now.TimeOfDay) //Si l'heure est valide
                        {
                            heureValide= true;
                        }
                    }
                    else
                    {
                        heureValide = true;
                    }
                    if (heureValide == true)
                    {
                        if (textBox1.Text.Length > 0) // si la mémoire comporte un nom
                        {
                            if (sonnerieActive) // si la sonnerie est  active
                            {
                                if (domainUpDown1.Text == null) //Si la liste des choix de sonnerie est vide
                                {
                                    msg("Vous devez choisir une sonnerie.");
                                    return;
                                }
                                else //Si la liste des choix de sonnerie n'est pas vide
                                {
                                    if (File.Exists(Application.StartupPath + "\\Sonneries\\" + domainUpDown1.Text))
                                    {
                                        sonnerie = domainUpDown1.Text; // On defini un String sonnerie de la valeur du choix
                                        listBox4.Items.Add(sonnerie); // On ajoute le text de la sonnerie dans la listBox4
                                    }
                                    else
                                    {
                                        msg("La sonnerie choisie n'existe pas.\nPas de sonnerie parametrée.");
                                        listBox4.Items.Add("Pas de sonnerie");
                                    }
                                }
                            }
                            else
                            {
                                listBox4.Items.Add("Pas de sonnerie");
                            }
                            if (checkBox1.Checked) // Si la Checkbox sonnerie intervalle est checked
                            {
                                radyo.Checked = checkBox1.Checked; // On check le "radioButton"
                            }
                            listBox1.Items.Add(nom);
                            date = date.Substring(0, 10);
                            listBox2.Items.Add(date);
                            listBox3.Items.Add(time);
                            listBox5.Items.Add(numericUpDown1.Value); // On ajoute la veleur intervalle à listBox5
                            descriptions.Add(richTextBox1.Text); // On ajoute la description à la liste de String "descriptions"
                            BanqueMemoires.Add(nom + "%OI%" + date + "%OI%" + time + "%OI%" + sonnerie + "%OI%" + numericUpDown1.Value.ToString() + "%OI%" + checkBox1.Checked.ToString() + "%OI%" + richTextBox1.Text);
                        }
                        else // Pas de nom
                        {
                            msg("Vous devez donner un nom à votre mémoire.");
                        }
                    }
                    else // Heure Erronnée
                    {
                        msg("L'heure indiquer est erronnée (voir l'heure actuelle)");
                    }
                }
                else // Date dépassée
                {
                    msg("La date indiquée est passée.");
                }
            }
            else // Mémoires max
            {
                msg("Nombre maximum de mémoires atteint.");
            }
            
        }

        private void RefreshSonneriesForm1()
        {
            sonneries = Sonns.Listsonneries();
            int lastSlashIndex;
            string fileName;
            domainUpDown1.Items.Clear();
            for (int i = 0; i < sonneries.Length; i++)
            {
                if (sonneries[i] != null)
                {
                    lastSlashIndex = sonneries[i].LastIndexOf('\\');
                    fileName = sonneries[i].Substring(lastSlashIndex + 1);
                    sonneries[i] = fileName;
                    domainUpDown1.Items.Add(fileName);
                }
            }
        }

        private RadioButton FindingRadioButton(int count)
        {
            if (count > 0)
            {
                String nameRadioButton = "radioButton" + (count).ToString();
                nameRadioButton = nameRadioButton.Trim('-');
                Control[] ListeRadioButton = Controls.Find(nameRadioButton, false);
                RadioButton radyo = ListeRadioButton[0] as RadioButton;
                return radyo;
            }
            return null;
        }

        private void button2_Click(object sender, EventArgs e) // BOUTON OUVERTURE DES PARAMETRES
        {
            Form paramForm = new Parametres();
            if (paramForm.ShowDialog() == DialogResult.OK) { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) // Active/désactive las sonnerie
        {
            if (!checkBox2.Checked)
            {
                domainUpDown1.Enabled= false;
                sonnerieActive = false;
            }
            else
            {
                domainUpDown1.Enabled= true;
                sonnerieActive = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)//Suppression de memoire dans les listboxes
        {
            RadioButton radyo;
            List<bool> checksRadyos = new List<bool>();
            if (yesno("Voulez-vous vraiment supprimer " + listBox1.Text + "?") && listBox1.SelectedIndex >= 0)
            {
                BanqueMemoires.RemoveAt(listBox1.SelectedIndex);
                listBox2.Items.RemoveAt(listBox1.SelectedIndex); 
                listBox3.Items.RemoveAt(listBox1.SelectedIndex); 
                listBox4.Items.RemoveAt(listBox1.SelectedIndex); 
                listBox5.Items.RemoveAt(listBox1.SelectedIndex);
                for (int i = 1; i < 14; i++) // Recolte de toutes les valeurs radios
                {
                    radyo = FindingRadioButton(i);
                    if(i != listBox1.SelectedIndex + 1)
                    {
                        checksRadyos.Add((bool)radyo.Checked);
                    }
                }
                checksRadyos.Add(false);
                for (int i = 1; i < 14; i++) // Reéécriture de toutes les valeurs radios
                {
                    radyo = FindingRadioButton(i);
                    radyo.Checked = checksRadyos[i-1];
                }
                descriptions.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex); 
                if(listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    richTextBox2.Text = "";
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.SelectedIndex = listBox1.SelectedIndex;
            listBox3.SelectedIndex = listBox1.SelectedIndex;
            listBox4.SelectedIndex = listBox1.SelectedIndex;
            listBox5.SelectedIndex = listBox1.SelectedIndex;
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex <= descriptions.Count-1)
            {
                richTextBox2.Clear();
                richTextBox2.Text = descriptions[listBox1.SelectedIndex];
            }
        }

        private void button7_Click(object sender, EventArgs e) //Bouton Sauvegarde memoires
        {
            SaveMemoires();
        }

        private void SaveMemoires()
        {
            using (StreamWriter sw = new StreamWriter(Application.StartupPath + "\\" + nomSaveTherme))
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    sw.Write(listBox1.Items[i].ToString() + "%OI%");
                    sw.Write(listBox2.Items[i].ToString() + "%OI%");
                    sw.Write(listBox3.Items[i].ToString() + "%OI%");
                    sw.Write(listBox4.Items[i].ToString() + "%OI%");
                    sw.Write(listBox5.Items[i].ToString() + "%OI%");
                    sw.Write(FindingRadioButton(i + 1).Checked.ToString() + "%OI%");
                    string formatSaveDescription = descriptions[i].ToString();
                    sw.WriteLine(formatSaveDescription);
                }
            }
        }

        private void ChargerLesMemoires()
        {
            string chemin = Application.StartupPath + "\\" + nomSaveTherme;
            string[] lines = File.ReadAllLines(chemin);
            string[] separator = new string[1];
            separator[0] = "%OI%";
            foreach (string line in lines)
            {
                if (!BanqueMemoires.Contains(line)) // Empeche l'enrégistrement de deux mémoires similaires
                {
                    BanqueMemoires.Add(line);
                }
            }
            if (BanqueMemoires.Count.ToString() != "0")
            {
                for (int i = 0; i < BanqueMemoires.Count; i++)
                {
                    ChaqueParametresDeLaMemoire = BanqueMemoires[i].Split(separator, StringSplitOptions.None).ToList();
                    listBox1.Items.Add(ChaqueParametresDeLaMemoire[0]); //NOM
                    listBox2.Items.Add(ChaqueParametresDeLaMemoire[1]); //DATE
                    listBox3.Items.Add(ChaqueParametresDeLaMemoire[2]); //HEURE
                    listBox4.Items.Add(ChaqueParametresDeLaMemoire[3]); //SONNERIE
                    listBox5.Items.Add(ChaqueParametresDeLaMemoire[4]); //JAV
                    if (ChaqueParametresDeLaMemoire[5].ToString() == "True") //PRE-SONNERIE RADIOBUTTON
                    {  
                        FindingRadioButton(i + 1).Checked = true;
                    }
                    else
                    {
                        FindingRadioButton(i + 1).Checked = false;
                    }
                    descriptions.Add(ChaqueParametresDeLaMemoire[6]); //DESCRIPTION  
                }
            }   
        }

        private Timer timerTest; //Déclaration de notre composant timer
        public void TimerControl(int delay)
        {
            timerTest = new Timer();
            timerTest.Interval = delay;
            timerTest.Tick += TimerTest_Tick; //Abonnement à l'événement Tick du timer
            timerTest.Start(); //Activation du timer
        }

        private void TimerTest_Tick(object sender, EventArgs e)
        {
            
            LectureDesDatesEtPreSonneries();
        }

        private void LectureDesDatesEtPreSonneries()
        {
            DateTimeOffset dateActuel;
            DateTimeOffset dateLu;
            DateTimeOffset dateHeureLu;
            TimeSpan heureLu;
            bool dateHeureEnPhase;
            int JAV;
            string[] separator = new string[1];

            if (BanqueMemoires.Count.ToString() != "0")
            {
                
                separator[0] = "%OI%";
                for (int i = 0; i < BanqueMemoires.Count; i++)
                {
                    dateActuel = DateTime.Now;
                    dateHeureEnPhase = false;
                    ChaqueParametresDeLaMemoire = BanqueMemoires[i].Split(separator, StringSplitOptions.None).ToList();
                    dateLu = DateTime.Parse(ChaqueParametresDeLaMemoire[1]);
                    dateHeureLu = DateTime.Parse(ChaqueParametresDeLaMemoire[2]);
                    heureLu = dateHeureLu.TimeOfDay;
                    JAV = Convert.ToInt32(ChaqueParametresDeLaMemoire[4]);
                    
                    if (ChaqueParametresDeLaMemoire[5].ToString() == "True") //TOUTLESJOURS RADIOBUTTON
                    {
                        for (int j = 0; j <= JAV; j++) // CONTROL DATE
                        {
                            if (dateActuel.DayOfYear == dateLu.DayOfYear)
                            {
                                
                                dateHeureEnPhase = true;
                            }
                            dateLu = dateLu.AddDays(-1);
                        }

                        dateActuel = DateTime.Now;
                        if (dateHeureEnPhase) // CONTROL HEURE
                        {
                            if (heureLu.TotalMinutes == Math.Floor(dateActuel.TimeOfDay.TotalMinutes))
                            {
                                dateHeureEnPhase = true;
                            }
                            else
                            {
                                dateHeureEnPhase = false;
                            }
                        }
                    }
                    else
                    {
                        for (int j = JAV; j >= 0; j--) // CONTROL DATE
                        {
                            dateActuel.AddDays(-j);
                            if (dateActuel.DayOfYear == dateLu.DayOfYear)
                            {
                                 dateHeureEnPhase = true;
                            }
                        }
                        dateActuel = DateTime.Now;
                        if (dateHeureEnPhase) // CONTROL HEURE
                        {
                            if (heureLu.TotalMinutes == Math.Floor(dateActuel.TimeOfDay.TotalMinutes))
                            {
                                dateHeureEnPhase = true;
                            }
                            else
                            {
                                dateHeureEnPhase = false;
                            }
                        }
                    }
                    if (dateHeureEnPhase) // LA SONNERIE VA SE LANCER
                    {
                        DescriptionSonnerie = ChaqueParametresDeLaMemoire[6];
                        NameSonnerie = ChaqueParametresDeLaMemoire[3];
                        Form AlarmForm = new Alarm();
                        AlarmForm.Text = ChaqueParametresDeLaMemoire[0];
                        if (AlarmForm.ShowDialog() == DialogResult.OK) { }
                    }
                }
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            RefreshSonneriesForm1();
        }
        
    }
}
