using System;
using System.IO;
using System.Threading.Tasks;

namespace library
{
    internal class Friends
    {
        private bool hasFriend = false;
        private bool userFound = false;
        private string friendName;

        public bool AuthenticateUser()
        {
            return userFound;
        }

        public async Task CheckUserAsync(string filepath, string username)
        {
            userFound = false;
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 3 && data[0].Trim() == username)
                        {
                            userFound = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t------------------------------------------------");
                Console.WriteLine(e.Message);
                Console.WriteLine("\t------------------------------------------------");
            }
        }
        public async Task SendnotificationAsync(string filepath , string username)
        {
            string line;
            bool hasnotification = false;
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        string[] data = line.Split(':');
                        if(data.Length == 2 && data[1].Trim() == username)
                        {
                            Console.WriteLine("\t--------------------------------------------------");
                            Console.WriteLine($"\tFrom {data[0].Trim()} has added you as a friend");
                            Console.WriteLine("\t--------------------------------------------------");
                            hasnotification = true;
                        }
                    }
                    if(!hasnotification)
                    {
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("\tNo friend request received");
                        Console.WriteLine("------------------------------------------------------");
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }
        public async Task AddFriendAsync(string loginFilePath, string friendsFilePath, string loggedInUsername , string friendpath)
        {
            Console.WriteLine("\t-----------------------------------");
            Console.WriteLine("\tEnter Friend's Name: ");
            Console.WriteLine("\t-----------------------------------");
            friendName = Console.ReadLine();

            
            await CheckUserAsync(loginFilePath, friendName);
            if (AuthenticateUser())
            {
                string friendData = $"{loggedInUsername}:{friendName}";
                string userdata = $"{loggedInUsername}:{friendName}";
                try
                {
                    using (StreamWriter writer = File.AppendText(friendsFilePath))
                    {
                        await writer.WriteLineAsync(friendData);
                    }
                    Console.WriteLine("\t----------------------------------");
                    Console.WriteLine("\tFriend added successfully!");
                    Console.WriteLine("\t----------------------------------");
                    using (StreamWriter wr = new StreamWriter(friendpath , append:true))
                    {
                        await wr.WriteLineAsync(userdata);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\t-----------------------------------------------------------------");
                    Console.WriteLine($"\tAn error occurred while writing to the file: {e.Message}");
                    Console.WriteLine("\t-----------------------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\t-----------------------");
                Console.WriteLine("\tUser does not exist.");
                Console.WriteLine("\t-----------------------");
                return;
            }
            
        }

        public async Task SearchFriendAsync(string filePath, string username)
        {
            Console.WriteLine("\t--------------------------------------");
            Console.WriteLine("\tEnter Friend's Name to Search: ");
            Console.WriteLine("\t--------------------------------------");
            string friendNameToSearch = Console.ReadLine();
            bool found = false;

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] friendData = line.Split(':');
                        if (friendData.Length == 2 &&
                            friendData[0].Trim() == username &&
                            friendData[1].Trim() == friendNameToSearch)
                        {
                            Console.WriteLine($"Friend '{friendNameToSearch}' found.");
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("\t-----------------------");
                    Console.WriteLine("\tFriend not found.");
                    Console.WriteLine("\t-----------------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t------------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-------------------------------------------------------------");
            }
        }

        public async Task DisplayAllFriendsAsync(string filePath, string username)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    bool hasFriends = false;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] friendData = line.Split(':');
                        if (friendData.Length == 2 && friendData[0].Trim() == username)
                        {
                            Console.WriteLine($"Friend: {friendData[1].Trim()}");
                            hasFriends = true;
                        }
                    }

                    if (!hasFriends)
                    {
                        Console.WriteLine("\t--------------------");
                        Console.WriteLine("\tNo friends found.");
                        Console.WriteLine("\t---------------------");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t-------------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while reading the file: {e.Message}");
                Console.WriteLine("\t-------------------------------------------------------------");
            }
        }

        public async Task DeleteFriendAsync(string filePath, string username)
        {
            Console.WriteLine("\t--------------------------------------");
            Console.WriteLine("\tEnter Friend's Name to Delete: ");
            Console.WriteLine("\t--------------------------------------");
            string friendNameToDelete = Console.ReadLine();
            bool found = false;
            string tempFile = Path.GetTempFileName();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                using (StreamWriter writer = new StreamWriter(tempFile))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] friendData = line.Split(':');
                        if (friendData.Length == 2 &&
                            friendData[0].Trim() == username &&
                            friendData[1].Trim() == friendNameToDelete)
                        {
                            found = true;
                            continue;
                        }
                        await writer.WriteLineAsync(line);
                    }
                }

                File.Delete(filePath);
                File.Move(tempFile, filePath);

                if (found)
                {
                    Console.WriteLine("\t------------------------------------");
                    Console.WriteLine("\tFriend deleted successfully.");
                    Console.WriteLine("\t------------------------------------");
                }
                else
                {
                    Console.WriteLine("\t------------------------------------");
                    Console.WriteLine("\tFriend not found.");
                    Console.WriteLine("\t------------------------------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t---------------------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while processing the file: {e.Message}");
                Console.WriteLine("\t---------------------------------------------------------------------");
            }
        }

        public async Task CheckHasFriendAsync(string filepath, string username)
        {
            hasFriend = false;

            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        string[] friendData = line.Split(':');
                        if (friendData.Length == 2 && friendData[0].Trim() == username)
                        {
                            hasFriend = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t------------------------------------------------------------------");
                Console.WriteLine($"\tAn error occurred while checking friends: {e.Message}");
                Console.WriteLine("\t------------------------------------------------------------------");
            }
        }

        public bool AuthenticateHasFriend()
        {
            return hasFriend;
        }

        public static async Task MenuAsync(string loggedInUsername)
        {
            Friends friendsInstance = new Friends();
            string filePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friends.txt";
            string loginFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\login.txt"; 
            string friendFilePath = "D:\\.Networking_programing\\C#\\practice.project1\\library\\library\\friendnotification.txt"; 

            bool flag = true;
            int choice;

            while (flag)
            {
                Console.WriteLine("\t------------------------------------------------");
                Console.WriteLine("\n\t" +
                    "Press 1 to Add Friend\n\t" +
                    "Press 2 to Search Friend\n\t" +
                    "Press 3 to Delete Friend\n\t" +
                    "Press 4 to Display All Friends\n\t" +
                    "Press 5 to see friend request\n\t" +
                    "Press 6 to Exit");
                Console.WriteLine("\t------------------------------------------------");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("\t-----------------------------------");
                    Console.WriteLine("\tInvalid choice, please try again.");
                    Console.WriteLine("\t-----------------------------------");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        await friendsInstance.AddFriendAsync(loginFilePath, filePath, loggedInUsername , friendFilePath);
                        break;
                    case 2:
                        await friendsInstance.SearchFriendAsync(filePath, loggedInUsername);
                        break;
                    case 3:
                        await friendsInstance.DeleteFriendAsync(filePath, loggedInUsername);
                        break;
                    case 4:
                        await friendsInstance.DisplayAllFriendsAsync(filePath, loggedInUsername);
                        break;
                    case 5:
                        await friendsInstance.SendnotificationAsync(friendFilePath, loggedInUsername);
                        break;
                    case 6:
                        Console.WriteLine("\t-------------------------------");
                        Console.WriteLine("\tExiting Friends Menu...");
                        Console.WriteLine("\t-------------------------------");
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("\t-----------------------------------");
                        Console.WriteLine("\tInvalid choice, please try again.");
                        Console.WriteLine("\t-----------------------------------");
                        break;
                }
            }
        }
    }
}
