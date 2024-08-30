using TelephoneBook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TelephoneBook
{
    class Phonebook
    {
        #region Поля и свойства

        private List<Subscriber>? abonents;
        private string fileName;
        private static Phonebook instance;

        #endregion

        #region Конструктор

        protected Phonebook()
        {
            abonents = new List<Subscriber>();
            fileName = Path.Combine(Directory.GetCurrentDirectory(), "phonebook.txt");
            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                if (new FileInfo(fileName).Length != 0)
                {
                    abonents = JsonSerializer.Deserialize<List<Subscriber>>(fileStream);
                }
                
            }
        }

        #endregion

        #region Методы

        public static Phonebook GetInstance()
        {
            if (instance == null)
                instance = new Phonebook();
            return instance;
        }

        /// <summary>
        /// Вывести главное меню телефонной книги
        /// </summary>
        public void GetMenu()
        {
            int userInput;
            do
            {
                Console.Write("Телефонная книга. Главное меню:\n" +
                              "1. Добавить абонента,\n" +
                              "2. Найти абонента,\n" +
                              "3. Выход\n" +
                              "Введите номер опции: ");

                userInput = GetUserInput(3); 
                if (userInput == 1)
                    AddSubscriber();
                if (userInput == 2)
                    GetSubscriber();
                Thread.Sleep(1000);
                Console.Clear();
            } while (userInput != 3);
            CloseApp();
        }

        /// <summary>
        /// Получить от пользователя номер пункта меню и проверить его на корректность
        /// </summary>
        /// <param name="numberMenuItems">Количество пунктов в меню</param>
        /// <returns></returns>
        private int GetUserInput(int numberMenuItems)
       {
            int result;
            while (true)
            {
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out result))
                {
                    if (result < 1 || result > numberMenuItems)
                        Console.WriteLine("Пункта меню с таким номером нет");
                    else
                        break;
                }
                else
                    Console.WriteLine("Не удалось распознать число");
                Console.Write("Попробуйте еще раз: ");
            }
            return result;
        }

        /// <summary>
        /// Добавить пользователя в телефонную книгу
        /// </summary>
        /// <returns>True, если новый контакт добавлен в телефонную книгу, false - если такой абонент уже был зарегистрирован </returns>
        private bool AddSubscriber()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите номер: ");
            string phoneNumber = Console.ReadLine();
            Subscriber abonent = new Subscriber(name, phoneNumber);
            if (!IsRepeatSubscriber(abonent))
            {
                abonents.Add(abonent);
                Console.WriteLine("успешно!");
                return true;
            }
            else
            {
                Console.WriteLine("Такой абонент уже зарегистрирован!");
                return false;
            }
        }

        /// <summary>
        /// Проверить, не совпадает ли новый контакт с уже зарегистрированными в телефонной книге
        /// </summary>
        /// <param name="newAbonent">новый контакт</param>
        /// <returns>True, если совпадает, false - если нет</returns>
        private bool IsRepeatSubscriber(Subscriber newAbonent)
        {
            foreach (Subscriber thisAnonent in abonents)
            {
                if (thisAnonent.Equals(newAbonent))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Вывести на экран список искомых абонентов и меню для работы с контректным из них
        /// </summary>
        private void GetSubscriber()
        {
            Console.Write("Введите имя или номер абонента: ");
            string userInput = Console.ReadLine();
            List<int> foundSubscriber = new List<int>();

            GetMatches(userInput, foundSubscriber);

            if (foundSubscriber.Count > 0)
            {
                for (int i = 0; i < foundSubscriber.Count; i++)
                    Console.WriteLine($"{i + 1}. {abonents[foundSubscriber[i]].ToString()}");

                int choice = 0;
                Console.Write("Введите порядковый номер абонента для работы с ним: ");
                int subscriberSeralNumber = GetUserInput(foundSubscriber.Count) - 1;
                do
                {
                    Console.WriteLine(abonents[foundSubscriber[subscriberSeralNumber]].ToString());
                    Console.Write("1. Изменить,\n" +
                                  "2. Удалить,\n" +
                                  "3. Выйти в главное меню\n" +
                                  "Выберите пункт: ");

                    choice = GetUserInput(3);
                    if (choice == 1)
                        UpdateSubscriber(foundSubscriber[subscriberSeralNumber]);
                    if (choice == 2)
                    {
                        RemoveAbonent(foundSubscriber[subscriberSeralNumber]);
                        Console.WriteLine("Успешно!");
                    }
                    choice = 3;
                } while (choice != 3);
            }
            else
                Console.WriteLine("Совпадений не найдено");
        }

        /// <summary>
        /// Заполнить лист с индексами искомых абонентов в телефонной книге
        /// </summary>
        /// <param name="userInput">Ввод пользователя (имя или номер телефона абонента)</param>
        /// <param name="foundSubscriber">Лист с индексами искомых абонентов</param>
        private void GetMatches(string userInput, List<int> foundSubscriber)
        {
            for (int i = 0; i < abonents.Count; i++)
            {
                if (abonents[i].CompareNames(userInput))
                    foundSubscriber.Add(i);
            }

            for (int i = 0; i < abonents.Count; i++)
            {
                if (abonents[i].ComparePhoneNumbers(userInput))
                    foundSubscriber.Add(i);
            }
        }

        /// <summary>
        /// Удалить контакт
        /// </summary>
        /// <param name="subscriberSeralNumber">Индекс контакта в телефонной книге</param>
        private void RemoveAbonent(int subscriberSeralNumber)
        {
            abonents.RemoveAt(subscriberSeralNumber);
        }

        /// <summary>
        /// Изменить контакт
        /// </summary>
        /// <param name="subscriberSeralNumber">Индекс контакта в телефонной книге</param>
        private void UpdateSubscriber(int subscriberSeralNumber)
        {
            bool success = AddSubscriber();
            if (success)
                RemoveAbonent(subscriberSeralNumber);
        }

        /// <summary>
        /// Закрыть программу и сохранить телефонную книгу в файл
        /// </summary>
        private void CloseApp()
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, abonents);
            }
        }
    }
    #endregion
}

