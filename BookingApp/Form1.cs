using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BookingApp
{
    public partial class Form1 : Form
    {
        private const int numRows = 15;
        private const int numColumns = 15;
        private Button[,] seatButtons;
        TableLayoutPanel seatLayout;
        private bool isAdmin;

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowBookings()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                string selectBookingsQuery = "SELECT Bookings.Id, Users.Username FROM Bookings INNER JOIN Users ON Bookings.UserId = Users.Id";
                SQLiteCommand selectBookingsCommand = new SQLiteCommand(selectBookingsQuery, connection);
                SQLiteDataReader reader = selectBookingsCommand.ExecuteReader();

                StringBuilder sb = new StringBuilder();
                while (reader.Read())
                {
                    int bookingId = Convert.ToInt32(reader["Id"]);
                    string username = reader["Username"].ToString();
                    sb.AppendLine($"Booking Number: {bookingId}, User: {username}");
                }

                MessageBox.Show(sb.ToString(), "Bookings");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataBaseClass.InitaliseDataBase();

            // Добавление элементов управления на форму

            comboBoxGenre.Items.Add("комедия");
            comboBoxGenre.Items.Add("драма");
            comboBoxGenre.Items.Add("ужасы");
            comboBoxGenre.Items.Add("фантастика");

            comboBoxType.Items.Add("кино");
            comboBoxType.Items.Add("театр");

            isAdmin = false;
            UpdateEventsList();
        }

        private void UpdateEventsList()
        {
            listBoxEvents.Items.Clear();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                // Загрузка данных о мероприятиях
                string selectEventsQuery = "SELECT Id, Name, EventTime FROM Events WHERE EventDate = @eventDate";
                if (comboBoxGenre.SelectedItem != null)
                    selectEventsQuery += " AND Genre = @genre";
                if (comboBoxType.SelectedItem != null)
                    selectEventsQuery += " AND EventType = @eventType";

                SQLiteCommand selectEventsCommand = new SQLiteCommand(selectEventsQuery,
                connection);
                selectEventsCommand.Parameters.AddWithValue("@eventDate",
                dateTimePicker.Value.ToString("yyyy-MM-dd"));

                if (comboBoxGenre.SelectedItem != null)
                    selectEventsCommand.Parameters.AddWithValue("@genre",
                    comboBoxGenre.SelectedItem.ToString());
                if (comboBoxType.SelectedItem != null)
                    selectEventsCommand.Parameters.AddWithValue("@eventType",
                    comboBoxType.SelectedItem.ToString());

                SQLiteDataReader reader = selectEventsCommand.ExecuteReader();

                while (reader.Read())
                {
                    string eventName = reader["Name"].ToString();
                    string eventTime = reader["EventTime"].ToString();
                    string listItem = eventName + " - " + eventTime;
                    listBoxEvents.Items.Add(listItem);
                }
            }
        }

        private void listBoxEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEvents.SelectedItem == null)
                return;
            string selectedItem;
            string eventName;
            // Удаление старой схемы зала
            if (seatLayout != null && this.Controls.Contains(seatLayout))
            {
                this.Controls.Remove(seatLayout);
                seatLayout.Dispose();
            }

            if (listBoxEvents.SelectedIndex != -1)
            {
                 selectedItem = listBoxEvents.SelectedItem.ToString();
                 eventName = selectedItem.Split('-')[0].Trim();

                using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
                {
                    connection.Open();

                    // Загрузка информации о мероприятии
                    string selectEventQuery = "SELECT ImagePath FROM Events WHERE Name = @eventName";
                    SQLiteCommand selectEventCommand = new SQLiteCommand(selectEventQuery, connection);
                    selectEventCommand.Parameters.AddWithValue("@eventName", eventName);
                    SQLiteDataReader reader = selectEventCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        string imagePath = reader["ImagePath"].ToString();
                        pictureBoxImage.Image = Image.FromFile(imagePath);
                    }

                    // Отображение описания мероприятия
                    string selectEventDescriptionQuery = "SELECT EventDescription FROM Events WHERE Name = @eventName";
                    SQLiteCommand selectEventDescriptionCommand = new SQLiteCommand(selectEventDescriptionQuery, connection);
                    selectEventDescriptionCommand.Parameters.AddWithValue("@eventName", eventName);
                    string eventDescription = selectEventDescriptionCommand.ExecuteScalar().ToString();
                    richTextBoxDescription.Text = eventDescription;

                    if (isAdmin)
                    {
                        richTextBoxAdmin.Visible = true;
                        var bookingsListQuery = "SELECT Bookings.BookingNumber as BookingNumber , Users.Username as Username FROM Bookings INNER JOIN Users ON Bookings.UserId=Users.Id WHERE Bookings.EventId=(SELECT Id FROM Events WHERE Name=@eventName)";
                        var bookingsListCmd = new SQLiteDataAdapter(bookingsListQuery, connection);
                        bookingsListCmd.SelectCommand.Parameters.AddWithValue("@eventName", eventName);
                        var bookingsListDt = new DataTable();
                        bookingsListCmd.Fill(bookingsListDt);
                        var bookingsList = bookingsListDt.AsEnumerable().Select(row => row.Field<string>("Username") + ": " + row.Field<string>("BookingNumber"))
                            .ToList();
                        richTextBoxAdmin.Text = $"Список брони:\n{string.Join("\n", bookingsList)}";
                    }
                }
            }
            const int buttonSize = 41;
            seatButtons = new Button[numRows, numColumns];

            // Заполнение таблицу зала с кнопками
            seatLayout = new TableLayoutPanel();
            seatLayout.RowCount = numRows;
            seatLayout.ColumnCount = numColumns;

            for (int row = 0; row < numRows; row++)
            {
                seatLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonSize));

                for (int column = 0; column < numColumns; column++)
                {
                    seatLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, buttonSize));

                    Button seatButton = new Button();
                    seatButton.Size = new Size(buttonSize, buttonSize);
                    seatButton.BackColor = Color.Green; // Начальное состояние - свободное место
                    seatButton.Click += SeatButton_Click; // Обработчик события нажатия на кнопку места

                    // Установка текста кнопки в соответствии с номером места
                    char rowLetter = (char)('A' + row);
                    int seatNumber = column + 1;
                    seatButton.Font = new Font("Microsoft Sans Serif",7F, FontStyle.Regular,GraphicsUnit.Point, (byte)(204));
                    seatButton.Text = rowLetter + seatNumber.ToString();

                    seatLayout.Controls.Add(seatButton, column, row);

                    seatButtons[row, column] = seatButton;
                }
            }
            seatLayout.Location = new Point(15, 250);
            seatLayout.Size = new Size(buttonSize * numColumns, buttonSize * numRows);
            // Добавление таблицы зала на форму
            this.Controls.Add(seatLayout);

            // Сброс состояния всех кнопок мест
            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    seatButtons[row, column].BackColor = Color.Green;
                }
            }

                // Получение ID выбранного мероприятия
                selectedItem = listBoxEvents.SelectedItem.ToString();
                eventName = selectedItem.Split('-')[0].Trim();
                int eventId = -1;

                using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
                {
                    connection.Open();

                    string selectEventIdQuery = "SELECT Id FROM Events WHERE Name = @eventName";
                    SQLiteCommand selectEventIdCommand = new SQLiteCommand(selectEventIdQuery, connection);
                    selectEventIdCommand.Parameters.AddWithValue("@eventName", eventName);
                    eventId = Convert.ToInt32(selectEventIdCommand.ExecuteScalar());

                    // Загрузка информации о бронировании для выбранного мероприятия
                    string selectBookingsQuery = "SELECT SeatRow, SeatColumn FROM Bookings WHERE EventId = @eventId";
                    SQLiteCommand selectBookingsCommand = new SQLiteCommand(selectBookingsQuery, connection);
                    selectBookingsCommand.Parameters.AddWithValue("@eventId", eventId);
                    SQLiteDataReader reader = selectBookingsCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        int seatRow = Convert.ToInt32(reader["SeatRow"]);
                        int seatColumn = Convert.ToInt32(reader["SeatColumn"]);
                        seatButtons[seatRow, seatColumn].BackColor = Color.Red;
                    }
                    string selectUserBookingsQuery = "SELECT SeatRow, SeatColumn FROM Bookings WHERE EventId=@eventId AND UserId=(SELECT Id FROM Users WHERE Username=@username)";
                    SQLiteCommand selectUserBookingsCommand = new SQLiteCommand(selectUserBookingsQuery, connection);
                    selectUserBookingsCommand.Parameters.AddWithValue("@eventId", eventId);
                    selectUserBookingsCommand.Parameters.AddWithValue("@username", username);
                    reader = selectUserBookingsCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        int seatRow = Convert.ToInt32(reader["SeatRow"]);
                        int seatColumn = Convert.ToInt32(reader["SeatColumn"]);
                        seatButtons[seatRow, seatColumn].Enabled = true;
                    }
                }
        }

        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button seatButton = (Button)sender;
            int row = seatLayout.GetRow(seatButton);
            int column = seatLayout.GetColumn(seatButton);
            string selectedItem = listBoxEvents.SelectedItem.ToString();
            string eventName = selectedItem.Split('-')[0].Trim();
            int eventId = -1;
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();
                string selectEventIdQuery = "SELECT Id FROM Events WHERE Name = @eventName";
                SQLiteCommand selectEventIdCommand = new SQLiteCommand(selectEventIdQuery, connection);
                selectEventIdCommand.Parameters.AddWithValue("@eventName", eventName);
                eventId = Convert.ToInt32(selectEventIdCommand.ExecuteScalar());
                string selectBookingQuery = "SELECT UserId FROM Bookings WHERE EventId=@eventId AND SeatRow=@row AND SeatColumn=@column";
                SQLiteCommand selectBookingCommand = new SQLiteCommand(selectBookingQuery, connection);
                selectBookingCommand.Parameters.AddWithValue("@eventId", eventId);
                selectBookingCommand.Parameters.AddWithValue("@row", row);
                selectBookingCommand.Parameters.AddWithValue("@column", column);
                var bookingUserIdObj = selectBookingCommand.ExecuteScalar();
                if (bookingUserIdObj != null)
                {
                    int bookingUserId = Convert.ToInt32(bookingUserIdObj);
                    string userIdQuery = "SELECT Id FROM Users WHERE Username=@username";
                    SQLiteCommand userIdCommand = new SQLiteCommand(userIdQuery, connection);
                    userIdCommand.Parameters.AddWithValue("@username", username);
                    int currentUserId = Convert.ToInt32(userIdCommand.ExecuteScalar());
                    if (bookingUserId != currentUserId)
                        return;
                }
            }
            if (seatButton.BackColor == Color.Green) // Проверяем, является ли место свободным
            {
                seatButton.BackColor = Color.Red; // Забронированное место становится красным
            }
            else if (seatButton.BackColor == Color.Red) // Проверяем, является ли место забронированным
            {
                seatButton.BackColor = Color.Green; // Освобождаем место, делаем его снова зеленым
            }
        }
       
        private void createAccountButton_Click(object sender, EventArgs e)
        {
            // Отображение формы для создания учетной записи пользователя
            CreateUserForm createUserForm = new CreateUserForm();
            DialogResult result = createUserForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Получение данных нового пользователя из формы создания учетной записи
                string username = createUserForm.textBoxUserName.Text;
                string password = createUserForm.textBoxPassword.Text;

                // Создание учетной записи в базе данных
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
                {
                    connection.Open();

                    string insertUserQuery = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";
                    SQLiteCommand insertUserCommand = new SQLiteCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@username", username);
                    insertUserCommand.Parameters.AddWithValue("@password", password);
                    insertUserCommand.ExecuteNonQuery();

                    connection.Close();
                }
                MessageBox.Show("Учетная запись успешно создана!");
            }
        }

        private void buttonBook_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Пожалуйста, авторизуйтесь перед бронированием билетов.");
                return;
            }
            // Получение ID выбранного мероприятия
            string selectedItem = listBoxEvents.SelectedItem.ToString();
            string eventName = selectedItem.Split('-')[0].Trim();
            int eventId = -1;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                string selectEventIdQuery = "SELECT Id FROM Events WHERE Name = @eventName";
                SQLiteCommand selectEventIdCommand = new SQLiteCommand(selectEventIdQuery, connection);
                selectEventIdCommand.Parameters.AddWithValue("@eventName", eventName);
                eventId = Convert.ToInt32(selectEventIdCommand.ExecuteScalar());
            }


            // Сохранение информации о бронировании в базе данных
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                // Получение ID текущего пользователя
                string userIdQuery = "SELECT Id FROM Users WHERE Username = @username";
                SQLiteCommand userIdCommand = new SQLiteCommand(userIdQuery, connection);
                userIdCommand.Parameters.AddWithValue("@username", username);
                int userId = Convert.ToInt32(userIdCommand.ExecuteScalar());

                string insertBookingQuery = "INSERT INTO Bookings (UserId, EventId, SeatRow, SeatColumn, BookingNumber) VALUES (@userId, @eventId, @seatRow, @seatColumn,@bookingNumber)";
                SQLiteCommand insertBookingCommand = new SQLiteCommand(insertBookingQuery, connection);
                insertBookingCommand.Parameters.AddWithValue("@userId", userId);
                insertBookingCommand.Parameters.AddWithValue("@eventId", eventId);

                for (int row = 0; row < numRows; row++)
                {
                    for (int column = 0; column < numColumns; column++)
                    {
                        if (seatButtons[row, column].BackColor == Color.Red) // Проверяем, является ли место забронированным
                        {
                            seatButtons[row, column].Enabled = false;
                            insertBookingCommand.Parameters.AddWithValue("@seatRow", row);
                            insertBookingCommand.Parameters.AddWithValue("@seatColumn", column);

                            string bookingNumber = Guid.NewGuid().ToString();
                            insertBookingCommand.Parameters.AddWithValue("@bookingNumber", bookingNumber);

                            insertBookingCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            MessageBox.Show("Бронирование успешно выполнено!");
        }

        private string username;
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            DialogResult result = loginForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                username = loginForm.username;
                isAdmin = loginForm.isAdmin;
            }
        }

        private void comboBoxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEventsList();
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateEventsList();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEventsList();
        }
    }
}

