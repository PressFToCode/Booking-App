using System.Data.SQLite;

namespace BookingApp
{
    internal class DataBaseClass
    {
        public static void InitaliseDataBase()
        {
            // Создание базы данных
            SQLiteConnection.CreateFile("TheatreBookingSystem.db");

            // Подключение к базе данных
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TheatreBookingSystem.db;Version=3;"))
            {
                connection.Open();

                // Создание таблицы для мероприятий
                string createEventsTableQuery = "CREATE TABLE Events (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, ImagePath TEXT, EventTime TEXT, EventDescription TEXT, Genre TEXT, EventDate TEXT, EventType TEXT)";
                SQLiteCommand createEventsTableCommand = new SQLiteCommand(createEventsTableQuery, connection);
                createEventsTableCommand.ExecuteNonQuery();

                string insertEventQuery = "INSERT INTO Events (Name, ImagePath, EventTime, EventDescription, Genre, EventDate, EventType) VALUES (@name, @imagePath, @eventTime, @eventDescription, @genre, @eventDate, @eventType)";
                SQLiteCommand insertEventCommand = new SQLiteCommand(insertEventQuery, connection);

                insertEventCommand.Parameters.AddWithValue("@name", "Спектакль 'Гамлет'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "1.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "19:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Гамлет - одна из самых известных и загадочных трагедий Уильяма Шекспира. Это история о принце Дании, который после смерти отца узнает, что его дядя убил его ради власти и женщины. Гамлет решает отомстить, но попадает в ловушку своих сомнений, страстей и безумия. Спектакль затрагивает вечные темы любви, предательства, смерти и смысла жизни.");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-20");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Опера 'Кармен'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "2.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "18:30");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Кармен - одна из самых популярных и красивых опер в мире. Это история о страстной и свободолюбивой цыганке, которая очаровывает двух мужчин - солдата Дон Хосе и тореадора Эскамильо. Кармен не хочет подчиняться никому и живет по своим правилам, но ее выбор приводит к трагическому концу. Опера наполнена знаменитыми мелодиями, яркими костюмами и захватывающими танцами.");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-21");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Спектакль 'Ромео и Джульетта'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "3.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "19:30");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Ромео и Джульетта - самая известная и романтичная трагедия Уильяма Шекспира. Это история о любви двух молодых людей, которые принадлежат к враждующим семьям. Ромео и Джульетта решают бороться за свое счастье, но судьба распоряжается иначе. Спектакль показывает, как сила любви может преодолеть все преграды и как она может стать причиной большой беды. ");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-22");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Фильм 'Начало'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "4.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "21:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Начало - это фантастический триллер режиссера Кристофера Нолана. Это история о команде специалистов, которые умеют входить в сны людей и влиять на их подсознание. Они получают задание от таинственного заказчика - не просто извлечь информацию из сна, а внедрить идею. Но для этого им придется проникнуть в самые глубокие слои сновидений, где реальность и фантазия смешиваются. Фильм поражает своей оригинальностью, динамикой и визуальными эффектами. В главных ролях снялись Леонардо Ди Каприо, Марион Котийяр, Джозеф Гордон-Левитт и другие звезды кино.");
                insertEventCommand.Parameters.AddWithValue("@genre", "фантастика");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-22");
                insertEventCommand.Parameters.AddWithValue("@eventType", "кино");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Опера 'Травиата'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "5.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "18:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Травиата - это знаменитая опера Джузеппе Верди, основанная на романе Александра Дюма-сына “Дама с камелиями”. Это история о любви куртизанки Виолетты и молодого аристократа Альфредо. Виолетта отказывается от своей роскошной жизни ради настоящего чувства, но ее счастье разрушается общественными предрассудками и неизлечимой болезнью. Опера пронизана прекрасной музыкой, эмоциональными ариями и дуэтами.");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-20");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Фильм 'Матрица'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "6.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "22:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Матрица - это культовый фантастический боевик режиссеров-братьев Вачовски. Это история о программисте Томасе Андерсоне, который под псевдонимом Нео взламывает компьютерные системы. Он узнает, что вся его жизнь - это иллюзия, созданная искусственным интеллектом, который поработил человечество. Нео присоединяется к группе повстанцев, возглавляемой загадочным Морфеусом, и начинает борьбу за свободу. Фильм поразил своей оригинальной сюжетной линией, философскими идеями и революционными спецэффектами. В главных ролях снялись Киану Ривз, Лоуренс Фишбёрн, Кэрри-Энн Мосс и другие звезды кино.");
                insertEventCommand.Parameters.AddWithValue("@genre", "фантастика");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-21");
                insertEventCommand.Parameters.AddWithValue("@eventType", "кино");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Спектакль 'Отелло'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "7.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "19:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Вот возможное описание для афиши спектаклю Отелло:\r\n\r\nОтелло - это мощная и страшная трагедия Уильяма Шекспира. Это история о генерале Венеции Отелло, который женится на прекрасной Дездемоне. Но их счастье разрушает злобный лейтенант Яго, который хочет отомстить Отелло за то, что тот не повысил его в звании. Яго устраивает хитроумный интриги, чтобы внушить Отелло, что Дездемона изменяет ему с другим офицером. Отелло попадает в ярость и ревность, которые приводят к ужасным последствиям. Спектакль показывает, как человеческие страсти могут сломать разум и довести до преступления.");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-22");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Фильм 'Темный рыцарь'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "8.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "20:30");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Темный рыцарь - это вторая часть трилогии о Бэтмене режиссера Кристофера Нолана. Это история о противостоянии между тайным защитником Готэма Брюсом Уэйном и безумным преступником Джокером. Джокер хочет доказать, что любой человек может стать таким же злым и жестоким, как он, и развязывает войну с правосудием и обществом. Бэтмен должен остановить его, но для этого ему придется переступить свои принципы и пожертвовать своей репутацией. Фильм поразил своей напряженностью, актерской игрой и саундтреком. В главных ролях снялись Кристиан Бэйл, Хит Леджер, Аарон Экхарт и другие звезды кино.");
                insertEventCommand.Parameters.AddWithValue("@genre", "боевик");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-23");
                insertEventCommand.Parameters.AddWithValue("@eventType", "кино");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Опера 'Аида'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "9.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "18:30");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Аида - это величественная опера Джузеппе Верди, написанная по заказу хедива Египта. Это история о любви между египетским полководцем Радамесом и эфиопской пленницей Аидой. Аида - это дочь эфиопского царя, который ведет войну с Египтом. Радамес и Аида должны выбирать между своей любовью и своей родиной. Опера изобилует знаменитыми ариями, хорами и балетами.");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-24");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Фильм 'Интерстеллар'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "10.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "21:00");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Интерстеллар - это эпический фантастический фильм режиссера Кристофера Нолана. Это история о группе ученых и пилотов, которые отправляются в космическое путешествие, чтобы найти новый дом для человечества. Земля находится на грани экологической катастрофы, и единственная надежда - это червоточина, которая открывает доступ к другим галактикам. Фильм поражает своей научной достоверностью, эмоциональной силой и визуальной красотой. В главных ролях снялись Мэттью Макконахи, Энн Хэтэуэй, Джессика Честейн и другие звезды кино.");
                insertEventCommand.Parameters.AddWithValue("@genre", "фантастика");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-23");
                insertEventCommand.Parameters.AddWithValue("@eventType", "кино");
                insertEventCommand.ExecuteNonQuery();

                insertEventCommand.Parameters.AddWithValue("@name", "Спектакль 'Макбет'");
                insertEventCommand.Parameters.AddWithValue("@imagePath", "11.jpg");
                insertEventCommand.Parameters.AddWithValue("@eventTime", "19:30");
                insertEventCommand.Parameters.AddWithValue("@eventDescription", "Макбет - это кровавая и мрачная трагедия Уильяма Шекспира. Это история о шотландском генерале Макбете, который после встречи с тремя ведьмами решает убить короля и захватить власть. Но его жестокость и амбиции приводят к тому, что он становится жертвой своего страха, паранойи и вины. Спектакль показывает, как человеческая душа может быть разрушена жаждой власти и как она может стать причиной множества злодеяний. ");
                insertEventCommand.Parameters.AddWithValue("@genre", "драма");
                insertEventCommand.Parameters.AddWithValue("@eventDate", "2023-05-22");
                insertEventCommand.Parameters.AddWithValue("@eventType", "театр");
                insertEventCommand.ExecuteNonQuery();
                // Создание таблицы для пользователей
                string createUsersTableQuery = "CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT, Password TEXT)";
                SQLiteCommand createUsersTableCommand = new SQLiteCommand(createUsersTableQuery, connection);
                createUsersTableCommand.ExecuteNonQuery();

                // Создание таблицы для бронирования
                string createBookingsTableQuery = "CREATE TABLE Bookings (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, EventId INTEGER, SeatRow INTEGER, SeatColumn INTEGER, BookingNumber TEXT)";
                SQLiteCommand createBookingsTableCommand = new SQLiteCommand(createBookingsTableQuery, connection);
                createBookingsTableCommand.ExecuteNonQuery();
            }
        }
    }
}

