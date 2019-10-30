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

namespace CsCppTranslator
{
    internal class SymbolTable
    {
        /// <summary>
        /// Populated with static classifiers (Static objects, Enum classifiers, etc.)
        /// </summary>
        List<string> StaticObjects = new List<string>();


        public void AddStaticObjectIdentifier(string objectId)
        {
            StaticObjects.Add(objectId);
        }

        public bool ContainsSymbol(string objectId)
        {
            return StaticObjects.Contains(objectId);
        }
    }
}
