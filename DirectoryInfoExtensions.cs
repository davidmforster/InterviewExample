using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommBank.Api.Provisioning.IO
{
    public static class DirectoryInfoExtensions
    {
        public static void MoveTo(this FileSystemInfo fileSystemInfo, string newPath)
        {
            if (fileSystemInfo == null)
            {
                throw new ArgumentNullException("fileSystemInfo");
            }
            if (fileSystemInfo is FileInfo)
            {
                ((FileInfo)fileSystemInfo).MoveTo(newPath);
            }
            else if (fileSystemInfo is DirectoryInfo)
            {
                ((DirectoryInfo)fileSystemInfo).MoveTo(newPath);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void ForceDelete(this DirectoryInfo directoryInfo)
        {
            directoryInfo.Attributes = FileAttributes.Normal;

            foreach (var info in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directoryInfo.Delete(true);
        }

        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directoryInfo, string searchPattern, IEnumerable<string> excludeDirs)
        {
            return directoryInfo.TreeSearch(excludeDirs, x => x.EnumerateFiles(searchPattern));
        }

        private static IEnumerable<T> TreeSearch<T>(this DirectoryInfo directoryInfo,
            IEnumerable<string> excludeDirs, Func<DirectoryInfo, IEnumerable<T>> action)
        {
            excludeDirs = excludeDirs.ToArray();
            var queue = new Queue<DirectoryInfo>();
            queue.Enqueue(directoryInfo);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var subDir in current.EnumerateDirectories())
                {
                    if (!excludeDirs.Any(e => e.Equals(subDir.Name)))
                    {
                        queue.Enqueue(subDir);
                    }
                }

                var results = action(current);
                foreach (var result in results)
                {
                    yield return result;
                }
            }
        }

    }
}