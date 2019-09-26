/******************************************************************************
 *  Copyright (c) 2019 Jensen Miller
 *
 *  License: The GNU License
 *  
 *  This file is part of IoTDotNet.
 *
 *  IoTDotNet is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IoTDotNet is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IoTDotNet.  If not, see <https://www.gnu.org/licenses/>.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CS_CPP_Translator
{    
    internal class CompilerFlags
    {
        private bool _generateOutput;
        public bool GenerateOutput {
            get { return _generateOutput; }
            set { _generateOutput = value; }
        }
    }


    internal static class Compiler
    {
        /// <summary>
        /// Compile routine. Used to compile the CSharp code
        /// </summary>
        /// <param name="projectDir"></param>
        public static void Compile(string projectDir, CompilerFlags flags)
        {
            //  Obtain the source code from the project Program file.
            string sourceCodeText = ReadSourceFile(projectDir + "\\Program.cs");

            //  Parse the source code into a syntax tree.
            CSharpSyntaxTree syntaxTree = Parse(sourceCodeText);

            //  Get the root node of the syntax tree
            CSharpSyntaxNode rootNode = syntaxTree.GetRoot();

            //  Generate output or serialize compile unit
            //
            System.IO.Directory.CreateDirectory(projectDir + "\\out");
            if (flags.GenerateOutput)
            {
                System.IO.File.WriteAllText(projectDir + "\\out\\program.cpp", CPPCodeGenerator.GenerateCode(rootNode).ToString());
            }
            else
            {
                rootNode.SerializeTo(System.IO.File.OpenWrite(projectDir + "\\out\\out.cu"));            
            }                                                
        }
        

        private static CSharpSyntaxTree Parse(string text)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(text);
        }


        private static string ReadSourceFile(string filePath) => System.IO.File.ReadAllText(filePath);
    }
}
