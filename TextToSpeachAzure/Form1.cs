using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;
using System.Threading;
using System.Configuration;
namespace TextToSpeachAzure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Text Files|*.txt";
                    openFileDialog.ShowDialog();
                    string fname = openFileDialog.FileName;

                    StreamReader streamReader = new StreamReader(fname);
                    textBox1.Text = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Por favor escolha um ficheiro .txt correto");
                }

            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                //Inicia a ligação á api e add as vozes disponiveis á combobox



                string[] vozes = { "pt-PT-FernandaNeural", "pt-PT-RaquelNeural", "pt-PT-DuarteNeural" };

                foreach (var voice in vozes)
                {
                    comboBox1.Items.Add(voice);
                }

                //add com default a voz pt á combobox
                comboBox1.SelectedText = "pt-PT-FernandaNeural";


            }
            catch (Exception)
            {

                MessageBox.Show("Escolha uma voz");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            this.timer1.Start();

            string codigo64 = "";

            //converte o texto para base 64
            codigo64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text.ToLower() + comboBox1.Text.ToLower()));

            String smallString = "";


            //verifica o tamanho do codigo64 é menor que 250 char e se não for corta a string 
            if (codigo64.Length > 220) smallString = codigo64.Substring(0, 220);
            else smallString = codigo64;


            string Value = ".\\Resources\\" + smallString.ToString() + ".mp3";
            if (File.Exists(Value) == true)
            {
                this.progressBar1.Value = 100;
                MessageBox.Show("Este conversão já foi feita está disponivel em " + Value);
                this.progressBar1.Value = 0;
            }

            else
            {

                    TextToSpeach api = new TextToSpeach();
                    api.inicializarAPI(textBox1.Text, comboBox1.Text, Value);
                    this.progressBar1.Value = 100;
                    MessageBox.Show("Convertido com sucesso");
                   
                    this.progressBar1.Value = 0;
      

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progressBar1.Increment(1);
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
