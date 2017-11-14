String file_name = @"D:/py.py";
if (!System.IO.File.Exists(file_name))
{
    using (System.IO.FileStream fs = System.IO.File.Create(file_name))
    {
        Byte[] info = new UTF8Encoding(true).GetBytes("print(\"hello\")\nprint(\"new line\")\ninput(\"waiting\")");
        fs.Write(info, 0, info.Length);
    }
}


String cmdText = "D:/py.py";
Process cmd = new Process();
cmd.StartInfo = new ProcessStartInfo(@"C:/Users/zjw19/Anaconda3/python.exe", cmdText)
{
    RedirectStandardOutput = false,
    UseShellExecute = false,
    CreateNoWindow = false
};
cmd.Start();
