using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FakerLib.Generator;
namespace FakerLib
{
    public class FakerConfig
    {

        private List<Type> mainTypes = new List<Type>();
        private List<Type> fieldTypes = new List<Type>();
        private List<ICustomGenerator> Generators = new List<ICustomGenerator>();

        private List<object> funcs = new List<object>();


        public void Add<MainType,fieldType,GeneratorType>(Expression<Func<MainType, fieldType>> expression ) 
            where GeneratorType : ICustomGenerator
        {

            
            mainTypes.Add(typeof(MainType));
            fieldTypes.Add(typeof(fieldType));

            GeneratorType generator =  (GeneratorType)Activator.CreateInstance(typeof(GeneratorType));
            Generators.Add(generator);
            funcs.Add(expression.Compile());
            

        }

        public object CheckConfig<f,r>(object defValue,object testedObj)
        {
            Type main = typeof(int);
            Type mainType = testedObj.GetType();
            Type fieldType = defValue.GetType();
            for (int i =0; i < mainTypes.Count;i++)
            {
                if (mainTypes[i] == mainType && fieldType == fieldTypes[i])
                {
                    
                    object val = (funcs[i] as Func<f,r>)((f)testedObj);
                    if (defValue == val)
                    {
                        return Generators[i].Generate();

                    }

                }


            }

            return null;
        }


    }




}
