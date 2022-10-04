using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using MySql.Data.MySqlClient;
using static budilnik.Sym_alg;

namespace budilnik
{

    public partial class clock : Form
    {
        String soobshen;

        Timer timer01 = new Timer();
        Timer timer03 = new Timer();
        SoundPlayer sp = new SoundPlayer(@"C:\Users\edika\Downloads\budilnik\budilnik\1.wav");

        bool b = false;



        

        public clock()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            timer01.Interval = 1000;
            timer01.Tick += new EventHandler(timer1_Tick);
            timer03.Interval = 60000;
            timer03.Tick += new EventHandler(timer3_Tick);
            timer01.Start();
            timer03.Start();



        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label1.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP AND `demo` = 0", db.getconnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Work_sess.loguser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Work_sess.passuser;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 0)
            {

                buttonaktiv.Visible = false;
            }
           


        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            
                if (b == false)
                {
                    label2.Text = maskedTextBox1.Text;
                    timer2.Start();
                    maskedTextBox1.Visible = false;
                    button1.Text = "Убрать будильник";
                    b = true;
                    soobshen = textBox1.Text.ToString();
                    textBox1.Clear();
                }
                else if (b == true)
                {
                    label2.Text = "00:00";
                    timer2.Stop();
                    maskedTextBox1.Visible = true;
                    button1.Text = "Завести будильник";
                    b = false;

                }
            

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label1.Text == label2.Text + ":00")
            {
                String vremya = label2.Text;
                button2.Enabled = true;
                sp.Play();
                MessageBox.Show(soobshen);

                byte[] b_mess = new byte[1]; // сообщение в байтах

                b_mess = System.Text.Encoding.Default.GetBytes(soobshen);

                
                byte[] encypt = new byte[1]; // шифротекст
                String KEY ; // ключ
                String IV; // вектор инициализации
                Sym_alg SM = new Sym_alg();
                byte[] k = SM.key_gen();
                byte[] iv = SM.key_gen();
                KEY = System.Text.Encoding.Default.GetString(k);
                IV = System.Text.Encoding.Default.GetString(iv);
                //encypt = SM.Rijndaelt_Encrypt(b_mess, KEY, IV);

                string encyptstr = RijndaelAlgorithm.Encrypt
                     (
                       soobshen,
                       KEY,
                       "salt",
                       "SHA256",
                       2,
                       IV,
                       256
                     );
                



                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `soobshenias` (`messages`, `login`, `k`,`iv`,`timemessage`) VALUES(@usmes, @login, @k, @iv,@uvr)", db.getconnection());
                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = Work_sess.loguser;
                command.Parameters.Add("@usmes", MySqlDbType.VarChar).Value = encyptstr;
                command.Parameters.Add("@k", MySqlDbType.VarChar).Value = KEY;
                command.Parameters.Add("@iv", MySqlDbType.VarChar).Value = IV;
                command.Parameters.Add("@uvr", MySqlDbType.VarChar).Value = vremya;
                db.openconnection();
                if (command.ExecuteNonQuery() != 1)
                    MessageBox.Show("Ошибка отправки сообщения");
                db.closeconnection();
                
                
            }
            
        }



        private void button2_Click(object sender, EventArgs e)
        {
            sp.Stop();
            button2.Enabled = false;
            maskedTextBox1.Visible = true;
            button1.Text = "Завести будильник";
            b = false;
        }

        private void nameuserlog_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonaktiv_Click(object sender, EventArgs e)
        {

            aktiv ak2 = new aktiv();
            ak2.Show();
            Hide();

        }

        private void buttonrelog_Click(object sender, EventArgs e)
        {
            author a2 = new author();
            a2.Show();
            Hide();

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DateTime.Now.Minute.ToString("00");
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP AND `demo` = 0", db.getconnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Work_sess.loguser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Work_sess.passuser;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                
                MessageBox.Show("Поддержите рзработчика, купив лицензию");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            history h2 = new history();
            h2.Show();
            Hide();
        }

        private void clock_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
