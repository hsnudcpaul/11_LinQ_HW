using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            var q = Enumerable.Range(1, 300);          
              foreach(int i in q.ToArray())
            {
                string s=AddNode(i);
                if (treeView1.Nodes[s] == null)
                {
                    TreeNode node = null;
                  node=  treeView1.Nodes.Add(s,s);
                  // node.Name = s;
                        node.Nodes.Add(i.ToString());
                }
                else
                {
                    treeView1.Nodes[s].Nodes.Add(i.ToString());
                }
            }
            //  for(int i=0;i<treeView1.Nodes.Count;i++)
            //{
            //    treeView1.Nodes[i].Text = $"{treeView1.Nodes[i].Text} ({treeView1.Nodes[i].Nodes.Count})";
            //}
        }

         string AddNode(int i)
        {
            if (i <= 100) return "Small";
            else if (i <= 200) return "Medium";
            else return "Large";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            var q = from f in files
                    orderby f.Length descending
                    group f by FileSize(f.Length) into g
                    select new { g.Key, FileCount = g.Count(),MyGroup=g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach(var n in q)
            {
                string s1 = $"{n.Key} ({n.FileCount})";
                TreeNode node = treeView1.Nodes.Add(n.Key,s1);
                    foreach(var item in n.MyGroup)
                {
                    string s = $"{item}(Size: {item.Length})";
                    node.Nodes.Add(item.ToString(),s);
                }
            }
                    
        }

        private string FileSize(long length)
        {
            if (length < 2000) return "Small_File";
            else if (length < 10000) return "Medium_File";
            else return "Large_File";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();
            var q = from f in files
                    orderby f.CreationTime descending
                    group f by f.CreationTime.Year into g
                    select new { g.Key, FileCount = g.Count(), MyGroup = g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var n in q)
            {
                string s1 = $"{n.Key} ({n.FileCount})";
                TreeNode node = treeView1.Nodes.Add(n.Key.ToString(), s1);
                foreach (var item in n.MyGroup)
                {
                    string s = $"{item}(CreationTime: {item.CreationTime})";
                    node.Nodes.Add(item.ToString(), s);
                }
            }
        }
    }
}
