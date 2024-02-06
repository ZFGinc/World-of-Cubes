using UnityEngine;
using System.IO;
using Unity.SharpZipLib.Zip;
using System;

namespace ZFGinc.WorldOfCubes
{
    public class ZipFile
    {
        public static void UnZip(string filePath, byte[] data)
        {
            using (ZipInputStream s = new ZipInputStream(new MemoryStream(data)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        var dirPath = Path.Combine(filePath, directoryName);
                        Directory.CreateDirectory(dirPath);
                    }

                    if (fileName != string.Empty)
                    {
                        var entryFilePath = Path.Combine(filePath, theEntry.Name);
                        using (FileStream streamWriter = File.Create(entryFilePath))
                        {
                            int size = 2048;
                            byte[] fdata = new byte[size];
                            while (true)
                            {
                                size = s.Read(fdata, 0, fdata.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(fdata, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}