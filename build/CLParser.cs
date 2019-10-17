//  Copyright (c) 2018-2019 Jensen Miller.
//  CsCpp Translator licensed under the GNU GPL-3.0 license.
//  See the LICENSE file in the project root for more information.

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

                    parsedFlags.GenerateOutput = clArgs.Compile;
                }
            );

            return parsedFlags;
        }
    }
}
