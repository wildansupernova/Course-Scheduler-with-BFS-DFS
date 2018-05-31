using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphDFSBFS;

namespace FileInput
{
    class ReadFromFile
	{
		public ReadFromFile() {
		}
		public void read(string filename, out DirectedEdgeMatrix M, out Dictionary<int, string> MatKul) {
			string[] lines = System.IO.File.ReadAllLines(filename);
            string[] hasilSplit;
            Dictionary<string, int> indexMatkul = new Dictionary<string, int>();
            MatKul = new Dictionary<int, string>();
			int n = 0;
            //Pembentukan ukuran matrix
			foreach (string line in lines)
			{
				n++;
			}
            M = new DirectedEdgeMatrix(n);
            int i = 0;
            int j;
            string matkul;
            //Parsing untuk pembentukan dictionary matkul
			foreach (string line in lines)
			{
                hasilSplit = line.Replace(".", "").Replace(" ", "").Split(',');
                MatKul.Add(i, hasilSplit[0]);
                indexMatkul.Add(hasilSplit[0], i);
                i++;
			}
            i = 0;
            //Parsing dan memasukan data pada file input ke dalam matriks
            foreach (string line in lines)
            {
                j = 0;
                hasilSplit = line.Replace(".", "").Replace(" ", "").Split(',');
                matkul = hasilSplit[0];
                foreach (String prerequisite in hasilSplit)
                {
                    if (!matkul.Equals(prerequisite))
                    {
                        indexMatkul.TryGetValue(prerequisite, out j);
                        M.addEdge(j, i);
                    }
                    j++;
                }
                i++;
            }

		}
		
	}
}
