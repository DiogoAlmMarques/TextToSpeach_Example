
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.TextToSpeech.v1;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections.Specialized;

namespace TextToSpeach
{
    public partial class IBMForm : Form
    {
        //vai buscar a autenticação da api á app.config 
        string key = ConfigurationManager.AppSettings.Get("API_KEY");
        string uRL = ConfigurationManager.AppSettings.Get("API_URL");
        public IBMForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {

                //Inicia a ligação á api e add as vozes disponiveis á combobox
                IamAuthenticator authenticator = new IamAuthenticator(apikey: key);

                TextToSpeechService textToSpeech = new TextToSpeechService(authenticator);
                textToSpeech.SetServiceUrl(uRL);

                var result = textToSpeech.ListVoices();

                foreach (var voice in result.Result._Voices)
                {
                    comboBox1.Items.Add(voice.Name);
                }

                //add com default a voz pt á combobox
                comboBox1.SelectedText = "pt-BR_IsabelaV3Voice";


            }
            catch (Exception)
            {

                MessageBox.Show("Escolha uma voz");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {

                this.timer1.Start();
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "mp3 Files|*.mp3";
                string codigo64 = "";

                //converte o texto para base 64
                codigo64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text.ToLower()));

                String smallString;


                //verifica o tamanho do codigo64 é menor que 250 char e se não for corta a string 
                if (codigo64.Length > 250) smallString = codigo64.Substring(0, 250);
                else smallString = codigo64; 

                
                string Value = ".\\Resources\\" + smallString.ToString() + ".mp3";
                  if (File.Exists(Value) == true) {
                       this.progressBar1.Value = 100;
                       MessageBox.Show("Este conversão já foi feita está disponivel em " + Value); }

                  else{
                        using (FileStream fs = File.OpenWrite(Value)){

                        fileDialog.FileName = Value;
                        string fname = fileDialog.FileName;
                        //MessageBox.Show(fname);
                       
                        IamAuthenticator authenticator = new IamAuthenticator(apikey: key);
                        TextToSpeechService textToSpeech = new TextToSpeechService(authenticator);
                        
                        textToSpeech.SetServiceUrl(uRL);

                        var result = textToSpeech.Synthesize
                        (
                           text: textBox1.Text,
                           accept: Constants.ACCEPT,
                           voice: comboBox1.Text
                        );

                        result.Result.WriteTo(fs);
                        fs.Close();
                        result.Result.Close();
                        this.progressBar1.Value = 100;
                        MessageBox.Show("Convertido com sucesso" + Value); }             
                }
            }
            catch (Exception)
            {
              MessageBox.Show("Por favor escolha um nome para o ficheiro");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.progressBar1.Increment(1);
        }

        
    }
}
