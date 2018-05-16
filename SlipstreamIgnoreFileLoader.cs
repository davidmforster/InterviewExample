using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommBank.Api.Provisioning.Services.Source
{
    /// <summary>
    /// Provides the ability to load .ssignore file content, if one is present in the scaffold.
    /// </summary>
    public interface ISlipstreamIgnoreFileLoader
    {
        /// <summary>
        /// Loads .ssignore content as individual rows.
        /// </summary>
        /// <param name="path">The path to folder where .ssignore file is located.</param>
        /// <returns>Non empty entries from the .ssignore file; <see langword="null"/> if the file can't be loaded.</returns>
        List<string> LoadEntries(string path);
    }

    /// <summary>
    /// Loads .ssignore file content, if one is present in the scaffold.
    /// </summary>
    public sealed class SlipstreamIgnoreFileLoader : ISlipstreamIgnoreFileLoader
    {
        public const string IgnoreFileName = ".ssignore";


        /// <summary>
        /// Loads .ssignore content as individual rows.
        /// </summary>
        /// <param name="path">The path to folder where .ssignore file is located.</param>
        /// <returns>Non empty entries from the .ssignore file; <see langword="null"/> if the file can't be loaded.</returns>
        public List<string> LoadEntries(string path)
        {
            try
            {
                var fullPath = Path.Combine(path, IgnoreFileName);
                if (!File.Exists(fullPath))
                {
                    return null;
                }

                var reader = File.OpenText(fullPath);
                return reader.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
