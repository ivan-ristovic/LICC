using RICC.Adapters;
using RICC.Context;

namespace RICC.Core
{
    public interface IParseEventHandler
    {
        void SubscribeToListener(ParserListener listener)
        {
            listener.TranslationUnitEnterEvent += TranslationUnitEnter;
            listener.TranslationUnitLeaveEvent += TranslationUnitLeave;
        }

        void TranslationUnitEnter(object? sender, EnterTranslationUnitEventArgs? e);
        void TranslationUnitLeave(object? sender, LeaveTranslationUnitEventArgs? e);
    }
}
