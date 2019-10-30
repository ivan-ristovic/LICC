using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using RICC.Adapters;
using RICC.Context;
using Serilog;

namespace RICC.Core
{
    public class SourceCodeParser : IParseEventHandler
    {
        public ParseResult? Result { get; set; }


        public SourceCodeParser()
        {
            this.Result = null;
        }


        public ParseResult? Parse(string path)
        {
            Log.Debug("Reading source at: {Path}", path);
            this.Result = new ParseResult();
            var listener = ParserListener.ForFile(path);
            Parser parser = listener.CreateParser(path);
            listener.ListenParse(parser);
            return this.Result;
        }


        public void TranslationUnitEnter(object? sender, EnterTranslationUnitEventArgs? e)
        {
            Log.Debug("Translation unit enter event fired");
        }

        public void TranslationUnitLeave(object? sender, LeaveTranslationUnitEventArgs? e)
        {
            Log.Debug("Translation unit leave event fired");
        }
    }
}
