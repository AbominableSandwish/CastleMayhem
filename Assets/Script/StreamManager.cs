using UnityEngine;
using System.IO;
class StreamManager
{
    public static string readTextFile(string fileName, string file_path = "")
    {
        string data = "";
        if (file_path == "")
        {
            file_path = Application.persistentDataPath;
        }

        StreamReader inp_stm = new StreamReader(file_path + "/" + fileName);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            data += inp_ln + '\n';
        }

        inp_stm.Close();

        return data;
    }

    public static void WriteToFile(string fileName, string data, string file_path = "")
    {
       
        if (file_path == "")
        {
            file_path = Application.persistentDataPath; 
        }

        Debug.Log(file_path);
        StreamWriter sw = new StreamWriter(Application.persistentDataPath +  "/" + fileName);
        sw.WriteLine(data);
        sw.Close();
    }
}