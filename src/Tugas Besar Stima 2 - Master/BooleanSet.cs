using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooleanSet
{
    class Boolean_Set
    {
        //Set to store visited vertices
        //Member Variables
        private bool[] data;
        private int Size;

        //Constructors
        public Boolean_Set()
        {
            Size = 10;
            data = new bool[Size];
            for (int i = 0; i < Size; i++)
            {
                data[i] = false;
            }
        }
        public Boolean_Set(int size)
        {
            Size = size;
            data = new bool[Size];
            for (int i = 0; i < Size; i++)
            {
                data[i] = false;
            }
        }

        //Member Functions
        public bool isVisited(int i)
        //Returns the value of Set[i]
        {
            return data[i];
        }
        public void visit(int i)
        //Visits the vertice at i, setting the value to true
        {
            data[i] = true;
        }
    }
}