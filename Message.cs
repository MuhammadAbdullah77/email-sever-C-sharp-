using System;
using System.IO;
using System.Threading.Tasks;

namespace library
{
    internal class Message
    {
        private bool loginid = false;
        private bool hassfriend = false;
        private string friendUsername;

        public bool Authenticate()
        {
            return loginid;
        }

        public bool AuthenticateFriend()
        {
            return hassfriend;
        }

        public async Task CheckFriend(string filepath, string username)
        {
            string line;
            hassfriend = false;
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(':');
                        if (data.Length == 2 && data[0].Trim() == username && data[1].Trim() == friendUsername)
                        {
                            hassfriend = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t------------------------------------------------");
                Console.WriteLine($"\t{e.Message}");
                Console.WriteLine("\t------------------------------------------------");
            }
        }

        public async Task CheckUsernameExist(string filepath)
        {
            string line;
            loginid = false;
            bool found = false;
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length > 0 && data[0].Trim() == friendUsername)
                        {
                            loginid = true;
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("\t---------------------------");
                    Console.WriteLine($"\t{friendUsername} not found");
                    Console.WriteLine("\t---------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendMessageAsync(string filePath, string senderUsername)
        {
            Console.WriteLine("\t---------------------------------------------------------------------");
            Console.WriteLine("\tEnter the name of your friend to whom you want to send this message: ");
            Console.WriteLine("\t---------------------------------------------------------------------");
            friendUsername = Console.ReadLine();
            string filePathLogin = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\login.txt";
            string filePathFriend = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friends.txt";
            await CheckUsernameExist(filePathLogin);
            await CheckFriend(filePathFriend, senderUsername);

            if (Authenticate() && AuthenticateFriend())
            {
                Console.WriteLine("\t-----------------");
                Console.WriteLine("\tType your message: ");
                Console.WriteLine("\t-----------------");
                string message = Console.ReadLine();

                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath, append: true))
                    {
                        string userData = $"{senderUsername}:{friendUsername}:{message}";
                        await sw.WriteLineAsync(userData);
                        Console.WriteLine("\t------------------------");
                        Console.WriteLine("\tMessage sent successfully.");
                        Console.WriteLine("\t------------------------");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\t-------------------------------------");
                    Console.WriteLine($"\tFailed to send message: {ex.Message}");
                    Console.WriteLine("\t-------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\t-----------------------------------------------");
                Console.WriteLine($"\t{friendUsername} not found or not your friend.");
                Console.WriteLine("\t-----------------------------------------------");
            }
        }

        public async Task ReceiveMessage(string filepath, string username)
        {
            string line;
            bool hasMessages = false;
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(':');
                        if (data.Length == 3 && data[1].Trim() == username)
                        {
                            Console.WriteLine($"From {data[0].Trim()}: {data[2].Trim()}");
                            hasMessages = true;
                        }
                    }

                    if (!hasMessages)
                    {
                        Console.WriteLine("\t---------------------");
                        Console.WriteLine("\tNo messages received.");
                        Console.WriteLine("\t---------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task Reply(string filePath, string username)
        {
            await ReceiveMessage(filePath, username);
            await SendMessageAsync(filePath, username);
        }
        public async Task Notification(string filepath, string username)
        {
            string line;
            bool hasMessages = false;
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(':');
                        if (data.Length == 3 && data[1].Trim() == username)
                        {
                            hasMessages = true;
                            break;
                        }
                    }

                    if (hasMessages)
                    {
                        Console.WriteLine("\t----------------------");
                        Console.WriteLine("\tYou have new messages.");
                        Console.WriteLine("\t----------------------");
                    }
                    else
                    {
                        Console.WriteLine("\t----------------");
                        Console.WriteLine("\tNo new messages.");
                        Console.WriteLine("\t----------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task AsyncMenu(string loggedInUsername)
        {
            bool flag = true;
            string filePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\messge.txt";
            string filePathFriend = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friends.txt";
            while (flag)
            {
                Console.WriteLine("\t------------------------------------------------");
                Console.WriteLine("\n\t" +
                    "Press 1 to send a message to your friend\n\t" +
                    "Press 2 to check your inbox\n\t" +
                    "Press 3 to reply to a message\n\t" +
                    "Press 4 to check for new messages\n\t" +
                    "Press 5 to exit");
                Console.WriteLine("\t------------------------------------------------");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("\t-------------------------------------");
                    Console.WriteLine("\tInvalid input, please enter a number.");
                    Console.WriteLine("\t-------------------------------------");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\t-------------------------------------------");
                        Console.WriteLine("\tYou chose to send a message to your friend.");
                        Console.WriteLine("\t-------------------------------------------");
                        await CheckFriend(filePathFriend, loggedInUsername);
                        await SendMessageAsync(filePath, loggedInUsername);
                        break;
                    case 2:
                        Console.WriteLine("\t------------------------------");
                        Console.WriteLine("\tYou chose to check your inbox.");
                        Console.WriteLine("\t------------------------------");
                        await ReceiveMessage(filePath, loggedInUsername);
                        break;
                    case 3:
                        Console.WriteLine("\t--------------------------------");
                        Console.WriteLine("\tYou chose to reply to a message.");
                        Console.WriteLine("\t--------------------------------");
                        await Reply(filePath, loggedInUsername);
                        break;
                    case 4:
                        Console.WriteLine("\t------------------------------------");
                        Console.WriteLine("\tYou chose to check for new messages.");
                        Console.WriteLine("\t------------------------------------");
                        await Notification(filePath, loggedInUsername);
                        break;
                    case 5:
                        Console.WriteLine("\t-----------------------");
                        Console.WriteLine("\tExiting Message Menu...");
                        Console.WriteLine("\t-----------------------");
                        Console.Clear();
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("\t--------------------------");
                        Console.WriteLine("\tInvalid choice, try again.");
                        Console.WriteLine("\t--------------------------");
                        break;
                }
            }
        }
    }
}
