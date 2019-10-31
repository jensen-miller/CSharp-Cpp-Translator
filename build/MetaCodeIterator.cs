using System;
using System.Collections.Generic;
using System.Text;

namespace CsCppTranslator
{
    class MetaCodeIterator
    {
        string functionNameBeingDefined = null;
        string objectNameBeingDefined = null;
        uint blockIndex = 0;

        public bool InFunctionDefinition
        {
            get { return functionNameBeingDefined != null; }
        }

        public string DefiningFunction
        {
            get { return functionNameBeingDefined; }
            set { functionNameBeingDefined = value; }
        }        


        public bool InClassDefinition
        {
            get { return objectNameBeingDefined != null; }
        }


        public string DefiningClass
        {
            get { return objectNameBeingDefined; }
            set { objectNameBeingDefined = value; }
        }

        public uint IncrementBlockIndex()
        {
            blockIndex++;
            return blockIndex;
        }


        public uint DecrementBlockIndex()
        {
            blockIndex--;
            return blockIndex;
        }
    }
}
