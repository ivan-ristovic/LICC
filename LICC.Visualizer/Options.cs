using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace LICC.Visualizer
{
    internal sealed class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Value(0, Required = true, HelpText = "Specification path.")]
        public string Source { get; set; } = "";
    }
}
