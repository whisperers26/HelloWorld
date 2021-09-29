using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Class
{
    public class MyClass
    {
        private string myString;

        public string MyString
        {
            set => myString = value;
        }

        public virtual string GetString()
        {
            return myString;
        }
    }

    public class MyDerivedClass:MyClass
    {
        public override string GetString()
        {
            return (base.GetString() + " (output from the derived class)");
        }
    }
}
