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

namespace budilnik
{
    
    public partial class author : Form
    {
        public author()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Work_sess.loguser = LoginField.Text;
            Work_sess.passuser = PassField.Text;
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP",db.getconnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Work_sess.loguser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Work_sess.passuser;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                clock c2 = new clock();
                c2.Show();
                Hide();

            }
            else MessageBox.Show("Неправильные данные");



        }

        private void buttonreg_Click(object sender, EventArgs e)
        {
            reg g2 = new reg();
            g2.Show();
            Hide();
        }

        private void author_FormClosed(object sender, FormClosedEventArgs e)
        {
          
        }
    }
}
