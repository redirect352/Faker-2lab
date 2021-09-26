﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib.Generator
{
    public class ListGenerator : ICollectionGenerator
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);
        private Type collectionType = typeof(List<>);

        public Type CollectionType
        {
            get { return collectionType; }
        }


        public object Generate<T>( Faker faker)
        {
            List<T> resultList = new List<T>();
            int count = rnd.Next(0,10);
            for (int i = 0; i< count; i++)
            {
                resultList.Add(faker.Create<T>());

            }
            
            
            return resultList;
        }


    }
}
