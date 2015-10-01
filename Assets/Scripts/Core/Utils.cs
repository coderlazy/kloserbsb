using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public static class Utils {

    private static MD5 md5 = MD5.Create();

    public static string Md5File(string fileName) {
        
        string fullPersistentPath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        FileStream fs = File.OpenRead(fullPersistentPath);
        string md5string = BitConverter.ToString(md5.ComputeHash(fs)).Replace("-","").ToLower();
        
        fs.Close();
        
        return md5string;
    }
    
    public static string Md5(string strToEncrypt) {
        
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        
        string hashString = "";
        
        for (int i = 0; i < hashBytes.Length; i++)
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        
        return hashString;
    }

	public static string GetPersistentDataFromFile(string fileName)
    {
        string fullPersistentPath = System.IO.Path.Combine(Application.persistentDataPath, fileName);

        if (System.IO.File.Exists(fullPersistentPath))
        {
            var sr = new System.IO.StreamReader(fullPersistentPath);
            string fileContents = sr.ReadToEnd();
            sr.Close();

            return fileContents;
        }
        else
            return null;

    }

    public static string ConvertTo_ProperCase(string text)
    {
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        return myTI.ToTitleCase(text.ToLower());
    }
}
