/******************************************************************************
 *  Copyright (c) 2019 Jensen Miller
 *
 *  License: The GNU License
 *  
 *  This file is part of CS-CPP-Translator.
 *
 *  CS-CPP-Translator is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  CS-CPP-Translator is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with CS-CPP-Translator.  If not, see <https://www.gnu.org/licenses/>.
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsCppTranslator
{
    class TypeSystem
    {
        private static List<string> builtinTypes = new List<string>
        {
            "bool",
            "char",
            "unsigned char",
            "signed char",
            "int",
            "unsigned int",
            "signed int",
            "short",
            "unsigned short",
            "signed short",
            "float",
            "double",
        };




        /// <summary>
        /// Determines if the provided type is a C++ built-in type.
        /// </summary>
        /// <param name="typeSyntax"></param>
        /// <param name="generator"></param>
        /// <returns></returns>
        public static bool IsBuiltinType(TypeSyntax typeSyntax, CPPCodeGenerator generator)
        {
            string type_name = typeSyntax.Accept(generator).ToString();
            return builtinTypes.Contains(type_name);            
        }
    }
}
