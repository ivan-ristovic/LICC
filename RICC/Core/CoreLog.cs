using System;
using System.Collections.Generic;
using System.Text;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core
{
    internal static class CoreLog
    {
        public static void DeclarationMissing(DeclarationSpecifiersNode specs, DeclaratorNode decl)
            => Log.Warning("Missing declaration for {Specs} {Identifier}, declared at line {Line}", specs, decl.Identifier, specs.Line);
        
        public static void DeclarationSpecifiersMismatch(DeclaratorNode decl, DeclarationSpecifiersNode expected, DeclarationSpecifiersNode actual)
        {
            Log.Warning("Declaration specifier mismatch for {Identifier}, declared at line {Line}: expected {ExpectedSpecs}, got {ActualSpecs}", 
                        decl.Identifier, expected.Line, expected, actual);
        }

        public static void ExtraDeclarationFound(DeclarationSpecifiersNode specs, DeclaratorNode decl)
            => Log.Warning("Extra declaration found: {Specs} {Identifier}, declared at line {Line}", specs, decl.Identifier, specs.Line);

        public static void DeclaratorMismatch(DeclaratorNode expected, DeclaratorNode actual)
        {
            Log.Warning("Declarator mismatch for {Identifier}, declared at line {Line}: expected {ExpectedDecl}, got {ActualDecl}", 
                        expected.Identifier, expected.Line, expected, actual);
        }

        public static void VariableInitializerMismatch(string var, int line, object? expected, object? actual)
        {
            Log.Error("Variable initializer mismatch for {Identifier}, declared at line {Line}: expected {ExpectedValue}, got {ActualValue}", 
                      var, line, expected, actual);
        }
    }
}
