using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;


namespace Youtube_downloader
{
    public partial class Trimmer : Form
    {
        public Trimmer()
        {

            InitializeComponent();
            buttonLoad.Text = GUI.OpisElementowGUI.ButtonLoad;
            buttonTrim.Text = GUI.OpisElementowGUI.ButtonTrim;
        }
        double wynik1,wynik2;
        int hours1, hours2, minutes1, minutes2, seconds1, seconds2;

        string sciezka = "";

        string[] separatingChars = { ":" };
        void TrimMp3(string inputPath, string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");
            using (var reader = new Mp3FileReader(inputPath))
            using (var writer = File.Create(outputPath))
            {
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                    if (reader.CurrentTime >= begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                           writer.Write(frame.RawData, 0, frame.RawData.Length);
                        else break;
                    }
            }
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Music files|*.mp3";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sciezka = openFileDialog1.FileName;
                this.textBox1.Text = sciezka;
                Mp3FileReader reader = new Mp3FileReader(sciezka);
                TimeSpan duration = reader.TotalTime;
                string dura = duration.ToString();
                buttonTrim.Enabled = true;
                string koniec = dura.Substring(0, 8);
                textBox3.Text = koniec.ToString();



            }
           
        }
        private void buttonTrim_Click(object sender, EventArgs e)
        {
            var mp3Path = sciezka;
            var outputPath = Path.ChangeExtension(mp3Path, "trimmed.mp3");
            string start = textBox2.Text;

            string[] words1 = start.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            string[] words2 = textBox3.Text.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            hours1 = int.Parse(words1[0]);
            minutes1 = int.Parse(words1[1]);
            seconds1 = int.Parse(words1[2]);
            hours2 = int.Parse(words2[0]);
            minutes2 = int.Parse(words2[1]);
            seconds2 = int.Parse(words2[2]);

            wynik1 =( hours1 * 24 * 60)+(  minutes1 * 60) + seconds1;
            wynik2 = (hours2 * 24 * 60) + (minutes2 * 60) + seconds2;

            if (checkBox1.Checked && !checkBox2.Checked)
            {
                TrimMp3(mp3Path, outputPath, TimeSpan.FromSeconds(wynik1), TimeSpan.FromSeconds(wynik2));
            }
            if (!checkBox1.Checked && checkBox2.Checked)
            {
                TrimMp3(mp3Path, outputPath, TimeSpan.MinValue,TimeSpan.FromSeconds(wynik2));
            }
            if(checkBox1.Checked && checkBox2.Checked)
            {
                TrimMp3(mp3Path, outputPath, TimeSpan.FromSeconds(wynik1), TimeSpan.FromSeconds(wynik2));
            }
            if(!checkBox1.Checked && !checkBox2.Checked)
            {
                TrimMp3(mp3Path, outputPath, TimeSpan.MinValue, TimeSpan.MaxValue);
            }


        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox3.Enabled = true;

            }
            else
            {
                textBox3.Enabled = false;

            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
            }
        } 
    }
}

