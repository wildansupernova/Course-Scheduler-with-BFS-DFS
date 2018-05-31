using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDFSBFS;
using BooleanSet;

namespace BFSSort
{
    class BFS_Sort
    {
        //Member Variables
        private Boolean_Set VisitedSet;//Set Simpul yang telah ditelusuri
        private int[] indegree;//Array derajat masuk setiap simpul
        private int[,] indegreePerStep;//Derajat masuk setiap simpul setiap langkah
        private List<int> Solution;//Solusi yang dibentuk
        private int Size;
        //Constructor
        public BFS_Sort()
        {
            Size = 10;
            VisitedSet = new Boolean_Set(Size);
            Solution = new List<int>(Size);
            indegree = new int[Size];
            indegreePerStep = new int[Size, Size];
        }
        public BFS_Sort(int size)
        {
            Size = size;
            VisitedSet = new Boolean_Set(Size);
            Solution = new List<int>(Size);
            indegree = new int[Size];
            indegreePerStep = new int[Size, Size];
        }
        //MemberFunction
        public void Search(DirectedEdgeMatrix edge)
        //Search the graph from the vertice with 0 indegree
        {
            //Menghitung derajat masuk
            for (int i = 0; i < Size; i++)
            {
                indegree[i] = edge.countIndegree(i);
            }
            int j;
            for (int i = 0; i < Size; i++)
            {
                j = 0;
                //Mencari simpul yang belum dikunjungi yang memiliki indegree 0
                while (indegree[j] != 0 || VisitedSet.isVisited(j))
                {
                    j++;
                }
                VisitedSet.visit(j);
                Solution.Add(j);
                //Pencatatan indegree setiap langkah
                for (int k = 0; k < Size; k++)
                {
                    indegreePerStep[i, k] = indegree[k];
                }
                //Pengurangan indegree tentangga simpul yang dipilih
                for (int k = 0; k < Size; k++)
                {
                    if (edge.isAdjacent(j, k))
                    {
                        indegree[k] -= 1;
                    }
                }
            }
        }

        public int getSolution(int i)
        {
            return Solution[i];
        }
        public List<int> getAllSolution()
        {
            return Solution;
        }
        public int getIndegreeOfStepandVertice(int Step, int Vertice)
        {
            return indegreePerStep[Step, Vertice];
        }
    }
}