using System.Collections.Generic;
using System.IO;

namespace PandaChatServer.Class
{
    public class DataBase
    {
        private FileInfo fileDB { get; set; }
        private string[] fileStrings { get; set; }
        private bool isCached { get; set; }

        public DataBase(FileInfo file)
        {
            fileDB = file;
            CachedFile();
            isCached = true;
        }

        private void CachedFile()
        {
            fileStrings = null;
            fileStrings = File.ReadAllLines(fileDB.FullName);
        }

        public Dictionary<string, string> ValidateUser(string Login, string Password)
        {
            Dictionary<string, string> userDataBools = new Dictionary<string, string>();
            if (!isCached)
                CachedFile();
            userDataBools.Add("ValidateLogin", "false");
            userDataBools.Add("ValidateData", "false");
            userDataBools.Add("IsBan", "false");
            userDataBools.Add("ReasonBan", "");
            userDataBools.Add("isAdmin", "false");
            foreach (var userInfo in fileStrings)
            {
                string[] buffer = userInfo.Split('/');
                if (buffer[0] == Login)
                {
                    userDataBools["ValidateLogin"] = "true";
                    if (buffer[1] == Password)
                    {
                        userDataBools["ValidateData"] = "true";
                        if (buffer[2] == "banFalse")
                            userDataBools["IsBan"] = "false";
                        else
                        {
                            userDataBools["IsBan"] = "true";
                            userDataBools["ReasonBan"] = buffer[3];
                            return userDataBools;   // забанен
                        }
                        if (buffer[3] == "adminTrue")
                            userDataBools["isAdmin"] = "true";
                        else
                            userDataBools["isAdmin"] = "false";
                    }
                    break;
                }
            }
            return userDataBools;
        }

        public void Add_User(string newDataUser)
        {
            StreamWriter addUser = fileDB.AppendText();
            addUser.WriteLine(newDataUser);
            addUser.Close();
            isCached = false;
        }

        public void Update_User(string oldDataUser , string updateDataUser)
        {
            File.Copy(fileDB.FullName, fileDB.FullName + ".bak");
            StreamWriter updater = new StreamWriter(fileDB.FullName);
            StreamReader reader = new StreamReader(fileDB.FullName);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (oldDataUser == line)
                {
                    updater.WriteLine(updateDataUser);
                    continue;
                }
                updater.WriteLine(line);
            }
            updater.Close();
            reader.Close();
            updater = null;
            reader = null;
            isCached = false;
        }
    }
}
