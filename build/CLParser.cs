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
using CommandLine;

namespace CsCppTranslator
{
    /// <summary>
    /// Command line parser (CLP).
    /// </summary>
    /// <remarks>
    /// Set the project site for the command line parser from commandlineparser/commandline:
    ///     https://github.com/commandlineparser/commandline
    /// </remarks>
    static class CLParser
    {
        /// <summary>
        /// Parse the command line options/arguments into compiler flags and directives.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CompilerFlags Parse(string[] args, ref string projectPath)
        {
            CompilerFlags parsedFlags = new CompilerFlags();

            Parser.Default.ParseArguments<CLOptions>(args)
                .WithParsed<CLOptions>(clArgs =>
                {
                    Console.WriteLine();

                    if (clArgs.Verbose)
                    {
                        Console.WriteLine("> Verbose messages will be outputed during translation.");
                        parsedFlags.Verbose = clArgs.Verbose;
                    }

                    if (clArgs.FrameworkName != null)
                    {
                        if (clArgs.FrameworkName.Contains("arduino"))
                            parsedFlags.ArduinoSketch = true;
                    }

                    parsedFlags.GenerateOutput = clArgs.Compile;
                }
            );

            return parsedFlags;
        }
    }
}
