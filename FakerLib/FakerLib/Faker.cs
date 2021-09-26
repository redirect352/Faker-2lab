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
        Dictionary<Type, IGenerator> Generators = new Dictionary<Type, IGenerator>();
        Stack<Type> nestedTypes = new Stack<Type>();


        public Faker()
        {
            Type mainInterface = typeof(IGenerator);
            try
            {

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type[] types = assembly.GetTypes();
                
                int k = 0;
                for (int i = 0; i < types.Length; i++)
                {

                    if (mainInterface.IsAssignableFrom(types[i]) && mainInterface != types[i])
                    {

                        IGenerator generator = (IGenerator)Activator.CreateInstance(types[i]);
                        Generators.Add(generator.ResultType,generator);
                        k++;
                    }
                }
            }
            catch
            {
                throw new Exception("Cannot create Faker");
            }

        }

        public T Create<T>(TextBox text)
        {
            Type type = typeof(T);
            nestedTypes.Push(type);

            T tmp = (T)CheckGenerators(type);
            if (tmp !=null)
            {
                nestedTypes.Pop();
                return tmp;
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
                    text.Text = text.Text + "\r\n";
                    
                    result = (T)constructor.Invoke(values.ToArray());
                }

            }
            catch (Exception ex)
            {
                nestedTypes.Pop();
                throw ex;
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
              value = this.GetType().GetMethod("Create").MakeGenericMethod(valueType).Invoke(this, new object[] { null});
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

    public class MyClass
    {

        public MyClass() { }
        public MyClass(int k, double par2, string ferr2)
        {
            field1 = k;
            field23 = par2;
            field3 = ferr2;
        }

        public int field1 = 0;
        public double field23 = 0;
        public object field3 ;
        public MyClass2 Class2;
        public MyClass classMy;

        public int Property1 { get; set; }
        public object PropertyObject { get; set; }
        private object PrivateProperty { get; set; }

    }

    public class MyClass2
    {
        public int count;
        public string str;
        public MyClass classMy { get; set; }

    }

}
