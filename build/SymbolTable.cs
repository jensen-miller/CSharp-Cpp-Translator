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
