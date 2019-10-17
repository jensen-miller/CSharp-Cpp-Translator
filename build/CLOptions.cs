//  Copyright (c) 2018-2019 Jensen Miller.
//  CsCpp Translator licensed under the GNU GPL-3.0 license.
//  See the LICENSE file in the project root for more information.


using System;
using CommandLine;


namespace CsCppTranslator
{
    /// <summary>
    /// Command Line Options (CLO).    
    /// </summary>
    /// <remarks>
    /// Set the project site for the command line parser from commandlineparser/commandline:
    ///     https://github.com/commandlineparser/commandline
    /// </remarks>
    class CLOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('c', "compile", Default = true, Required = false, HelpText = "Only compile.")]
        public bool Compile { get; set; }

        [Option('o', "output", Default = "a.out", HelpText ="Name of output file")]
        public string OutputFileName { get; set; }


        [Option(Default = false, HelpText = "Make the output pretty format")]
        public bool Pretty { get; set; }
    }
}
