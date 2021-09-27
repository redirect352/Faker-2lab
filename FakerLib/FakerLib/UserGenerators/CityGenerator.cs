using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    public class CityGenerator:ICustomGenerator
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private static readonly string[] cityes= new string[] { "Moscow","Minsk", "London", "Berlin","Denver"};

        private Type resultType = typeof(string);
        public Type ResultType
        {
            get
            {
                return resultType;
            }

        }

        public object Generate()
        {
            int ind = rnd.Next(0, cityes.Length-1);
            return cityes[ind];
        }

        public static bool IsGeneratedHere(string s)
        {
            return cityes.Contains(s);
        }

    }
}
