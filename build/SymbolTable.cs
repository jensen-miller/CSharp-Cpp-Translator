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
    internal class SymbolTable
    {
        /// <summary>
        /// Populated with static classifiers (Static objects, Enum classifiers, etc.)
        /// </summary>
        List<string> staticClasses = new List<string>();
        Dictionary<string, Tuple<TypeSyntax, VariableDeclaratorSyntax>> declaredVariableMap = new Dictionary<string, Tuple<TypeSyntax, VariableDeclaratorSyntax>>();
        Dictionary<string, Dictionary<string, uint>> referenceMap = new Dictionary<string, Dictionary<string, uint>>();
        List<string> definedClasses = new List<string>();

        public void AddStaticObjectIdentifier(string objectId)
        {
            staticClasses.Add(objectId);
        }



        /// <summary>
        /// Determine if an identifier resolves as a static class definition.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool IsStaticClass(string objectId)
        {
            return staticClasses.Contains(objectId);
        }



        /// <summary>
        /// Declare a variable.
        /// </summary>
        /// <param name="variableType"></param>
        /// <param name="variableDeclarator"></param>
        /// <param name="generator"></param>
        /// <returns></returns>
        public bool DeclareVariable(TypeSyntax variableType, VariableDeclaratorSyntax variableDeclarator)
        {
            declaredVariableMap.Add(
                variableDeclarator.Identifier.ValueText,
                new Tuple<TypeSyntax, VariableDeclaratorSyntax>(variableType, variableDeclarator)
            );            
            return true;
        }


        public uint ReferenceIdentifier(IdentifierNameSyntax expression, MetaCodeIterator metaCode)
        {  
            if (metaCode.InFunctionDefinition)
            {
                if (referenceMap.ContainsKey(expression.Identifier.ValueText))
                {
                    referenceMap[metaCode.DefiningFunction][expression.Identifier.ValueText] += 1;
                }
                else
                {
                    var new_identifier_ref = new Dictionary<string, uint>();
                    new_identifier_ref.Add(expression.Identifier.ValueText, 1);
                    referenceMap.Add(metaCode.DefiningFunction, new_identifier_ref);
                }
                return referenceMap[metaCode.DefiningFunction][expression.Identifier.ValueText];
            }
            return 0;
        }


        public void CollectGarbage(MetaCodeIterator metaCode, StringBuilder blockBuilder)
        {
            if (metaCode.InFunctionDefinition
                && referenceMap.ContainsKey(metaCode.DefiningFunction))
            {
                foreach (var dynamicallyAllocVar in referenceMap[metaCode.DefiningFunction])
                {

                }
            }
        }
    }
}
