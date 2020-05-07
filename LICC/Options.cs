using CommandLine;

namespace LICC
{
    [Verb("cmp", HelpText = "Compare source against the specification source")]
    internal sealed class CompareOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Value(0, Required = true, HelpText = "Specification path.")]
        public string? Source { get; set; }

        [Value(1, Required = true, HelpText = "Test source path.")]
        public string? Destination { get; set; }
    }
    
    [Verb("ast", HelpText = "AST generation commands")]
    internal sealed class ASTOptions
    {
        [Option('v', "verbose", Default = false, Required = false, HelpText = "Verbose output")]
        public bool Verbose { get; set; }

        [Option('t', "tree", Default = false, Required = false, HelpText = "Show AST tree")]
        public bool Tree { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output path")]
        public string? OutputPath { get; set; }
        
        [Option('c', "compact", Default = false, Required = false, HelpText = "Compact AST output")]
        public bool Compact { get; set; }

        [Value(0, Required = true, HelpText = "Source path")]
        public string? Source { get; set; }
    }
}
