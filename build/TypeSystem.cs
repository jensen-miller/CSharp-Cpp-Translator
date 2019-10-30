using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsCppTranslator
{
    class TypeSystem
    {
        static private List<string> primitiveTypes = new List<string>
        {
            "int",
            "bool",
            "float",
            "double",
            "char"
        };
        public bool IsPrimitiveType(TypeSyntax typeSyntax, CPPCodeGenerator generator)
        {
            string type_name = typeSyntax.Accept(generator).ToString();
            return primitiveTypes.Contains(type_name);            
        }
    }
}
