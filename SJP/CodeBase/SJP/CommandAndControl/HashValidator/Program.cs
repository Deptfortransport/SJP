// *********************************************** 
// NAME             : WMIMonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of HashValidator utility
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using SJP.Common.ChecksumUtils;
using System.Threading;


namespace SJP.Common.HashValidator
{
    class Program
    {
        static System.Collections.Specialized.StringCollection inaccessibleFilesLog = new System.Collections.Specialized.StringCollection();

        private void displayHelp()
        {
            // The text below is displayed to the user showing how a valid call can be constructed
            Console.WriteLine("Usage: SJP.Common.HashValidator [root directory] [/help]");
            Console.WriteLine("[root directory] - The root directory containing the files and subdirectories to be checked");
            Console.WriteLine("NB The directory specified to check must have had the WebSupportHashGenerator application run" +
            "against it first, and the generated encrypted hash file must be in the directory from which the " +
            "SJP.Common.HashValidator application is run.");
            Console.WriteLine("[/help] - Displays this help text and performs no further activity");
        }

        //Main - iterates through files in the root directory passed in as a parameter building up the hashes
        static int Main(string[] args)
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

                bool hashesMatch = false;
                ChecksumHelper ChecksumHelper = new ChecksumHelper();
                string sOrigEncryptedHashFileName = ChecksumHelper.GetHashFileNameForDir(root);
                string sComparedEncryptedHashFileName = "Compared-" + sOrigEncryptedHashFileName;

                try
                {
                    //Verify the passed in target directory against the hash file in the current directory and key 
                    Console.WriteLine("Key used for verification: " + ChecksumHelper.SecretKey);
                    //ChecksumHelper.DecryptFile(ChecksumHelper.GetHashFileNameForDir(root), "DecryptedHashFile.txt", ChecksumHelper.SecretKey);

                    //Now compare the target directory and validate - regen an encrypted hash file and compare to the original one
                    using (System.IO.StreamWriter hashOutputFile = new System.IO.StreamWriter("TempOutputFile.txt", true))
                    {
                        ChecksumHelper.hashDirectoryContentsToFile(root,extensionsToIgnore, hashOutputFile, ref inaccessibleFilesLog);
                    }

                    ChecksumHelper.EncryptFile("TempOutputFile.txt", sComparedEncryptedHashFileName, ChecksumHelper.SecretKey);
                    //hashOutputFile.Close();

                    int file1byte, file2byte;
                    
                    // Open the two files.
                    using (FileStream fs1 = new FileStream(sOrigEncryptedHashFileName, FileMode.Open))
                    {
                        using (FileStream fs2 = new FileStream(sComparedEncryptedHashFileName, FileMode.Open))
                        {
                            // Check the file sizes. If they are not the same, the files 
                            // are not the same.
                            if (fs1.Length != fs2.Length)
                            {
                                hashesMatch = false;
                            }
                            else
                            //sizes check out so continue comparison
                            {
                                // Read and compare a byte from each file until either a non-matching set of bytes is 
                                // found or until the end of file1 is reached.
                                do
                                {
                                    // Read one byte from each file.
                                    file1byte = fs1.ReadByte();
                                    file2byte = fs2.ReadByte();
                                }
                                while ((file1byte == file2byte) && (file1byte != -1));

                                // Return the success of the comparison. "file1byte" is 
                                // equal to "file2byte" at this point only if the files are 
                                // the same.
                                if ((file1byte - file2byte) == 0)
                                { hashesMatch = true; }
                                else
                                { hashesMatch = false; }
                            }
                        }
                    }

                    if (hashesMatch)
                    { 
                        Console.WriteLine("Hashes Match ");
                        return 0;
                    }
                    else
                    { 
                        Console.WriteLine("Hashes Do Not Match ");
                        return 2;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error - unable to verify files: " + e.Message);
                    return 1;
                }
                finally
                {
                    //Now delete the temp file as this is the unencrypted hashes!
                    File.Delete("TempOutputFile.txt");
                }

            }
            else
            {
                // Invalid params
                hashGen.displayHelp();
                return 2;
            }
        }
    }
}