using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DiscUtils;

namespace PythLR
{
    internal static class Archive
    {
        public static Stream CollectFilesToArchive(this IEnumerable<DiscFileInfo> files,
            string archivePath)
        {
            var archiveFile = new FileInfo(archivePath);
            Directory.CreateDirectory(archiveFile.Directory.FullName);
            var outStream = File.Open(archivePath, FileMode.Create, FileAccess.ReadWrite);
            using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    WriteFileToArchive(archive, file);
                }
            }
            return outStream;
        }

        private static void WriteFileToArchive(ZipArchive archive, DiscFileInfo file)
        {
            Console.WriteLine("Collecting File: {0}", file.FullName);
            using (var stream = file.Open(FileMode.Open, FileAccess.Read))
            {
                WriteStreamToArchive(archive, file.FullName, stream);
            }
        }

        private static void WriteStreamToArchive(ZipArchive archive, string entryName, Stream stream)
        {
            var entry = archive.CreateEntry(entryName, CompressionLevel.Fastest);
            using (var writer = entry.Open())
            {
                stream.CopyTo(writer);
            }
        }
    }
}