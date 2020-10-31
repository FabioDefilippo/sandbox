using System.IO;
using System.Security;
using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Security.Policy;

namespace sandbox
{
    class Program
    {
        private static readonly string SandBoxFolder = @"C:\Users\Public\Desktop\";
        private static readonly string[] VARS = new string[] {"OS", "PATHEXT", "PROCESSOR_ARCHITECTURE", "PROCESSOR_IDENTIFIER", "PROCESSOR_LEVEL",
                            "LOCALAPPDATA", "NUMBER_OF_PROCESSORS", "LOGONSERVER", "COMPUTERNAME","APPDATA", "HOMEDRIVE", "PUBLIC", "HOMEPATH",
                            "SystemDrive","SystemRoot","USERNAME", "USERPROFILE", "USERDOMAIN", "USERDOMAIN_ROAMINGPROFILE", "TMP", "TEMP"  };
        static void Main(string[] args)
        {
            if (args.Length.Equals(0) || args.Equals(null))
            {
                Console.Error.WriteLine("sandbox, by FabioDefilippoSoftware");
                Console.Error.WriteLine("arg1=Usename's Domain (example, MyDomain)");
                Console.Error.WriteLine("arg2=Usename (example, Guest)");
                Console.Error.WriteLine("arg3=User password");
                Console.Error.WriteLine("arg4=EXE file (example, C:\\Windows\\System32\\cmd.exe)");
                Console.Error.WriteLine("arg5=Arguments (example, \"/K whoami /all\")");
            }
            else
            {
                Console.Error.WriteLine("Sandbox loading...");
                try
                {
                    int LUN = args.Length;
                    if (LUN.Equals(5))
                    {
                        if (!args[0].Equals(String.Empty) && !args[1].Equals(String.Empty) && !args[3].Equals(String.Empty))
                        {
                            string PROGRAMMA = args[3];
                            string DOMINIO = args[0];
                            string UTENTE = args[1];
                            string PASSWD = args[2];
                            string ARGOMENTI = args[4];
                            Console.Error.WriteLine("checking...");
                            if (File.Exists(PROGRAMMA))
                            {
                                Console.Error.WriteLine("verifying...");

                                Console.Error.WriteLine("setting...");
                                SecureString SS = new SecureString();
                                if (PASSWD.Length > 0)
                                {
                                    foreach (char CARA in PASSWD.ToCharArray())
                                    {
                                        SS.AppendChar(CARA);
                                    }
                                }
                                ProcessStartInfo PSI;
                                PSI = new ProcessStartInfo(PROGRAMMA, ARGOMENTI);
                                PSI.WindowStyle = ProcessWindowStyle.Maximized;
                                PSI.WorkingDirectory = SandBoxFolder;
                                PSI.UseShellExecute = false;
                                PSI.UserName = UTENTE;
                                PSI.Domain = DOMINIO;
                                PSI.Password = SS;
                                foreach (string VAR in VARS)
                                {
                                    PSI.Environment.Remove(VAR);
                                }
                                PSI.Environment.Add("PATHEXT", ".EXE");
                                Process.Start(PSI);
                                Console.Error.WriteLine("Sandbox loaded!");

                            }
                            else
                            {
                                Console.Error.WriteLine("{0} does not exist", PROGRAMMA);
                            }
                        }
                        else
                        {
                            Console.Error.WriteLine("Domain, Username and Program path must be not empty!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
        }
    }
}
