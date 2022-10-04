using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static budilnik.Sym_alg;

namespace budilnik
{
    public partial class history : Form
    {
        public history()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void history_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonback_Click(object sender, EventArgs e)
        {
            clock c2 = new clock();
            c2.Show();
            Hide();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonshow_Click(object sender, EventArgs e)
        {
            byte[] decypt = new byte[1]; // дешифрованное сообщение
            Sym_alg SM = new Sym_alg();
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand("SELECT * FROM `soobshenias` WHERE `login` = @uL", db.getconnection());
            command2.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Work_sess.loguser;
            adapter.SelectCommand = command2;
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                String messget = row.Field<string>("messages");
                String kget = row.Field<string>("k");
                String ivget = row.Field<string>("iv");
                String vremyaget = row.Field<string>("timemessage");
                byte[] ivgetbt = System.Text.Encoding.Default.GetBytes(ivget);
                byte[] kgetbt = System.Text.Encoding.Default.GetBytes(kget);
                byte[] messgetbt = System.Text.Encoding.Default.GetBytes(messget);
                //decypt = SM.Rijndaelt_Decrypt(messgetbt, kgetbt, ivgetbt);

                byte[] cipherTextBytes = System.Text.Encoding.Default.GetBytes(messget);
                string messgetde2 = RijndaelAlgorithm.Decrypt
        (
            messget,
            kget,
            "salt",
            "SHA256",
            2,
            ivget,
            256
        );
                

                textBox1.Text += "Cообщения "+Work_sess.loguser+"["+i+"] Время сообщения " + vremyaget + ": " + "'" + messgetde2 + "'" + "\r\n";
                
            }
        }

        private void buttonclose_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void history_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
