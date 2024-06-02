using System;
using System.Threading.Tasks;

namespace library
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            string loginFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\login.txt";
            string friendsFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friends.txt";
            string friendFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friendnotification.txt";
            string messageFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\messge.txt";

            var loginInstance = await Login.LoginMenuAsync(loginFilePath);
            Console.Clear();
            var messageInstance = new Message();
            var friendInstance = new Friends();
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("------------------------Notifications-----------------------\n");
            Console.WriteLine("\t----------------Message Notifications------------------\n");
            await messageInstance.Notification(messageFilePath , loginInstance.Username);
            Console.WriteLine("\t-------------------------------------------------------\n");
            Console.WriteLine("\t----------------Friend Request Notifications------------------\n");
            await friendInstance.SendnotificationAsync(friendFilePath, loginInstance.Username);
            Console.WriteLine("----------------------------------------------------------------\n");
            Console.WriteLine("----------------------------------------------------------------");
            bool flag = true;

            while (flag)
            {
                Console.WriteLine("\n\n\t------------------------------------------------");
                Console.WriteLine("\tPress 1 for Friends function\n\t" +
                    "Press 2 to see your account details\n\t" +
                    "Press 3 to enter message function\n\t" +
                    "Press 4 to display all users\n\t" +
                    "Press 5 to go back to login page\n\t" +
                    "Press 6 to search user\n\t" +
                    "Press 7 to exit");
                Console.WriteLine("\t------------------------------------------------");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("\t------------------------------------------------");
                    Console.WriteLine("\tInvalid input, please enter a number.");
                    Console.WriteLine("\t------------------------------------------------");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        if (loginInstance.Authenticate())
                        {
                            await Friends.MenuAsync(loginInstance.Username);
                        }
                        else
                        {
                            Console.WriteLine("\t------------------------------------------------");
                            Console.WriteLine("\tYou must be logged in to access this feature.");
                            Console.WriteLine("\t------------------------------------------------");
                        }
                        break;
                    case 2:
                        if (loginInstance.Authenticate())
                        {
                            await loginInstance.DisplayAllInfoAsync(loginFilePath);
                        }
                        else
                        {
                            Console.WriteLine("\t------------------------------------------------");
                            Console.WriteLine("\tYou must be logged in to see your account details.");
                            Console.WriteLine("\t------------------------------------------------");
                        }
                        break;
                    case 3:
                        if (loginInstance.Authenticate())
                        {
                            await friendInstance.CheckHasFriendAsync(friendsFilePath, loginInstance.Username);
                            if (friendInstance.AuthenticateHasFriend())
                            {
                                await messageInstance.AsyncMenu(loginInstance.Username);
                            }
                            else
                            {
                                Console.WriteLine("\t---------------------------------------------------");
                                Console.WriteLine("\tYou must have friends to access the message function.");
                                Console.WriteLine("\t---------------------------------------------------");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\t---------------------------------------------------");
                            Console.WriteLine("\tYou must be logged in to access the message function.");
                            Console.WriteLine("\t---------------------------------------------------");
                        }
                        break;
                    case 4:
                        if (loginInstance.Authenticate())
                        {
                            await loginInstance.DisplayAllUsersAsync(loginFilePath);
                        }
                        else
                        {
                            Console.WriteLine("\t-------------------------------------");
                            Console.WriteLine("\tYou must be logged in to see all users.");
                            Console.WriteLine("\t-------------------------------------");
                        }
                        break;
                    case 5:
                        Console.WriteLine("\t--------------------------------");
                        Console.WriteLine("\tYou chose to go back to login page");
                        Console.WriteLine("\t--------------------------------");
                        await Login.LoginMenuAsync(loginFilePath);
                        break;
                    case 6:
                        Console.WriteLine("\t----------------------");
                        Console.WriteLine("\tYou chose to search user");
                        Console.WriteLine("\t----------------------");
                        await loginInstance.searchuserAsync(loginFilePath);
                        break;
                    case 7:
                        Console.WriteLine("\t--------------------");
                        Console.WriteLine("\tExiting application...");
                        Console.WriteLine("\t--------------------");
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("\t-------------------------------");
                        Console.WriteLine("\tInvalid choice, please try again.");
                        Console.WriteLine("\t-------------------------------");
                        break;
                }
            }
        }
    }
}
