using System;
using System.Collections.Generic;
using System.Text;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core
{
    internal static class CoreLog
    {
        public static void DeclarationMissing(string identifier, DeclarationSpecifiersNode expectedSpecs)
            => Log.Warning("Missing declaration for {Specs} {Symbol}, declared at line {Line}", expectedSpecs, identifier, expectedSpecs.Line);
        
        public static void DeclarationSpecifiersMismatch(string identifier, DeclarationSpecifiersNode expected, DeclarationSpecifiersNode actual)
        {
            Log.Warning("Declaration specifiers for {Symbol}, declared at line {Line}, are not matched: " +
                        "expected {ExpectedSpecs}, got {ActualSpecs}", identifier, expected.Line, expected, actual);
        }

        public static void ExtraDeclarationFound(string identifier, DeclarationSpecifiersNode specs)
            => Log.Warning("Extra declaration found: {Specs} {Symbol}, declared at line {Line}", specs, identifier, specs.Line);
    }
}
