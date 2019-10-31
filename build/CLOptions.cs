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

        [Option('z', "framework", Required = false, HelpText = "Chose a framework for the given project.")]
        public string FrameworkName { get; set; }


        [Option(Default = false, HelpText = "Make the output pretty format")]
        public bool Pretty { get; set; }
    }
}
