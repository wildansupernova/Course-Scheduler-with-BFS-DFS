using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFSSort;
using DFSSort;
using GraphDFSBFS;

namespace Semester
{
    class Pengambilan_Matkul
    //Class untuk urutan pengambilan matkul tiap semester
    {
        private List<List<int>> Semester;//array semester dan matkulnya, Semester[i] berisi matkul-matkul yang diambil pada semester i
        private List<int> MatkulPerSemester;//Jumlah matkul yang diambil per semester
        private int JumlahSemester;

        //Constructor
        //Constructor sekaligus membentuk isi Semester;
        public Pengambilan_Matkul(List<int> Solution, DirectedEdgeMatrix M)
        {
            //Pembentukan array berdasarkan ukuran graf M
            Semester = new List<List<int>>(M.getSize());
            MatkulPerSemester = new List<int>(M.getSize());
            for(int i = 0; i < M.getSize(); i++)
            {
                Semester.Add(new List<int>(M.getSize()));
            }

            //Pemilihan Matkul pada tiap semester
            int j = 0;
            int countMatkul = 0;
            for (int i = 0; i < M.getSize(); i++)
            {
                if(Semester[0].Count == 0)
                {
                    Semester[j].Add(Solution[i]);
                    countMatkul += 1;
                }
                else
                {
                    //Jika ada prerequisitenya, diambil pada semester berikutnya
                    bool foundPrerequisite = false;
                    for(int k = 0; k < countMatkul; k++)
                    {
                        if (M.isAdjacent(Semester[j][k], Solution[i]))
                        {
                            foundPrerequisite = true;
                        }
                    }
                    if (foundPrerequisite)
                    {
                        MatkulPerSemester.Add(countMatkul);
                        j++;
                        countMatkul = 0;
                    }
                    countMatkul += 1;
                    Semester[j].Add(Solution[i]);
                }
            }
            JumlahSemester = j+1; MatkulPerSemester.Add(countMatkul);
            MatkulPerSemester.Add(countMatkul);
        }
        //MemberFunction
        public List<int> getSemester(int i)
        {
            return Semester[i];
        }
        public int getMatkulCount(int i)
        {
            return MatkulPerSemester[i];
        }
        public int getSemesterCount()
        {
            return JumlahSemester;
        }
    }
}
