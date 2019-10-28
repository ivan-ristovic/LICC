using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using RICC.Context;

namespace RICC.Adapters
{
    public abstract class Listener
    {
        public abstract void Walk(Parser parser);
        public abstract Parser CreateParser(Stream input);


        public event EventHandler<EnterTranslationUnitEventArgs>? TranslationUnitEnterEvent;
        public event EventHandler<LeaveTranslationUnitEventArgs>? TranslationUnitLeaveEvent;

        protected virtual void OnEnterTranslationUnit(EnterTranslationUnitEventArgs e) => this.TranslationUnitEnterEvent?.Invoke(this, e);
        protected virtual void OnLeaveTranslationUnit(LeaveTranslationUnitEventArgs e) => this.TranslationUnitLeaveEvent?.Invoke(this, e);
    }
}
