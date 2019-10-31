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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CsCppTranslator
{    
    internal class CompilerFlags
    {
        private bool generateOutput;
        private bool verbose;
        private bool arduinoSketch;
        public bool GenerateOutput {
            get { return generateOutput; }
            set { generateOutput = value; }
        }

        public bool Verbose
        {
            get { return verbose; }
            set { verbose = value; }
        }

        public bool ArduinoSketch
        {
            get { return arduinoSketch; }
            set { arduinoSketch = value; }
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
            if (flags.GenerateOutput)
            {
                StringBuilder sourceCode = CPPCodeGenerator.GenerateCode(rootNode);
                if (flags.ArduinoSketch)
                {
                    System.IO.Directory.CreateDirectory(projectDir + "\\arduino\\program");
                    AddSketch(sourceCode, "BlinkSample", "Program", "Main");
                    System.IO.File.WriteAllText(projectDir + "\\arduino\\program\\program.ino", sourceCode.ToString());
                }
                else
                {
                    System.IO.Directory.CreateDirectory(projectDir + "\\src");
                    AddMainEntry(sourceCode, "BlinkSample", "Program", "Main");
                    System.IO.File.WriteAllText(projectDir + "\\src\\program.cpp", sourceCode.ToString());
                }                
            }
            else
            {
                rootNode.SerializeTo(System.IO.File.OpenWrite(projectDir + "\\out\\out.cu"));            
            }                                                
        }


        private static void AddMainEntry(StringBuilder sb, string namespaceScope, string className, string entryFnName)
        {
            sb.AppendLine("\r\n\r\n");
            sb.AppendFormat("int main()\r\n{{\r\n\t{0}::{1}::{2}();\r\n}}",
                namespaceScope,
                className,
                entryFnName
            );
        }

        private static void AddSketch(StringBuilder sb, string namespaceScope, string className, string entryFnName)
        {            
            sb.AppendLine("\r\n\r\n\r\n\r\n");
            sb.AppendFormat("void setup() {{\r\n\t{0}::{1}::{2}();\r\n}}\r\n",
                namespaceScope,
                className,
                entryFnName
            );
            sb.Append("\r\nvoid loop() {\r\n}");
        }
        

        private static CSharpSyntaxTree Parse(string text)
        {
            return (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(text);
        }


        private static string ReadSourceFile(string filePath) => System.IO.File.ReadAllText(filePath);
    }
}
