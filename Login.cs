using System;
using System.IO;
using System.Threading.Tasks;

namespace library
{
    internal class Login
    {
        public string username;
        private string password;
        private string email;
        private bool loginId = false;
        private string enteredPassword;
        private string enteredUsername;
        public string Username => username;

        public async Task CreateAccountAsync(string filePath)
        {
            Console.WriteLine("\t------------");
            Console.WriteLine("\tEnter Username: ");
            Console.WriteLine("\t------------");
            username = Console.ReadLine();

            if (await CheckUsernameAsync(filePath, username))
            {
                Console.WriteLine("\t--------------------------");
                Console.WriteLine("\tUsername is already taken.");
                Console.WriteLine("\t--------------------------");
                return;
            }
            Console.WriteLine("\t-------------");
            Console.WriteLine("\tEnter Password: ");
            Console.WriteLine("\t-------------");
            password = Console.ReadLine();
            Console.WriteLine("\t-----------");
            Console.WriteLine("\tEnter Email: ");
            Console.WriteLine("\t-----------");
            email = Console.ReadLine();

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    string userdata = $"{username},{password},{email}";
                    await writer.WriteLineAsync(userdata);
                }
                Console.WriteLine("\t-----------------------------");
                Console.WriteLine("\tAccount created successfully!");
                Console.WriteLine("\t-----------------------------");
                loginId = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("\t---------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while writing to the file: {e.Message}");
                Console.WriteLine("\t---------------------------------------------------------");
                loginId = false;
            }
        }

        public async Task LoginAccountAsync(string filePath)
        {
            Console.WriteLine("\t-------------");
            Console.WriteLine("\tEnter Username: ");
            Console.WriteLine("\t-------------");
            enteredUsername = Console.ReadLine();
            Console.WriteLine("\t-------------");
            Console.WriteLine("\tEnter Password: ");
            Console.WriteLine("\t-------------");
            enteredPassword = Console.ReadLine();

            try
            {
                string line;
                bool found = false;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] userData = line.Split(',');

                        if (userData.Length == 3 &&
                            userData[0].Trim() == enteredUsername &&
                            userData[1].Trim() == enteredPassword)
                        {
                            Console.WriteLine($"Login successful for '{enteredUsername}'.");
                            found = true;
                            loginId = true;
                            username = enteredUsername;
                            password = userData[1].Trim();
                            email = userData[2].Trim();
                            break;
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("\t------------------------------------------------");
                    Console.WriteLine("\tLogin failed. Incorrect username or password.");
                    Console.WriteLine("\t------------------------------------------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t-------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-------------------------------------------------------");
            }
        }

        private async Task<bool> CheckUsernameAsync(string filePath, string usernameToCheck)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length > 0 && data[0].Trim() == usernameToCheck)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t-------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-------------------------------------------------------");
            }
            return false;
        }

        public bool Authenticate()
        {
            return loginId;
        }

        public async Task DisplayAllInfoAsync(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 3 && data[0].Trim() == username)
                        {
                            Console.WriteLine($"Your username is: {username}\nYour password is: {password}\nYour email is: {email}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t-----------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-----------------------------------------------------");
            }
        }

        public async Task DisplayAllUsersAsync(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 3)
                        {
                            Console.WriteLine($"Username: {data[0].Trim()}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t-------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-------------------------------------------------------");
            }
        }
        public async Task searchuserAsync(string filepath)
        {
            Console.WriteLine("\t------------------------------------------------");
            Console.WriteLine("\tEnter the username you want search");
            Console.WriteLine("\t------------------------------------------------");
            string name = Console.ReadLine();
            string line;
            bool found = false;
            try
            {
                using(StreamReader reader = new StreamReader(filepath))
                {
                    while((line = await reader.ReadLineAsync())!= null)
                    {
                        string[] data = line.Split(',');
                        if(data.Length == 3 && data[0].Trim() == name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        Console.WriteLine("\t------------------------");
                        Console.WriteLine($"\t{name} Does not exist");
                        Console.WriteLine("\t------------------------");
                    }
                    else
                    {
                        Console.WriteLine("\t-----------------");
                        Console.WriteLine($"\t{name} is exist");
                        Console.WriteLine("\t-----------------");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static async Task<Login> LoginMenuAsync(string filePath)
        {
            Login loginInstance = new Login();
            bool flag = true;

            while (flag)
            {
                Console.WriteLine("\t------------------------------------------------");
                Console.WriteLine("\t----------- Welcome to my application ----------");
                Console.WriteLine("\t Press 1 to Login to your account");
                Console.WriteLine("\t Press 2 to Create Account");
                Console.WriteLine("\t Press 3 to Exit");
                Console.WriteLine("\t------------------------------------------------");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("\t--------------------------------------");
                    Console.WriteLine("\t Invalid input, please enter a number.");
                    Console.WriteLine("\t--------------------------------------");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\t-----------------------------------");
                        Console.WriteLine("\t You chose to login to your account");
                        Console.WriteLine("\t-----------------------------------");
                        await loginInstance.LoginAccountAsync(filePath);
                        break;
                    case 2:
                        Console.WriteLine("\t----------------------------------");
                        Console.WriteLine("\t You chose to create a new account");
                        Console.WriteLine("\t----------------------------------");
                        await loginInstance.CreateAccountAsync(filePath);
                        break;
                    case 3:
                        Console.WriteLine("\t-----------------------");
                        Console.WriteLine("\t Exiting application...");
                        Console.WriteLine("\t-----------------------");
                        Console.Clear();
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("\t----------------------------------");
                        Console.WriteLine("\t Invalid choice, please try again.");
                        Console.WriteLine("\t----------------------------------");
                        break;
                }

                if (loginInstance.Authenticate())
                {
                    flag = false;
                }
            }

            return loginInstance;
        }
    }
}
