using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Configuration;
using System.IO;


using Microsoft.Glee.Drawing;
using DFSSort;
using GraphDFSBFS;
using BFSSort;
using BooleanSet;
using FileInput;
using Semester;

using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {

        //Variable Global
        public System.Windows.Forms.Form form = new System.Windows.Forms.Form();
        public Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
        public Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");
        public DirectedEdgeMatrix resultGraph;
        public Dictionary<int, string> pemetaanIndex;
        public List<Edge> selfEdge;
        public List<int> solution;
        public int waktuDelay = 100;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Ini adalah tomboll trigger untuk memanggil animasi dan algoritma dfs
            //Inisiasi
            button2.Enabled = true;
            button3.Enabled = true;
            DFS_Sort algoritmaDFS = new DFS_Sort(resultGraph.getSize());
            //Mulai melakukan dfs
            int time = 1;
            for (int i = 0; i < resultGraph.getSize(); i++)
            {
                if (!algoritmaDFS.isVerticeVisited(i))
                {
                    algoritmaDFS.Search(resultGraph, i, ref time);
                }
            }
            algoritmaDFS.makeSolution();

            //Melakukan animasi dfs dengan memanfaatkan track record
            for (int i=0;i<algoritmaDFS.getOrderCount();i++)
            {
                bool isEnd = algoritmaDFS.isVerticeOrderEnd(i);
                int currentNode = algoritmaDFS.getVerticeOrder(i);
                Node currentNodeObject = graph.FindNode(pemetaanIndex[currentNode]);
                if (!isEnd)
                {
                    //Update time stamp
                    selfEdge[currentNode].Attr.Label = (i+1).ToString()+ "/";
                    currentNodeObject.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Yellow;
                }
                else
                {
                    selfEdge[currentNode].Attr.Label = selfEdge[currentNode].Attr.Label + (i + 1).ToString();
                }
                if (i!=0)
                {
                    int previousNode = algoritmaDFS.getVerticeOrder(i-1);
                    Node previousNodeObject = graph.FindNode(pemetaanIndex[previousNode]);
                  
                    previousNodeObject.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Red;
                }
                //Melakukan update gambar
                viewer.Graph = graph;
                viewer.Update();
                System.Threading.Thread.Sleep(waktuDelay);
            }
            //Menyimpan solusi
            solution = algoritmaDFS.getAllSolution();
            

            //form.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Melakukan pembangkitan gambar untuk hasil solusi sequence dari topological sort
            //Menggunakan library graphviz
            //Gambar akan ditampilkan pada picturebox1
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            //Deklarasi wrapper
            var wrapper = new GraphGeneration(getStartProcessQuery,
                                              getProcessStartInfoQuery,
                                              registerLayoutPluginCommand);

            // Membuat query ke graphviz
            string request = "digraph G{{rank = same;";

            for (int i = 0; i < resultGraph.getSize(); i++)
            {
                request = request + pemetaanIndex[solution[i]] + ";" ;
            }

            for (int i=0;i<resultGraph.getSize();i++)
            {
                for (int j = 0; j < resultGraph.getSize(); j++)
                {
                    if (resultGraph.isAdjacent(i,j))
                    {
                        request = request + pemetaanIndex[i] + "->" + pemetaanIndex[j] + ";";
                    }
                }
            }
            request = request+ "}}";

            // Membangkitkan gambar lau disimpan dalam byte
            byte[] output = wrapper.GenerateGraph(request, Enums.GraphReturnType.Jpg);

            //Drawing ke object Image
            Image gambar  = System.Drawing.Image.FromStream(new MemoryStream(output));
            //Tampilkan gambar;
            pictureBox1.Image = gambar;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //Menampilkan ke richtextbox hasil matakuliah yang diambil tiap semester

            Pengambilan_Matkul NgambilMatkul = new Pengambilan_Matkul(solution,resultGraph);
            richTextBox1.Text = "";
            for (int i = 0; i < NgambilMatkul.getSemesterCount(); i++)
            {
                string temp = "";
                temp = "Semester " + (i+1).ToString() + " : ";
                for (int j = 0;j < NgambilMatkul.getMatkulCount(i);j++)
                {
                    if(j != 0)
                    {
                        temp = temp + ", ";
                    }
                    temp = temp + pemetaanIndex[NgambilMatkul.getSemester(i)[j]];
                }
                richTextBox1.Text = richTextBox1.Text + temp + "\r\n"; 
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Onload event form one
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            form.SuspendLayout();

            viewer.Dock = System.Windows.Forms.DockStyle.Fill;

            form.Controls.Add(viewer);

            form.ResumeLayout();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Menampilkan hasil pembacaan file dengan visualisasi DAG
            button1.Enabled = true;
            button4.Enabled = true;

            for (int i = 0; i < resultGraph.getSize(); i++)
            {
                for (int j = 0; j < resultGraph.getSize(); j++)
                {
                    if (resultGraph.isAdjacent(i, j))
                    {
                        string from, to;
                        pemetaanIndex.TryGetValue(i, out from);
                        pemetaanIndex.TryGetValue(j, out to);
                        graph.AddEdge(from, to);
                    }
                }
            }

            //Drawing
            selfEdge = new List<Edge>();
            for (int i=0;i<resultGraph.getSize();i++)
            {
                selfEdge.Add(graph.AddEdge(pemetaanIndex[i], pemetaanIndex[i]));
                selfEdge[i].Attr.Color = Microsoft.Glee.Drawing.Color.White;
                selfEdge[i].Attr.Label = "";
            }

            //Set graph
            viewer.Graph = graph;
            form.SuspendLayout();

            viewer.Dock = System.Windows.Forms.DockStyle.Fill;

            form.Controls.Add(viewer);

            form.ResumeLayout();
            form.MdiParent = this;
            //Show the form
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Ini adalah tombol trigger untuk memanggil algoritma dan animasi dari algoritma bfs
            //Inisiasi
            button2.Enabled = true;
            button3.Enabled = true;
            BFS_Sort algoritmaBFS = new BFS_Sort(resultGraph.getSize());
            algoritmaBFS.Search(resultGraph);
            


            //Animasi
            for (int i = 0; i < resultGraph.getSize(); i++) 
            {
                //Cari node yang ingin diganti warnanya
                Node currentNode = graph.FindNode(pemetaanIndex[algoritmaBFS.getSolution(i)]);
                currentNode.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Yellow;

                //Ganti warna edge yang artinya mengurangi edge
                foreach (Edge edgenow in graph.Edges)
                {
                    if (edgenow.Source == pemetaanIndex[algoritmaBFS.getSolution(i)] && edgenow.Source!= edgenow.Target)
                    {
                        //Ganti warna kuning
                        edgenow.Attr.Color = Microsoft.Glee.Drawing.Color.Yellow;
                    }
                }

                //Update status indegree
                for (int j=0;j < resultGraph.getSize();j++)
                {
                    selfEdge[j].Attr.Label = algoritmaBFS.getIndegreeOfStepandVertice(i, j).ToString();
                }
                viewer.Graph = graph;
                viewer.Update();
                System.Threading.Thread.Sleep(waktuDelay);
            }
            //Masukkan solusi algoritmanya
            solution = algoritmaBFS.getAllSolution();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void showControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Menampilkan semua control
            button1.Show();
            button2.Show();
            button3.Show();
            button4.Show();
            button5.Show();
            button6.Show();
            richTextBox1.Show();
            pictureBox1.Show();
            textBox1.Show();
        }

        private void hideControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //menyembunyikan control
            pictureBox1.Hide();
            button1.Hide();
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
            button6.Hide();
            richTextBox1.Hide();
            textBox1.Hide();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //MEMBACA FILE dari openfiledialog
            DialogResult hasil = openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;

            if (!File.Exists(textBox1.Text))
            {
                //Jika file tidak ditemukan
                MessageBox.Show("File not found");
            }
            else
            {
                //file ditemukan
                button6.Enabled = true;
                richTextBox1.Text = "";
                ReadFromFile inputanFile = new ReadFromFile();

                //Proses load output file ke rich textbox
                inputanFile.read(textBox1.Text, out resultGraph, out pemetaanIndex);
                foreach (KeyValuePair<int, string> entry in pemetaanIndex)
                {
                    richTextBox1.Text = richTextBox1.Text + entry.Key.ToString() + " = " + entry.Value + "\r\n";
                }

                for (int i = 0; i < resultGraph.getSize(); i++)
                {
                    string temp = "";
                    for (int j = 0; j < resultGraph.getSize(); j++)
                    {
                        temp = temp + (resultGraph.isAdjacent(i, j) ? 1.ToString() : 0.ToString()) + " ";
                    }
                    temp = temp + "\r\n";
                    richTextBox1.Text = richTextBox1.Text + temp;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Keluar daru aplikasi
            Application.Exit();
        }
    }
}
