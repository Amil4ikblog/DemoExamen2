using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Auto_Technical_Center
{
    // Модель клиента
    public class Client
    {
        public int Id { get; set; } // Идентификатор клиента
        public int ID_auto { get; set; } // Идентификатор автомобиля
        public string FIO { get; set; } // ФИО клиента
        public string ContactPhoneNumbers { get; set; } // Номер телефона клиента
        public string EmailAddresses { get; set; } // Email клиента
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        private readonly string connectionString = "server=127.0.0.1;port=3306;user id=root;password=root;database=auto_technical_center;";

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this; // Установка контекста данных для привязки
            LoadClients(); // Вызов метода загрузки данных
        }



        public void LoadClients()
        {
            string connectionString = "server=127.0.0.1;port=3306;user id=root;password=root;database=auto_technical_center;"; // Укажите строку подключения к вашей БД
            List<Client> clients = new List<Client>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, FIO, contact_phone_numbers, email_addresses FROM clients"; // Укажите ваш запрос

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clients.Add(new Client
                                {
                                    Id = reader.GetInt32(0),
                                    FIO = reader.GetString(1),
                                    ContactPhoneNumbers = reader.GetString(2),
                                    EmailAddresses = reader.GetString(3)
                                });
                            }
                        }
                    }
                }

                ListViewClient.ItemsSource = clients; // Устанавливаем источник данных для ListView
            }
            catch (MySqlException ex)
            {
                // Обработка ошибок, связанных с MySQL
                MessageBox.Show($"Ошибка при работе с базой данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Обработка других ошибок
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveClient(Client client)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command;

                    if (client.Id == 0) // Добавление нового клиента
                    {
                        command = new MySqlCommand("INSERT INTO clients (ID_auto, FIO, contact_phone_numbers, email_addresses) VALUES (@ID_auto, @FIO, @ContactPhoneNumbers, @EmailAddresses)", connection);
                    }
                    else // Обновление существующего клиента
                    {
                        command = new MySqlCommand("UPDATE clients SET ID_auto = @ID_auto, FIO = @FIO, contact_phone_numbers = @ContactPhoneNumbers, email_addresses = @EmailAddresses WHERE Id = @Id", connection);
                        command.Parameters.AddWithValue("@Id", client.Id);
                    }

                    // Добавление параметров
                    command.Parameters.AddWithValue("@ID_auto", client.ID_auto);
                    command.Parameters.AddWithValue("@FIO", client.FIO);
                    command.Parameters.AddWithValue("@ContactPhoneNumbers", client.ContactPhoneNumbers);
                    command.Parameters.AddWithValue("@EmailAddresses", client.EmailAddresses);

                    command.ExecuteNonQuery(); // Выполнение команды

                    // Обновление коллекции после сохранения
                    if (client.Id == 0)
                    {
                        client.Id = (int)command.LastInsertedId; // Получаем ID нового клиента
                        Clients.Add(client); // Добавление нового клиента в коллекцию

                        MessageBox.Show($"Добавлен новый клиент: {client.FIO}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Редактирован клиент: {client.FIO}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return; // Валидация ввода

            var newClient = new Client
            {
                ID_auto = 0, // Здесь можно установить значение по умолчанию или получить его из другого поля, если нужно
                FIO = FioTextBox.Text.Trim(),
                ContactPhoneNumbers = PhoneTextBox.Text.Trim(),
                EmailAddresses = EmailTextBox.Text.Trim()
            };

            SaveClient(newClient); // Сохранение нового клиента
            ClearInputFields(); // Очистка полей после добавления
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewClient.SelectedItem is Client selectedClient)
            {
                if (!ValidateInput()) return; // Валидация ввода

                // Обновление данных клиента из текстовых полей
                selectedClient.FIO = FioTextBox.Text.Trim();
                selectedClient.ContactPhoneNumbers = PhoneTextBox.Text.Trim();
                selectedClient.EmailAddresses = EmailTextBox.Text.Trim();

                SaveClient(selectedClient); // Сохранение обновленного клиента
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Сообщение, если клиент не выбран
            }
        }
        private void ClearInputFields()
        {
            FioTextBox.Clear();
            PhoneTextBox.Clear();
            EmailTextBox.Clear();
            ListViewClient.SelectedItem = null; // Сброс выбора
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FioTextBox.Text))
            {
                MessageBox.Show("Введите ФИО клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Введите номер телефона клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !EmailTextBox.Text.Contains("@"))
            {
                MessageBox.Show("Введите корректный email клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true; // Все проверки пройдены
        }
        private void SearchClients(string searchTerm)
        {
            var filteredClients = Clients.Where(c => c.FIO.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            // Обновите ListView с отфильтрованными клиентами
            ListViewClient.ItemsSource = filteredClients;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewClient.SelectedItem is Client selectedClient)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить клиента {selectedClient.FIO}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            var command = new MySqlCommand("DELETE FROM clients WHERE Id = @Id", connection);
                            command.Parameters.AddWithValue("@Id", selectedClient.Id);
                            command.ExecuteNonQuery();
                            Clients.Remove(selectedClient); // Удаляем клиента из коллекции
                            MessageBox.Show($"Клиент {selectedClient.FIO} успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
            private void ListViewClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewClient.SelectedItem is Client selectedClient)
            {
                FioTextBox.Text = selectedClient.FIO;
                PhoneTextBox.Text = selectedClient.ContactPhoneNumbers;
                EmailTextBox.Text = selectedClient.EmailAddresses;
            }
        }

    }
}