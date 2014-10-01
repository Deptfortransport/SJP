// *********************************************** 
// NAME             : ChecksumHelper.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Provides file checksum validation 
//                    functions for validator and generator
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace SJP.Common.ChecksumUtils
{

    public class ChecksumHelper
    {

        // DES Encryption key - Must be 64 bits, 8 bytes - we'll need this for either encrypt or decrypt
        // Distribute this key to the user who will decrypt this file.
        public const string SecretKey = "§§Y????q";

        // Function to Generate a 64 bits Key.
        public string GenerateKey()
        {
            string retStr;
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            using (DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create())
            {
                // Use the Automatically generated key for Encryption. 
                retStr = ASCIIEncoding.ASCII.GetString(desCrypto.Key);
            }
            return retStr;
        }

        public string GetHashFileNameForDir(DirectoryInfo dirHashed)
        {
            string sEncryptedHashFileName;
            try
            {
                //Generate an encryptedOutputFile using the root dir path as part of the filename
                //use regex to strip out spl chars
                string rgPattern = @"[\\\/:\*\?""<>|]";
                Regex oRegex = new Regex(rgPattern);
                sEncryptedHashFileName = oRegex.Replace(dirHashed.FullName, "-") + "-EncryptedHashFile.txt";
            }
            catch
            {
                //on error just return an arbitrary filename
                sEncryptedHashFileName = "EncryptedHashFile.txt";
            }
            return sEncryptedHashFileName;

        }
        //Pass in a file path and name and i'll return you the SHA265 hash value as a Hexadecimal string
        public static string GenerateHash(string filePathAndName)
        {
            string hashText = "";
            string hexValue = "";

            byte[] fileData = File.ReadAllBytes(filePathAndName);
            using (SHA256 hashSHA256 = SHA256.Create())
            {
                byte[] hashData = hashSHA256.ComputeHash(fileData);

                foreach (byte b in hashData)
                {
                    hexValue = b.ToString("X").ToLower(); // Lowercase for compatibility on case-sensitive systems
                    hashText += (hexValue.Length == 1 ? "0" : "") + hexValue;
                }
            }
            return hashText;
        }

        public static void hashDirectoryContentsToFile(System.IO.DirectoryInfo root, string[] extensionsToIgnore, System.IO.StreamWriter outputFile, ref StringCollection inaccessibleFilesLog)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
            string fileHash;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // Log an error
                inaccessibleFilesLog.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    bool ignoreThisOne = false ;
                    foreach (string ext in extensionsToIgnore )
                    {
                        if (fi.Extension.ToString() == ext)
                        {
                            ignoreThisOne = true;
                        }
                    }
                    if (!ignoreThisOne) 
                    {
                        //if the extension was not on the ignore list then go ahead and include in the hash
                        Console.WriteLine(fi.FullName);
                        fileHash = GenerateHash(fi.FullName);
                        Console.WriteLine(fileHash);
                        //write the hash value to a new line in the output file
                        outputFile.WriteLine(fileHash);
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    hashDirectoryContentsToFile(dirInfo,extensionsToIgnore, outputFile, ref inaccessibleFilesLog);
                }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        public void EncryptFile(string sInputFilename,string sOutputFilename,string sKey)
        {
            // For additional security Pin the key.
            GCHandle gch = GCHandle.Alloc(sKey, GCHandleType.Pinned);

            using (FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read))
            {
                using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
                {
                    DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    ICryptoTransform desencrypt = DES.CreateEncryptor();

                    using (FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write))
                    {
                        CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                        byte[] bytearrayinput = new byte[fsInput.Length];
                        fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                    }
                }
            }

            // Remove the Key from memory. 
            NativeMethods.ZeroMemory(gch.AddrOfPinnedObject(), sKey.Length * 2);
            gch.Free();
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        public void DecryptFile(string sInputFilename,string sOutputFilename,string sKey)
        {
            // For additional security Pin the key.
            GCHandle gch = GCHandle.Alloc(sKey, GCHandleType.Pinned);

            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                //A 64 bit key and IV is required for this provider.
                //Set secret key For DES algorithm.
                desProvider.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                //Set initialization vector.
                desProvider.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                //Create a file stream to read the encrypted file back.
                using (FileStream fsread = new FileStream(sInputFilename,FileMode.Open,FileAccess.Read))
                {
                    //Create a DES decryptor from the DES instance.
                    ICryptoTransform desdecrypt = desProvider.CreateDecryptor();
                    //Create crypto stream set to read and do a 
                    //DES decryption transform on incoming bytes.
                    CryptoStream cryptostreamDecr = new CryptoStream(fsread,desdecrypt,CryptoStreamMode.Read);
                    //Print the contents of the decrypted file to the output file - overwrite if already exists.
                    using (StreamWriter fsDecrypted = new StreamWriter(sOutputFilename, false))
                    {
                        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                        fsDecrypted.Flush();
                    }
                }
            }
            // Remove the Key from memory. 
            NativeMethods.ZeroMemory(gch.AddrOfPinnedObject(), sKey.Length * 2);
            gch.Free();
        }
    }
}