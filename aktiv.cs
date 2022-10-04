using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using MySql.Data.MySqlClient;
using static budilnik.Sym_alg;

namespace budilnik
{
    public partial class aktiv : Form
    {
        public aktiv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clock c2 = new clock();
            c2.Show();
            Hide();

        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBoxemail.Text == "")
            { MessageBox.Show("Введите почту");
                return; }


            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand("SELECT * FROM `userskod` WHERE `usageble` = 1 LIMIT 1", db.getconnection());
            adapter.SelectCommand = command2;
            adapter.Fill(table);
            DataRow row = table.Rows[0];
            String kodget = row.Field<string>("kodus");
            
           
            MySqlCommand command22 = new MySqlCommand("UPDATE `userskod` SET `usageble` = 0 WHERE `id` = "+ row.Field<int>("id") +" ", db.getconnection());
            db.openconnection();
            if (command22.ExecuteNonQuery() != 1)
                MessageBox.Show("Ошибка обновления статуса кода");
            db.closeconnection();

            byte[] b_mess = new byte[1]; // сообщение в байтах
            byte[] hash_arr = new byte[1]; // хэш
            Cr hash = new Cr();
            b_mess = System.Text.Encoding.Default.GetBytes(kodget);
            hash_arr = hash.SHA_256(b_mess);
            String hashstr = System.Text.Encoding.Default.GetString(hash_arr);
            
            Sym_alg SM = new Sym_alg();
            byte[] k = SM.key_gen();
            byte[] iv = SM.key_gen();
            String kstr = System.Text.Encoding.Default.GetString(k);
            String ivstr = System.Text.Encoding.Default.GetString(iv);
            String sumhk = hashstr + kstr;

            byte[] encypt = new byte[1]; // шифротекст
            byte[] sumhkbt = System.Text.Encoding.Default.GetBytes(sumhk);
            //encypt = SM.Rijndaelt_Encrypt(sumhkbt, k, iv);
            
            string encyptstr = RijndaelAlgorithm.Encrypt
        (
            sumhk,
            hashstr,
            "salt",
            "SHA256",
            2,
            ivstr,
            256
        );
            

              
            MySqlCommand command23 = new MySqlCommand("UPDATE `userskod` SET `handk` = @uh, `keyforh` = @uk, `iv` = @uiv WHERE `id` = " + row.Field<int>("id") + " ", db.getconnection());
            command23.Parameters.Add("@uh", MySqlDbType.VarChar).Value = encyptstr;
            command23.Parameters.Add("@uk", MySqlDbType.VarChar).Value = kstr;
            command23.Parameters.Add("@uiv", MySqlDbType.VarChar).Value = ivstr;
            db.openconnection();
            if (command23.ExecuteNonQuery() != 1)
              MessageBox.Show("Ошибка обновления хэша");
            db.closeconnection();



            SmtpClient Smtp = new SmtpClient("smtp.mail.ru", 687);
                Smtp.EnableSsl = true;
                Smtp.Credentials = new NetworkCredential("rrtt.dcvv@mail.ru", "18120508fdcnhbz");
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("rrtt.dcvv@mail.ru");
                Message.To.Add(new MailAddress(textBoxemail.Text));
                Message.Subject = "Код активации";
                Message.Body = kodget;

                try
                {
                    Smtp.Send(Message);
                MessageBox.Show("Успешно отправлено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String fromtextbox = maskedTextBox1.Text;
            maskedTextBox1.Clear();


            byte[] b_mess1 = new byte[1]; // сообщение в байтах
            byte[] hash_arr2 = new byte[1]; // хэш
            Cr hash1 = new Cr();
            b_mess1 = System.Text.Encoding.Default.GetBytes(fromtextbox);
            hash_arr2 = hash1.SHA_256(b_mess1);
            String hashstrfromuser = System.Text.Encoding.Default.GetString(hash_arr2);

           
           
            byte[] decypt = new byte[1]; // дешифрованное сообщение
            Sym_alg SM = new Sym_alg();
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand("SELECT * FROM `userskod` WHERE `usageble` = 0", db.getconnection());
            adapter.SelectCommand = command2;
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                DataRow row = table.Rows[i];
                String hashandkget = row.Field<string>("handk");
                String kget = row.Field<string>("keyforh");
                String ivget = row.Field<string>("iv");
                byte[] ivgetbt = System.Text.Encoding.Default.GetBytes(ivget);
                byte[] kgetbt = System.Text.Encoding.Default.GetBytes(kget);
                byte[] hashandkgetbt = System.Text.Encoding.Default.GetBytes(hashandkget);
                
                string hashandkget2 = RijndaelAlgorithm.Decrypt
                      (
                           hashandkget,
                           hashstrfromuser,
                           "salt",
                           "SHA256",
                            2,
                           ivget,
                           256
                      );
               
                //decypt = SM.Rijndaelt_Decrypt(hashandkgetbt, kgetbt, ivgetbt);
                
                string razdel = hashandkget2.Substring(0, 32);
                if (razdel == hashstrfromuser)
                {
                    DataTable table1 = new DataTable();
                    MySqlDataAdapter adapter1 = new MySqlDataAdapter();
                    MySqlCommand command13 = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getconnection());
                    command13.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Work_sess.loguser;
                    adapter1.SelectCommand = command13;
                    adapter1.Fill(table1);
                    if (table1.Rows.Count > 0)
                    {
                        DataRow row1 = table1.Rows[0];
                        UInt32 userid = row1.Field<UInt32>("id");
                        MySqlCommand command24 = new MySqlCommand("UPDATE `users` SET `demo` = 1 WHERE `id` = " + userid + "", db.getconnection());
                        db.openconnection();
                        if (command24.ExecuteNonQuery() != 1)
                            MessageBox.Show("Ошибка обновления статуса аккаунта");
                        db.closeconnection();
                        clock c2 = new clock();
                        c2.Show();
                        Hide();
                        MessageBox.Show("Спасибо за поддержку");
                    }


                }
                else
                    MessageBox.Show("Ошибка подтверждения");

            }


        }

        private void aktiv_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
