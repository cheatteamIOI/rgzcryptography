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
    public partial class reg : Form
    {
        public reg()
        {
            InitializeComponent();
        }

        private void buttonreg_Click(object sender, EventArgs e)
        {
            if (LoginFieldreg.Text == "")
            {
                MessageBox.Show("Введите логин");
                return; }
            if (PassFieldreg.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (user_exist()) return;
            
            String loguser = LoginFieldreg.Text;
            String passuser = PassFieldreg.Text;
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`login`, `pass`, `demo`) VALUES(@loginreg, @passreg, `0`)", db.getconnection());
            command.Parameters.Add("@loginreg", MySqlDbType.VarChar).Value = loguser;
            command.Parameters.Add("@passreg", MySqlDbType.VarChar).Value = passuser;

            db.openconnection();
            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Succesfully");
            else
                MessageBox.Show("Fail");


            db.closeconnection();
            
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                clock c2 = new clock();
                c2.Show();
                Hide();

            }

        }
        public Boolean user_exist() 
        {                 
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getconnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = LoginFieldreg.Text;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой логин занят");
                return true;
            }
            else return false;
        }

        private void buttonspace_Click(object sender, EventArgs e)
        {
            author a2 = new author();
            a2.Show();
            Hide();
        }

        private void reg_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
