using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDFSBFS
{
    public class DirectedEdgeMatrix
    //Representasi graf berarah dengan edge matrix
    {
        public bool[,] adjMatrix;
        public int Size;

        //Constructor
        public DirectedEdgeMatrix(int n){
            Size = n;
            adjMatrix = new bool[Size, Size];
            for (int i=0;i<n;i++){
                for (int j=0; j<n;j++) {
                    adjMatrix[i, j] = false;
                }
            }
        }

        //Member Function
        public void addEdge(int from , int to) {
            adjMatrix[from, to] = true;
        }
        public void removeEdge(int from, int to)
        {
            adjMatrix[from, to] = false;
        }

        public bool isAdjacent(int from, int to)
        {
            return adjMatrix[from, to];
        }

        public int getSize()
        {
            return Size;
        }

        public int countIndegree(int i)
        //counts the amount of indegree of vertice i
        {
            int count = 0;
            for(int j = 0; j < Size; j++)
            {
                if (isAdjacent(j, i)){
                    count += 1;
                }
            }
            return count;
        }
    }
}
