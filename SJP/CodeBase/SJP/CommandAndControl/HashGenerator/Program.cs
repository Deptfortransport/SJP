// *********************************************** 
// NAME             : WMIMonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of HashGenerator utility
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using SJP.Common.ChecksumUtils;
using System.Threading;


namespace SJP.Common.HashGenerator
{
    class Program
    {
        static System.Collections.Specialized.StringCollection inaccessibleFilesLog = new System.Collections.Specialized.StringCollection();

        //Me help you long time
        private void displayHelp()
        {
            // The text below is displayed to the user showing how a valid call can be constructed
            Console.WriteLine("Usage: SJP.Common.HashGenerator [root directory] [/help] \n\n");
            Console.WriteLine("root directory - The root directory containing the files and subdirectories to be hashed");
            Console.WriteLine("/help - Displays this help text and performs no further activity");
        }


        //Main - iterates through files in the root directory passed in as a parameter building up the hashes
        static void Main(string[] args)
        {

            // Flag to indicate whether command line parameters are valid
            bool invalidParameters = false;

            string rootDir = string.Empty;
            string[] extensionsToIgnore = new string[0];
            string algorithm = string.Empty;
            string params1 = string.Empty;
            string params2 = string.Empty;
            char[] stringSeperator = { '|' };

            Program hashGen = new Program();

            #if Debug
                        Thread.Sleep(10000); //DEBUG - sleep for 10 secs so i can attach!
            #endif

            if (args.Length < 1 || args.Length > 2)
            {
                invalidParameters = true;
                Console.WriteLine("Error. Invalid number of parameters provided: " + args.Length);
            }
            else
            {
                #region DealWithCommandLineArgs
                // Iterate through each of the command line arguments
                foreach (string s in args)
                {
                    Console.WriteLine("Parameter: [" + s + "]");
                    if (s.StartsWith("/"))
                    {
                        s.ToLower();
                        string[] formattedArg = s.Split(stringSeperator, 2);
                        switch (formattedArg[0])
                        {
                            case "/help":
                                //setting invalid params flag will cause help to show anyway
                                invalidParameters = true;
                                break;
                            case "/algorithm":
                                if (formattedArg.Length == 2)
                                {
                                    algorithm = formattedArg[1];
                                }
                                else
                                {
                                    Console.WriteLine("Error. Unable to split directive properly. Was split into " + formattedArg.Length + " parts ");
                                    invalidParameters = true;
                                }
                                break;
                            default:
                                Console.WriteLine("Error. Unrecognised directive: " + formattedArg[0]);
                                invalidParameters = true;
                                break;
                        }
                    }
                    else
                    {
                        if (s.Equals(args[0]))
                        {
                            rootDir = s;
                        }
                        if (s.Equals(args[1]))
                        {
                            s.ToLower();
                            extensionsToIgnore = s.Split(stringSeperator);
                        }
                    }
                }
                #endregion
            }
            if (invalidParameters == false)
            {
                // All parameters have been validated so go through the passed in directory   
                System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(rootDir);
   
                try
                {
                    //Generate the hash file.
                    Console.WriteLine("Generating hash file");

                    using (System.IO.StreamWriter hashOutputFile = new System.IO.StreamWriter("OutputFile.txt", false))
                    {
                        SJP.Common.ChecksumUtils.ChecksumHelper.hashDirectoryContentsToFile(root,extensionsToIgnore, hashOutputFile, ref inaccessibleFilesLog);
                    }

                    // Write out all the files that could not be processed to the console for info.
                    Console.WriteLine("Files with restricted access:");
                    foreach (string s in inaccessibleFilesLog) { Console.WriteLine(s); }

                    //Now encrypt the hash file.
                    Console.WriteLine("Encrypting the hash file");
                    ChecksumHelper ChecksumHelper = new ChecksumHelper();
                    Console.WriteLine("Key used for generation: " + ChecksumHelper.SecretKey);

                    ChecksumHelper.EncryptFile("OutputFile.txt", ChecksumHelper.GetHashFileNameForDir(root), ChecksumHelper.SecretKey);

                    //Now delete the unencrypted one
                    File.Delete("OutputFile.txt");
                }
                catch (Exception e)
                {
                    { Console.WriteLine("Error - unable to verify files: " + e.Message); }
                }
                finally
                {
                    //Now delete the temp files
                    File.Delete("TempOutputFile.txt");
                    File.Delete("DecryptedHashFile.txt");
                }
                // Keep the console window open in debug mode.
                Console.WriteLine("Press any key");
                Console.ReadKey();
            }
            else
            {
                // Invalid params
                hashGen.displayHelp();
            }
        }
    }
}