using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariablesAndExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}

namespace fabulous
{
    // code in fabulous namespace
    using super;
    class ReferToNameSpace
    {
        int a = super.smashing.GreatName.great;

    }
}

namespace super
{
    namespace smashing
    {
        // great name defined
        class GreatName
        {
            public static int great;
        }
    }
}

