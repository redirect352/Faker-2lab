﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FakerLib;
using FakerLib.Generator;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Faker faker = new Faker();
            List<MyClass> f = faker.Create<List<MyClass>>();


            int j = 0;
            foreach (MyClass i in f)
            {
                textBox1.Text = textBox1.Text + "ind: "+j.ToString() + " value:" +i.ToString()+"\r\n";
                j++;
            }




            //faker.Create<MyClass>(textBox1);


            //Type t =  typeof(List<int>), t2 = typeof(List<Double>);
            //textBox1.Text = t.Name;
            //t.GetGenericTypeDefinition();
            //if (t.GetGenericTypeDefinition() == typeof(List<>)) {
            //    textBox1.Text = "Yes";
            //}

            //ListGenerator listGenerator = new ListGenerator();

            ////List<int> f = (List<int>)listGenerator.Generate(typeof(int),null);

            //textBox1.Text = "wfeewf";

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
