using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDFSBFS;
using BooleanSet;

namespace DFSSort
{
    class TimeStamp
    //TimeStamp penelusuran
    {
        //Member Variables
        private int start;
        private int end;
        //Contructors
        public TimeStamp()
        {
            start = 0;
            end = 0;
        }
        public TimeStamp(int s)
        {
            start = s;
            end = 0;
        }
        //Member Function
        public void setEnd(int e)
        {
            end = e;
        }
        public int getStart()
        {
            return start;
        }
        public int getEnd()
        {
            return end;
        }
    }
    class DFSCheck
    //Data penelusuran, Mulai/selesai dan simpulnya
    {
        private int Vertice;
        private bool End;
        public DFSCheck()
        {
            Vertice = -1;
            End = false;
        }
        public DFSCheck(int vertice, bool end)
        {
            Vertice = vertice;
            End = end;
        }
        public int getVertice()
        {
            return Vertice;
        }
        public bool isEnd()
        {
            return End;
        }
    }
    class DFS_Sort
    //Topological sort dengan pendekatan DFS
    {
        //Member Variables
        private Boolean_Set VisitedSet;//Set Simpul yang telah ditelusuri
        private TimeStamp[] VerticeTimeStamps;//Timestamp setiap simpul
        private List<DFSCheck> CheckOrder;//Urutan penelusuran
        private List<int> Solution;//Solusi yang dibentuk
        private int Size;
        //Constructor
        public DFS_Sort()
        {
            Size = 10;
            VisitedSet = new Boolean_Set(Size);
            CheckOrder = new List<DFSCheck>(Size*2);
            VerticeTimeStamps = new TimeStamp[Size];
            Solution = new List<int>(Size);
        }
        public DFS_Sort(int size)
        {
            Size = size;
            VisitedSet = new Boolean_Set(Size);
            CheckOrder = new List<DFSCheck>(Size * 2);
            VerticeTimeStamps = new TimeStamp[Size];
            Solution = new List<int>(Size);
        }

        //MemberFunction
        public int getOrderCount()
        {
            return Size * 2;
        }
        public int getSolution(int i)
        {
            return Solution[i];
        }
        public TimeStamp getTimeStamp(int i)
        {
            return VerticeTimeStamps[i];
        }
        public int getVerticeOrder(int i)
        {
            return CheckOrder[i].getVertice();
        }
        public bool isVerticeOrderEnd (int i)
        {
            return CheckOrder[i].isEnd();
        }
        public bool isVerticeVisited(int i)
        {
            return VisitedSet.isVisited(i);
        }
        public List<int> getAllSolution()
        {
            return Solution;
        }
        public void Search(DirectedEdgeMatrix edge, int VerticeCheck, ref int time)
        //Search the vertice Check, then search the next vertices from vertice Check
        {
            VisitedSet.visit(VerticeCheck);
            VerticeTimeStamps[VerticeCheck] = new TimeStamp(time);
            //Penelurusan mulai
            CheckOrder.Add(new DFSCheck(VerticeCheck, false));
            time += 1;
            //Penelusuran anak
            for(int i = 0; i < Size; i++)
            {
                if ((!VisitedSet.isVisited(i)) && (edge.isAdjacent(VerticeCheck, i)))
                {
                    Search(edge, i, ref time);
                }
            }
            //Penelusuran selesai
            VerticeTimeStamps[VerticeCheck].setEnd(time);
            CheckOrder.Add(new DFSCheck(VerticeCheck, true));
        }

        public void makeSolution()
        //Whole graph has been checked
        //Pembentukan solusi berdasarkan timestamp
        {
            int NextVertice = 0;
            int MaxEndTime;
            //Pengurutan berdasarkan timestamp selesai
            for(int i = 0; i < Size; i++)
            {
                MaxEndTime = 0;
                for(int j = 0; j < Size; j++)
                {
                    if((VerticeTimeStamps[j].getEnd() > MaxEndTime)&&(!Solution.Contains(j)))
                    {
                        MaxEndTime = VerticeTimeStamps[j].getEnd();
                        NextVertice = j;
                    }
                }
                Solution.Add(NextVertice);
            }
        }
    }
}
