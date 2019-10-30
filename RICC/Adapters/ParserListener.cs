using System;
using System.IO;
using Antlr4.Runtime;
using RICC.Adapters.C;
using RICC.Context;

namespace RICC.Adapters
{
    public abstract class ParserListener
    {
        public static ParserListener ForFile(string path)
        {
            var fi = new FileInfo(path);
            return fi.Extension switch
            {
                ".c" => new CListener(),
                _ => throw new ArgumentException("Unsupported file extension"),
            };
        }


        public abstract Parser CreateParser(string path);
        public abstract void ListenParse(Parser parser);


        protected ParserListener()
        {

        }


        public event EventHandler<EnterTranslationUnitEventArgs>? TranslationUnitEnterEvent;
        public event EventHandler<LeaveTranslationUnitEventArgs>? TranslationUnitLeaveEvent;

        protected virtual void OnEnterTranslationUnit(EnterTranslationUnitEventArgs e) => this.TranslationUnitEnterEvent?.Invoke(this, e);
        protected virtual void OnLeaveTranslationUnit(LeaveTranslationUnitEventArgs e) => this.TranslationUnitLeaveEvent?.Invoke(this, e);
    }
}
