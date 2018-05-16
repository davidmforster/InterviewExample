using System;
using System.Collections.Generic;
using System.Linq;

namespace CommBank.Api.Provisioning.Services.Source
{
    /// <summary>
    /// For a given scaffold folder provides the ability to retrieve items which are to be ignored during the provisioning.
    /// </summary>
    public interface ITemplateIgnoreContentProvider
    {
        List<string> Get(string scaffoldPath);
    }

    /// <summary>
    /// For a given scaffold folder provides items which are to be ignored during the provisioning.
    /// Template ignores will be read from .ssignore file, if the scaffold contains it.
    /// Otherwise the default set of ignore entries is provided.
    /// </summary>
    public sealed class TemplateIgnoreContentProvider : ITemplateIgnoreContentProvider
    {
        private static readonly List<string> DefaultIgnoreDirs = new List<string> { ".git", "node_modules", "bin", "obj" };
        private readonly ISlipstreamIgnoreFileLoader _slipStreamIgnoreFileLoader;


        public TemplateIgnoreContentProvider(ISlipstreamIgnoreFileLoader slipStreamIgnoreFileLoader)
        {
            _slipStreamIgnoreFileLoader = slipStreamIgnoreFileLoader;
        }


        public List<string> Get(string scaffoldPath)
        {
            if (string.IsNullOrWhiteSpace(scaffoldPath))
            {
                throw new ArgumentNullException(nameof(scaffoldPath));
            }

            var content = _slipStreamIgnoreFileLoader.LoadEntries(scaffoldPath);
            if (content == null)
            {
                // .templateignore file either not found or can't be read.
                // provide default ignores to retain the existing behavior
                return DefaultIgnoreDirs;
            }
            else
            {
                return content.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Select(x => x.Trim()).ToList();
            }
        }
    }
}
