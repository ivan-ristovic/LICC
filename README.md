# LICC - Language Invariant Code Comparer
![Build](https://github.com/ivan-ristovic/MSc/workflows/.NET%20Core/badge.svg)

LICC is a set of libraries which provide common AST abstraction API. Furthermore, as an example of use, a library for semantic comparison of structurally similar codes has been included in this solution.

## Motivation
There are many programming languages out there and, even though their syntax might be different, they often derive from or use certain universal programming concepts. We also call that a _way of writing code_ or, more commonly, a _programming paradigm_. The motivation for LICC came from the inability to find a shared API for every programming language that is a part of a procedural paradigm. LICC aims to create a common abstraction for imperative programming paradigm so that it is possible to view many different imperative programming languages on the same level of abstraction.

LICC was made as a proof of concept for my MSc thesis but has grown with aim to become fully operational and manageable long-term. LICC can work with any grammar and can be used as a tool to generate ASTs for given grammar or compare abstractions of source codes. Since LICC does not rely on any language-specific AST implementation, there is no constraint that both source codes must be written in the same programming language. LICC provides API for working with generated ASTs and AST visitors. AST comparer is also provided via intuitive API but the comparer module can also be extended and upgraded.

LICC can theoretically work with any programming language as long as the adapter for that language is written. Adapters serve as an intermediary between parse trees and common ASTs. Adapters are used to generate AST from a parse tree and they are different for every programming language due to native difference in parse trees. For now, adapters are written for the following languages:
- C
- Lua
- Pseudocode-like PoC language

Interested readers can read more in my ![thesis](Thesis/IvanRistovic_MasterRad.pdf) (currently only in Serbian).

## Used libraries/tools
- ![ANTLR4](https://www.antlr.org/)
- ![MathNET.Symbolics](https://symbolics.mathdotnet.com/)
