using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

// TODO: add search option
namespace Diary
{
    public partial class Phone : Form
    {
        public Phone()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Function thats opens or close the connection from application to mysql database.
        /// </summary>
        /// <param name="ver"> 1 to open connection or 0 to close</param>
        /// <returns></returns>
        public MySqlConnection StoreConnection(int ver)
        {           
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "user";
            builder.Password = "user123!";
            builder.Database = "phone";
            MySqlConnection connection = new MySqlConnection(builder.ToString());
            if (ver == 1)
                connection.Open();
            else          
                connection.Close();
   
            return connection;
        } 
        /// <summary>
        /// Simple function that's shows the database view in datagrid on application.
        /// </summary>
        void Display()
        {
            MySqlDataAdapter sda = new MySqlDataAdapter("SELECT * FROM phone ORDER BY Surname", StoreConnection(1));

            DataTable dt = new DataTable();

            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach(DataRow item in dt.Rows)
            {
                int hows = dataGridView1.Rows.Add();
                dataGridView1.Rows[hows].Cells[0].Value = item["Name"].ToString();
                dataGridView1.Rows[hows].Cells[1].Value = item["Surname"].ToString();
                dataGridView1.Rows[hows].Cells[2].Value = item["Mobile"].ToString();
                dataGridView1.Rows[hows].Cells[3].Value = item["Email"].ToString();
                dataGridView1.Rows[hows].Cells[4].Value = item["Category"].ToString();
            }
        }
        //in a start of application takes some simple changes and run display function 
        private void Phone_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBoxName;
            textBoxName.Focus();
            Display();         
        }
        //action to click NEW button thats clear all textboxes.
        private void buttonNEW_Click(object sender, EventArgs e)
        {
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPhone.Clear();
            textBoxEmail.Clear();
            comboBoxCategory.SelectedIndex = -1;
            textBoxName.Focus();
        }
        // action to click INSERT button which connect to database and inserts data from textboxes to database.
        private void buttonINSERT_Click(object sender, EventArgs e)
        {           
            string insert_user = "INSERT INTO phone(Name, Surname, Mobile, Email, Category) VALUES (@Name, @Surname, @Mobile, @Email, @Category)";
            MySqlCommand insertuser = new MySqlCommand(insert_user, StoreConnection(1));
            insertuser.CommandText = insert_user;
            insertuser.Parameters.AddWithValue("@Name", textBoxName.Text);
            insertuser.Parameters.AddWithValue("@Surname", textBoxSurname.Text);
            insertuser.Parameters.AddWithValue("@Mobile", textBoxPhone.Text);
            insertuser.Parameters.AddWithValue("@Email", textBoxEmail.Text);
            insertuser.Parameters.AddWithValue("@Category", comboBoxCategory.Text);
            insertuser.ExecuteNonQuery();
            MessageBox.Show("Dodano użytkownika");
            Display();
            StoreConnection(0);         
        }
        // action to click INSERT button which connect to database and delete user from database.
        private void buttonDELETE_Click(object sender, EventArgs e)
        {
            string insert_user = "DELETE FROM phone WHERE mobile=@Mobile";
            MySqlCommand insertuser = new MySqlCommand(insert_user, StoreConnection(1));
            insertuser.CommandText = insert_user;         
            insertuser.Parameters.AddWithValue("@Mobile", textBoxPhone.Text);   
            insertuser.ExecuteNonQuery();
            MessageBox.Show("Usunieto pomyslnie!");
            Display();
            StoreConnection(0);
        }
        //action to click UPDATE vutton which update all user data by identification of mobile number.
        private void buttonUPDATE_Click(object sender, EventArgs e)
        {
            string insert_user = "UPDATE phone SET Name = @Name, Surname = @Surname, Mobile =@Mobile, Email=@Email, Category = @Category WHERE Mobile =@Mobile";
            MySqlCommand insertuser = new MySqlCommand(insert_user, StoreConnection(1));
            insertuser.CommandText = insert_user;
            insertuser.Parameters.AddWithValue("@Name", textBoxName.Text);
            insertuser.Parameters.AddWithValue("@Surname", textBoxSurname.Text);
            insertuser.Parameters.AddWithValue("@Mobile", textBoxPhone.Text);
            insertuser.Parameters.AddWithValue("@Email", textBoxEmail.Text);
            insertuser.Parameters.AddWithValue("@Category", comboBoxCategory.Text);
            insertuser.ExecuteNonQuery();
            MessageBox.Show("Uaktualniono dane!");
            Display();
            StoreConnection(0);
        }
        //function, when you click datagrid verse that sends all data to textboxes.
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxName.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBoxSurname.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBoxPhone.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBoxEmail.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBoxCategory.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }      
    }
}
