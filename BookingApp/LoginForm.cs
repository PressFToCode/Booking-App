using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BookingApp
{
    public partial class LoginForm : Form
    {
        public int UserId { get; private set; }
        public string username;
        public bool isAdmin = false;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            button1.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            if (username == "admin" && password == "admin")
            {
                isAdmin = true;
                MessageBox.Show("Вы вошли как администратор!");
                this.Close();
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                string selectUserQuery = "SELECT Id FROM Users WHERE Username = @username AND Password = @password";
                SQLiteCommand selectUserCommand = new SQLiteCommand(selectUserQuery, connection);
                selectUserCommand.Parameters.AddWithValue("@username", username);
                selectUserCommand.Parameters.AddWithValue("@password", password);
                object result = selectUserCommand.ExecuteScalar();
                selectUserCommand.Parameters.AddWithValue("@usersid", Convert.ToInt32(result));

                if(username != "admin") isAdmin = false;

                if (result != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    MessageBox.Show($"Вход успешно выполнен. Добро пожаловать {username}!");
                }
                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль!");
                }
            }
        }
    }
}

