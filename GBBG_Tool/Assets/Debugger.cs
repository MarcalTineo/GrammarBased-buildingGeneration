
using System.IO;

public class Debugger
{
   
    public static void Log(string message)
    {
        StreamWriter sw = new StreamWriter("Assets/Resources/Log.txt", true);
        sw.WriteLine($"[{System.DateTime.Now}] {message}");
        sw.Close();
    }

    public static void Log(int i)
    {
        Log(i.ToString());
    }
}
