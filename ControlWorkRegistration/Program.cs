using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ControlWorkRegistration
{

    class Program
    {


        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            
            Console.WriteLine("Нажмите 1 - для регистрации \n\t2 - для входа \n\t3 - для выхода");
            int choice = Int32.Parse(Console.ReadLine());
            User user = new User();
            var serializer = new XmlSerializer(typeof(User));
            if (choice == 1)
            {
                do
                {
                    Console.WriteLine("Введите номер телефона (формат X XXX XXX XX XX): ");
                    string tmpNum = Console.ReadLine();
                    user.Phone = Regex.Replace(tmpNum, @"[^0-9]", "");
                }
                while (!(user.Phone.Length==11));
                
                do
                {
                    Console.WriteLine("Введите email: ");
                    user.Email = Console.ReadLine();
                }
                while (!IsValid(user.Email));
                
                Console.WriteLine("Введите пароль: ");
                user.Password = Console.ReadLine();
                Console.WriteLine("Введите город: ");
                user.City = Console.ReadLine();
                Console.WriteLine("Введите возраст: ");
                string tmpAge = Console.ReadLine();
                user.Age = Int32.Parse(Regex.Replace(tmpAge, @"[^0-9]", ""));
             
                
                using (var stream = File.Create("data.xml"))
                {
                    serializer.Serialize(stream, user);
                }
                using (var stream = File.OpenRead("data.xml"))
                {
                    var result = serializer.Deserialize(stream) as User;

                    Console.WriteLine($"{result.Phone}, { result.Password}");
                }
            }

            else if (choice == 2)
            {
                var stream = File.OpenRead("data.xml");
                var result = serializer.Deserialize(stream) as User;

                do
                {
                    Console.WriteLine("Введите номер телефона: ");
                    user.Phone = Console.ReadLine();
                    Console.WriteLine("Введите пароль: ");
                    user.Password = Console.ReadLine();
                    if (user.Phone != result.Phone)
                    {
                        Console.WriteLine("С таким логином нет пользователя");
                    }
                    else if (user.Password != result.Password)
                    {
                        Console.WriteLine("Пароль не верный");
                        do
                        {
                            Console.WriteLine("Введите пароль: ");
                            user.Password = Console.ReadLine();
                        }
                        while (user.Password != result.Password);
                    }
                }
                while (!(user.Phone == result.Phone && user.Password == result.Password));
                Console.WriteLine("Логин и пароль совпадают");
                Console.WriteLine($"{result.Phone}, { result.Password}");
                stream.Close();
            }
            else if (choice == 3)
            {
                System.Environment.Exit(1);
            }
        }
    }
}
