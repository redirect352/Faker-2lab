using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using FakerLib.Generator;

namespace FakerLib
{
    public class Faker
    {

        private Dictionary<Type, IGenerator> Generators = new Dictionary<Type, IGenerator>();
        private Dictionary<Type, ICollectionGenerator> CollectionsGenerators = new Dictionary<Type, ICollectionGenerator>();
        private FakerConfig config = null;


        Stack<Type> nestedTypes = new Stack<Type>();


        public Faker():this(null)
        {
            

        }

        public Faker(FakerConfig config3)
        {
            config = config3;

            Type mainInterface = typeof(IGenerator),
                 collectionsInterface = typeof(ICollectionGenerator);
            try
            {

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] types = assembly.GetTypes();
                
                for (int i = 0; i < types.Length; i++)
                {

                    if (mainInterface.IsAssignableFrom(types[i]) && mainInterface != types[i])
                    {

                        IGenerator generator = (IGenerator)Activator.CreateInstance(types[i]);
                        Generators.Add(generator.ResultType,generator);
                        continue;
                    }
                    if (collectionsInterface.IsAssignableFrom(types[i]) && collectionsInterface != types[i])
                    {

                        ICollectionGenerator collectionGenerator = (ICollectionGenerator)Activator.CreateInstance(types[i]);
                        foreach (Type key  in collectionGenerator.CollectionType)
                        {
                            CollectionsGenerators.Add(key, collectionGenerator);
                        }            
                        continue;
                    }

                }
            }
            catch
            {
                throw new Exception("Cannot create Faker");
            }

        }

        


        public T Create<T>()
        {


            Type type = typeof(T);
            nestedTypes.Push(type);

            object tmp = CheckGenerators(type);
            if (tmp !=null)
            {
                nestedTypes.Pop();
                return (T)tmp;
            }

            T result = default(T);
                 
            try
            {
                result = Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {

                var typeConstructors = type.GetConstructors();
                if (typeConstructors.Count()==0)
                {
                    nestedTypes.Pop();
                    return result;
                }
                foreach (ConstructorInfo constructor in typeConstructors)
                {

                    constructor.GetParameters().Count();
                    List<object> values = new List<object>();
                    
                    foreach (ParameterInfo parameter in constructor.GetParameters())
                    {
                      
                        values.Add(GenerateValue(parameter.ParameterType));
                    }
                    
                    result = (T)constructor.Invoke(values.ToArray());
                }

            }
            catch (Exception ex)
            {
                nestedTypes.Pop();
                throw ex;
            }

            if (type.IsValueType)

            {
                nestedTypes.Pop();
                return result;
            }

            foreach (FieldInfo f in type.GetFields())
            {
                
                object fieldValue = GenerateValue(f.FieldType);    
                f.SetValue(result, fieldValue);
            }

            foreach (PropertyInfo property in type.GetProperties())
            {

                if (property.CanWrite)
                {
                    object propertyValue = GenerateValue(property.PropertyType);
                    property.SetValue(result,propertyValue);
                }
            }

            if (config ==null)
            {
                nestedTypes.Pop();
                return result;
            }

            foreach (FieldInfo f in type.GetFields())
            {
                object defaultValue =  f.GetValue(result);
                if (defaultValue == null)
                    continue;
                object newValue = config.GetType().GetMethod("CheckConfig").MakeGenericMethod(typeof(T),f.FieldType).
                                    Invoke(config,new object[] { defaultValue,result });
                    

                if (newValue != null)
                {
                    f.SetValue(result, newValue);
                }

            }

            foreach (PropertyInfo property in type.GetProperties())
            {

                if (!property.CanWrite)
                {
                    continue;
                }
                object defaultValue2 = property.GetValue(result);
                if (defaultValue2 == null)
                    continue;
                object newValue = config.GetType().GetMethod("CheckConfig").MakeGenericMethod(typeof(T), property.PropertyType).
                                   Invoke(config, new object[] { defaultValue2, result });
                if (newValue != null)
                {
                    property.SetValue(result, newValue);
                }

            }


            nestedTypes.Pop();
            return result;
        }


        private object CheckGenerators( Type destinationType)
        {
            
            foreach (Type t in Generators.Keys)
            {
                if (Generators[t].ResultType == destinationType)
                {
                    return Generators[t].Generate();
                }

            }
            if (!destinationType.IsGenericType)
                return null;
            foreach (Type t in CollectionsGenerators.Keys)
            {     
                if (CollectionsGenerators[t].CollectionType.Contains(destinationType.GetGenericTypeDefinition()))
                {
                    var type = CollectionsGenerators[t].GetType();
                    var Method = type.GetMethod("Generate");
                    var MemberType = destinationType.GenericTypeArguments.First();
                    var res = (object)Method.MakeGenericMethod(MemberType).Invoke(CollectionsGenerators[t], new object[] { this });
                    return res;
                }

            }

            return null;
        }

        private object GenerateValue(Type valueType)
        {
            object value = default(object);
            if (Generators.ContainsKey(valueType))
            {
                value = Generators[valueType].Generate();
            }
            else if(!nestedTypes.Contains(valueType))
            {        
              value = this.GetType().GetMethod("Create").MakeGenericMethod(valueType).Invoke(this, null);
            }

            return value;

        }


        private static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }


    }

    

}
