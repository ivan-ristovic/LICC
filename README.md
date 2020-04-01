# LICC - Language Invariant Code Comparer
![Build](https://github.com/ivan-ristovic/MSc/workflows/.NET%20Core/badge.svg)

## Motivation
LICC was made as a proof of concept for my MSc thesis but has grown with aim to become fully operational and manageable long-term. LICC can work with any grammar and can be used as a tool to generate ASTs for given grammar or compare source codes. Since LICC does not rely on any language-specific AST implementation, there is no constraint that both source codes must be written in the same programming language. LICC provides API for working with generated ASTs and easily creating visitors which will traverse the AST. AST comparer is also provided via intuitive API but the comparer module can also be extended and upgraded.

The motivation for LICC came from the inability to find a shared API for every programming language that is a part of a procedural paradigm. Although all programming languages are different, they share a lot of concepts and hence it could be possible to view them as equal on some level of abstraction. The work I have done goes even further, trying to "equalize" (in this manner) even languages which aren't strictly typed with those that are - hence, apart from C which is procedural paradigm representative, Lua is chosen as a representative for script language paradigm. 

LICC can theoretically work with any programming language as long as the adapter for that language is written. Adapters are used to generate AST from a parse tree and they are different for every programming language due to native difference in parse trees. For now, adapters are written for the following languages:
- C
- Lua
- Pseudocode-like PoC language

Interested readers can read my ![thesis](Thesis/IvanRistovic_MasterRad.pdf).

## Used libraries/tools
- ![ANTLR4](https://www.antlr.org/)
- ![MathNET.Symbolics](https://symbolics.mathdotnet.com/)
