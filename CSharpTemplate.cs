﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommBank.Api.Provisioning.IO;

namespace CommBank.Api.Provisioning.Services.Source.Templates
{
    public class CSharpTemplate : ISourceTemplate
    {
        private static readonly string[] DefaultIgnoreDirs = { ".git", "node_modules", "bower_components", "packages", "bin", "obj" };

        private readonly string _name;
        private readonly string _repositoryUrl;
        private readonly string _solutionName;
        private readonly string _branchName;
        private readonly IDictionary<string, string> _templatedValues;
        private readonly string[] _ignoreDirs;

        public string Name
        {
            get { return _name; }
        }

        public string RepositoryUrl
        {
            get { return _repositoryUrl; }
        }

        public string SolutionName
        {
            get { return _solutionName; }
        }

        public string BranchName
        {
            get { return _branchName; }
        }

        public IDictionary<string, string> TemplatedValues
        {
            get { return _templatedValues; }
        }


        public CSharpTemplate(string name, string repositoryUrl, string solutionName, string branchName)
            : this(name, repositoryUrl, solutionName, branchName, new Dictionary<string, string>(), DefaultIgnoreDirs)
        {

        }

        public CSharpTemplate(string name, string repositoryUrl, string solutionName, string branchName, IDictionary<string, string> templatedValues)
            : this(name, repositoryUrl, solutionName, branchName, templatedValues, DefaultIgnoreDirs)
        {

        }

        public CSharpTemplate(string name, string repositoryUrl, string solutionName, string branchName,
            IDictionary<string, string> templatedValues, IEnumerable<string> ignoreDirs)
        {
            _name = name;
            _repositoryUrl = repositoryUrl;
            _solutionName = solutionName;
            _branchName = branchName;
            _templatedValues = templatedValues;
            _ignoreDirs = ignoreDirs.ToArray();
        }

        public void Apply(string workingDirectoryPath, string newSolutionName, string templateSolutionName, IDictionary<string, string> newTemplatedValues)
        {
            var templateSolnName = templateSolutionName ?? SolutionName;
            templateSolnName = templateSolnName.ToLower();

            RenameGitFoldersAndFiles(workingDirectoryPath, templateSolnName, newSolutionName);
            RenameGitFoldersAndFiles(workingDirectoryPath, newTemplatedValues);

            var replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {templateSolnName, newSolutionName}
            };

            var templatedReplacements = TemplatedValues.Select(x => new KeyValuePair<string, string>(x.Value, newTemplatedValues[x.Key]));
            foreach (var replacement in templatedReplacements)
            {
                replacements.Add(replacement.Key, replacement.Value);
            }

            FindReplaceInFile(workingDirectoryPath, replacements);
        }

        private void RenameGitFoldersAndFiles(string workingDirectoryPath, IDictionary<string, string> templatedValues)
        {
            if (templatedValues != null)
            {
                foreach (var item in templatedValues)
                {
                    RenameGitFoldersAndFiles(workingDirectoryPath, item.Key, item.Value);
                }
            }
        }

        private void RenameGitFoldersAndFiles(string workingDirectoryPath, string oldName, string newSolutionName)
        {
            var workingDir = new DirectoryInfo(workingDirectoryPath);

            var fileSystemInfos = workingDir
                .EnumerateFiles("*", _ignoreDirs)
                .Where(x => x.Name.ToLower().Contains(oldName))
                .OrderByDescending(x => x.FullName.Count(y => y == Path.PathSeparator))
                .ThenByDescending(x => x.FullName.Length)
                .ToArray();

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                // not important for the interview
                // do something useful
            }
        }

        private IEnumerable<Guid> FindProjectGuids(string workingDirectoryPath)
        {
            var workingDir = new DirectoryInfo(workingDirectoryPath);

            var fileInfos = workingDir
                .EnumerateFiles("*.csproj", _ignoreDirs);

            return Enumerable.Empty<Guid>();
        }

        private void FindReplaceInFile(string workingDirectoryPath, IDictionary<string, string> replacements)
        {
            var workingDirectory = new DirectoryInfo(workingDirectoryPath);
            var files = workingDirectory.EnumerateFiles("*.*", _ignoreDirs)
                //We want to ignore contents of the "packages" folder, *except* for the repositories.config file
                .Union(workingDirectory.EnumerateFiles("repositories.config", SearchOption.AllDirectories));

            foreach (var fileInfo in files)
            {
                // not important for the interview
                // do something useful
            }
        }
    }
}