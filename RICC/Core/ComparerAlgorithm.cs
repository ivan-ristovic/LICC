using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using RICC.Adapters;
using Serilog;

namespace RICC.Core
{
    public sealed class ComparerAlgorithm
    {
        private readonly string specPath;
        private readonly string testPath;


        public ComparerAlgorithm(string specPath, string testPath)
        {
            if (!File.Exists(specPath) || !File.Exists(testPath))
                throw new FileNotFoundException("One of the provided paths points to a file that does not exist.");
            this.specPath = specPath;
            this.testPath = testPath;
        }


        public void Execute()
        {
            ParseResult specResult = this.Parse(this.specPath);
            ParseResult testResult = this.Parse(this.testPath);
            this.Compare(specResult, testResult);
        }


        private ParseResult Parse(string path)
        {
            var sourceParser = new SourceCodeParser();
            return sourceParser.Parse(path) ?? throw new NullReferenceException("Result is incomplete.");
        }

        private void Compare(ParseResult specResult, ParseResult testResult)
        {
            // TODO
            Log.Debug("Comparing...");
        }
    }
}
