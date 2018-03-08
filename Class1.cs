using System.Collections.Generic;

namespace CommBank.Api.Provisioning.Services.Source.Templates
{
    public interface ISourceTemplate
    {
        string Name { get; }
        string RepositoryUrl { get; }
        string SolutionName { get; }
        string BranchName { get; }
        IDictionary<string, string> TemplatedValues { get; }
        void Apply(string workingDirectoryPath, string newSolutionName, string templateSolutionName, IDictionary<string, string> newTemplatedValues);
    }
}
