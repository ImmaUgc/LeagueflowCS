using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text.Unicode;

namespace LeagueflowCS
{
    public class LockfileContent
    {
        public string Authorization { get; set; }
        public int Port { get; set; }
    }
    public class Lockfile
    {
        public LockfileContent lockfile;

        public Lockfile()
        {
            Process[] ClientProcesses = Process.GetProcessesByName("LeagueClient");

            if (ClientProcesses == null)
            {
                Console.WriteLine("LeagueClient.exe not found");
                return;
            }

            Process ClientProcess = ClientProcesses[0];
            string LockfilePath = ClientProcess.MainModule.FileName;

            List<string> ListLockfilePath = LockfilePath.Split('\\').ToList<string>();
            ListLockfilePath.RemoveAt(ListLockfilePath.Count - 1);
            ListLockfilePath.Add("lockfile");
            LockfilePath = string.Join('/', ListLockfilePath);

            using (FileStream LockfileStream = new(LockfilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader Reader = new(LockfileStream))
                {
                    string Content = Reader.ReadToEnd();
                    string[] SplitContent = Content.Split(':');
                    lockfile = new LockfileContent
                    {
                        Authorization = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"riot:{SplitContent[3]}")),
                        Port = int.Parse(SplitContent[2])
                    };
                }
            }
        }
    }
}