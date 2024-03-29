﻿/******************************************************************************
 * Copyright (c) 2019 Jensen Miller
 * 
 * License: The GNU License
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warrenty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>
 ******************************************************************************/
using System;
using System.IO;




namespace CsCppTranslator
{
    class Program
    {
        /// <summary>
        /// The compiler switches.
        /// </summary>
        static CompilerFlags compilerFlags;

        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine(Properties.Resources.CL_WELCOME);
            Console.WriteLine(Properties.Resources.CL_COPYRIGHT);

            string projectSourceDirectory;
            if (args.Length > 0)
            {
                projectSourceDirectory = args[0];
            }
            else
            {
                projectSourceDirectory = Directory.GetCurrentDirectory();
            }

            compilerFlags = CLParser.Parse(args, ref projectSourceDirectory);
            
            Compiler.Compile(projectSourceDirectory, compilerFlags);
        }
    }
}
