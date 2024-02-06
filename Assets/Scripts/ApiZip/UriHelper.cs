using System;
using System.IO;

namespace ZFGinc.WorldOfCubes
{
    public class UriHelper
    {
        public static string GetFileName(string url)
        {
            string file = string.Empty;
            Uri u = new Uri(url);
            file = Path.GetFileName(u.AbsolutePath);
            return file;
        }
    }
}