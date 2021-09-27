using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    class IListGenerator 
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private Type collectionType = typeof(IList<>);

        public Type CollectionType
        {
            get { return collectionType; }
        }


        public object Generate<T>(Faker faker)
        {
            ListGenerator listGenerator = new ListGenerator();
            object resList =  listGenerator.Generate<T>(faker);
            return resList;
        }

    }
}
