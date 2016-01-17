using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace CriptoLock
{
    public partial class CryptoLock : Form
    {
        bool State = true;

        TcpListener server;
        TcpClient client;
        //convertor ASCII <-> string
        ASCIIEncoding encoder;
        //thread acceptare conexiuni clienti
        //		Thread acceptThread, handleThread;

        //functie setare text afisaj - threadSafe
        delegate void setTextCallback(string text);


        public CryptoLock()
        {
            InitializeComponent();

            //obtine ip-ul local
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            //afiseaza ip-ul server-ului in text
            label18.Text = host.AddressList[1].ToString();

            string userInput = richTextBox1.Text;
            userInput = userInput.Trim();
            string[] wordCount = userInput.Split(null);

            int charCount = 0;
            int digitCount = 0;
            int upperCaseLettersCount = 0;
            int lowerCaseLettersCount = 0;
            int symbolsCount = 0;

            foreach (var word in wordCount)
            {
                foreach (char character in word)
                {
                    if ((int)character > 47 && (int)character < 58) digitCount++;
                    else if ((int)character > 64 && (int)character < 90) upperCaseLettersCount++;
                    else if ((int)character > 96 && (int)character < 123) lowerCaseLettersCount++;
                    else symbolsCount++;
                }
                charCount += word.Length;
            }

            if (charCount == 0) label3.Text = "0";
            else label3.Text = wordCount.Length.ToString();

            label8.Text = charCount.ToString();
            label9.Text = digitCount.ToString();
            label12.Text = upperCaseLettersCount.ToString();
            label13.Text = lowerCaseLettersCount.ToString();
            label14.Text = symbolsCount.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void Crypt()
        {
            if (State)
            {
                switch(label22.Text)
                { 
                    case "Encryption 1":
                        int x;
                        if (Math.Sqrt(richTextBox1.Text.Length) % 1 == 0)
                            x = (int)Math.Sqrt(richTextBox1.Text.Length);
                        else x = (int)Math.Sqrt(richTextBox1.Text.Length) + 1;

                        char[,] a = new char[x + 1, x + 1];

                        char[] c = new char[(int)Math.Pow(x, 2)];
                        char[] ex = new char[(int)Math.Pow(x, 2)];


                        for (int i = 0; i < c.Length; i++)
                            c[i] = '#';

                        for (int i = 0; i < richTextBox1.Text.Length; i++)
                        {
                            c[i] = richTextBox1.Text.ToCharArray()[i];
                        }

                        for (int i = 0; i < c.Length; i++)
                        {
                            if (c[i] == ' ') c[i] = '#';
                        }

                        int nr = -1;
                        for (int i = 1; i <= Math.Sqrt(c.Length); i++)
                            for (int j = 1; j <= Math.Sqrt(c.Length); j++) a[i, j] = c[++nr];

                        nr = -1;

                        for (int i = 1; i <= Math.Sqrt(c.Length); i++)
                            for (int j = 1; j <= Math.Sqrt(c.Length); j++) ex[++nr] = a[j, i];


                        string aux = new string(ex);
                        richTextBox2.Text = aux;
                    break;
                        
                    case "Encryption 2":
                        if(textBox3.Text != "")
                            richTextBox2.Text = XOREncryptionDecryption.XOREncryptionDecryption.Encode(richTextBox1.Text, textBox3.Text);
                    break;
                }
            }
            else
            {
                switch(label22.Text)
                {
                    case "Decryption 1":
                        int x;
                        if (Math.Sqrt(richTextBox1.Text.Length) % 1 == 0)
                            x = (int)Math.Sqrt(richTextBox1.Text.Length);
                        else x = (int)Math.Sqrt(richTextBox1.Text.Length) + 1;

                        char[,] a = new char[x + 1, x + 1];

                        char[] c = new char[(int)Math.Pow(x, 2)];
                        char[] ex = new char[(int)Math.Pow(x, 2)];


                        for (int i = 0; i < ex.Length; i++)
                            ex[i] = '#';

                        for (int i = 0; i < richTextBox1.Text.Length; i++)
                        {
                            ex[i] = richTextBox1.Text.ToCharArray()[i];
                        }

                        int nr = -1;
                        for (int i = 1; i <= Math.Sqrt(ex.Length); i++)
                            for (int j = 1; j <= Math.Sqrt(ex.Length); j++) a[i, j] = ex[++nr];

                        nr = -1;

                        for (int i = 1; i <= Math.Sqrt(ex.Length); i++)
                            for (int j = 1; j <= Math.Sqrt(ex.Length); j++)
                            {
                                if (a[j, i] == '#') c[++nr] = ' ';
                                else c[++nr] = a[j, i];
                            }

                        string aux = new string(c);
                        richTextBox2.Text = aux;
                    break;

                    case "Decryption 2":
                        if (textBox3.Text != "")
                            richTextBox2.Text = XOREncryptionDecryption.XOREncryptionDecryption.Decode(richTextBox1.Text, textBox3.Text);
                    break;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string userInput = richTextBox1.Text;
            userInput = userInput.Trim();
            string[] wordCount = userInput.Split(null);

            int charCount = 0;
            int digitCount = 0;
            int upperCaseLettersCount = 0;
            int lowerCaseLettersCount = 0;
            int symbolsCount = 0;

            foreach (var word in wordCount)
            {
                foreach (char character in word)
                {
                    if ((int)character > 47 && (int)character < 58) digitCount++;
                    else if ((int)character > 64 && (int)character < 90) upperCaseLettersCount++;
                    else if ((int)character > 96 && (int)character < 123) lowerCaseLettersCount++;
                    else symbolsCount++;
                }
                charCount += word.Length;
            }

            if (charCount == 0)
            {
                button2.Enabled = false;
                label3.Text = "0";
            }
            else 
            {
                button2.Enabled = true;
                label3.Text = wordCount.Length.ToString();
            }

            label8.Text = charCount.ToString();
            label9.Text = digitCount.ToString();
            label12.Text = upperCaseLettersCount.ToString();
            label13.Text = lowerCaseLettersCount.ToString();
            label14.Text = symbolsCount.ToString();

            Crypt();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            State = !State;
            if (State)
            {
                label21.Text = "Encrypting";

                if (label22.Text == "Decryption 1") label22.Text = "Encryption 1";
                if (label22.Text == "Decryption 2") label22.Text = "Encryption 2";

                toolStripStatusLabel1.Text = "Changed to Encrypt";
            }
            else
            {
                label21.Text = "Decrypting";

                if (label22.Text == "Encryption 1") label22.Text = "Decryption 1";
                if (label22.Text == "Encryption 2") label22.Text = "Decryption 2";

                toolStripStatusLabel1.Text = "Changed to Decrypt";
            }
            
            if(label21.Text == "Encrypting")
            {
                label4.Text = "Normal Text";
                label1.Text = "Encrypted Text";
            }
            else
            {
                label1.Text = "Normal Text";
                label4.Text = "Encrypted Text";
            }

            String aux;
            aux = richTextBox1.Text;
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = aux;

            Crypt();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Clear all text from the textbox";
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (State) toolStripStatusLabel1.Text = "Change to Decrypt";
            else toolStripStatusLabel1.Text = "Change to Encrypt";
        }

        private void tabPage1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void crypt1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (State) label22.Text = "Encryption 1";
            else label22.Text = "Decryption 1";

            crypt2ToolStripMenuItem.Checked = false;

            richTextBox1.Height = 300;
            label24.Visible = false;
            textBox3.Visible = false;

            Crypt();
        }

        private void crypt2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (State) label22.Text = "Encryption 2";
            else label22.Text = "Decryption 2";

            crypt1ToolStripMenuItem.Checked = false;

            richTextBox1.Height = 250;
            label24.Visible = true;
            textBox3.Visible = true;

            Crypt();
        }

        private void crypt3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != null) Clipboard.SetText(richTextBox1.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(richTextBox2.Text != "") Clipboard.SetText(richTextBox2.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + Clipboard.GetText();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proprietati prop = new Proprietati();
            prop.ShowDialog();
        }

        private void CryptoLock_Activated(object sender, EventArgs e)
        {
            label23.Text = Properties.Settings.Default.username;
        }

        private void button5_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Enter(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "File";
        }

        private void criptTypeToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Crypt type";
        }

        private void serverToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Server";
        }

        private void helpToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Help";
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Copy text from text box one";
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Paste text to text box one";
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Copy text from text box two";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Crypt();
        }
    }
}
