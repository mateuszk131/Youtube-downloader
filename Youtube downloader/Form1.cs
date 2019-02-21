using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;
using System;
using System.IO;
using System.Windows.Forms;

namespace Youtube_downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            buttonStart.Text = GUI.OpisElementowGUI.ButtonStart;
            buttonTrimmer.Text = GUI.OpisElementowGUI.ButtonTrimmer;
            buttonMP3.Text = GUI.OpisElementowGUI.ButtonMP3;

        }
        
        string link = "";
        string sciezka = "";
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("youtube"))
            {


                link = textBox1.Text;
                var source = label1.Text;
                var youtube = YouTube.Default;
                var vid = youtube.GetVideo(link);
                sciezka = source + vid.FullName.Substring(0, vid.FullName.Length - 14) + ".mp4";
                File.WriteAllBytes(sciezka, vid.GetBytes());
                axWindowsMediaPlayer1.URL = sciezka;
                buttonMP3.Enabled = true;
            }
            else
            {
                MessageBox.Show("Nie wlasciwy link");
            }
        }
        private void buttonTrimmer_Click(object sender, EventArgs e)
        {
            Trimmer trimmer = new Trimmer();
            trimmer.Show();
        }
        private void buttonMP3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            var youtube = YouTube.Default;
            var vid = youtube.GetVideo(link);
            //File.Copy(source+vid.FullName,source+"copy "+vid.FullName);
            var inputFile = new MediaFile { Filename = sciezka };
            //sciezka = sciezka.Substring(0, sciezka.Length - 4);
            var outputFile = new MediaFile { Filename = $"{sciezka.Substring(0, sciezka.Length - 4)}.mp3" };
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                engine.Convert(inputFile, outputFile);
            }
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            using (var fldrDlg = new FolderBrowserDialog())
            {


                if (fldrDlg.ShowDialog() == DialogResult.OK)
                {
                    buttonStart.Enabled = true;
                    label1.Text = fldrDlg.SelectedPath;
                }
            }
        }
    }
}
    
