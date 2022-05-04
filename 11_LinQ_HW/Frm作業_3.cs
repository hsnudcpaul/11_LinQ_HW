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
using _11_LinQ_HW;
using LinqLabs;
namespace MyHomeWork
{
    public partial class Frm作業_3 : Form
    {
        public Frm作業_3()
        {
            InitializeComponent();
        }
        NorthwindEntities dbcontext = new NorthwindEntities();
        private void button4_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            var q = Enumerable.Range(1, 300);
            foreach (int i in q.ToArray())
            {
                string s = AddNode(i);
                if (treeView1.Nodes[s] == null)
                {
                    TreeNode node = null;
                    node = treeView1.Nodes.Add(s, s);
                    // node.Name = s;
                    node.Nodes.Add(i.ToString());
                }
                else
                {
                    treeView1.Nodes[s].Nodes.Add(i.ToString());
                }
                dataGridView2.DataSource = null;
                dataGridView1.DataSource = null;
            }
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Text = $"{treeView1.Nodes[i].Text} ({treeView1.Nodes[i].Nodes.Count})";
            }
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
            dataGridView2.DataSource = files;
            var q = from f in files
                    orderby f.Length descending
                    group f by FileSize(f.Length) into g
                    select new { g.Key, FileCount = g.Count(), MyGroup = g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var n in q)
            {
                string s1 = $"{n.Key} ({n.FileCount})";
                TreeNode node = treeView1.Nodes.Add(n.Key, s1);
                foreach (var item in n.MyGroup)
                {
                    string s = $"{item,-25}({"Size: ",-6} {item.Length,-8})";
                    node.Nodes.Add(item.ToString(), s);
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
            dataGridView2.DataSource = files;
            var q = from f in files
                    orderby f.CreationTime
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
                    string s = $"{item,-25}({"CreationTime: ",-15} {item.CreationTime,-20})";
                    node.Nodes.Add(item.ToString(), s);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = dbcontext.Products.ToList();
            var q = from p in dbcontext.Products.AsEnumerable()
                    where p.UnitPrice != null
                    orderby p.UnitPrice
                    group p by MyPrice(p.UnitPrice) into g
                    select new { MyPrice = g.Key, Count = g.Count(), Group = g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var n in q)
            {
                string s = $"{n.MyPrice}({n.Count})";
                TreeNode node = treeView1.Nodes.Add(n.MyPrice, s);
                foreach (var item in n.Group)
                {
                    node.Nodes.Add($"{item.ProductName,-35}({"UnitPrice:",-10} {item.UnitPrice,-10:c2})");
                }
            }
        }
        private string MyPrice(decimal? unitPrice)
        {
            if (unitPrice <= 20) return "低價位產品";
            else if (unitPrice <= 50) return "中價位產品";
            else return "高價位產品";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = dbcontext.Orders.ToList();
            var q = from o in dbcontext.Orders
                    orderby o.OrderDate.Value.Year
                    group o by o.OrderDate.Value.Year into g
                    select new { Year = g.Key, Count = g.Count(), Group = g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var n in q)
            {
                string s = $"{n.Year}({n.Count})";
                TreeNode node = treeView1.Nodes.Add(n.Year.ToString(), s);
                foreach (var item in n.Group)
                {
                    node.Nodes.Add($"{"OrderID: ",-8}{item.OrderID,-7}({"OrderDate: ",-10}{item.OrderDate.Value.ToShortDateString(),-10})");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = dbcontext.Orders.ToList();
            var q = from o in dbcontext.Orders.AsEnumerable()
                    orderby o.OrderDate.Value.Year
                    group o by new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month } into g
                    select new { Year_Month = $"{ g.Key.Year,-4}年{g.Key.Month,-2}月", Count = g.Count(), Group = g };
            dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var n in q)
            {
                string s = $"{n.Year_Month} ({n.Count})";
                TreeNode node = treeView1.Nodes.Add(n.Year_Month.ToString(), s);
                foreach (var item in n.Group)
                {
                    node.Nodes.Add($"{"OrderID: ",-8}{item.OrderID,-7}({"OrderDate: ",-10}{item.OrderDate.Value.ToShortDateString(),-10})");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = from od in dbcontext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    select new { Year = g.Key, TotalPrice = $"{g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)):c2}" };
            dataGridView1.DataSource = q.ToList();
            var q1 = from o in dbcontext.Order_Details.AsEnumerable()
                     group o by true into g
                     select new { 總金額 = $"{g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)):c2}" };
            dataGridView2.DataSource = q1.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var q =( from od in dbcontext.Order_Details.AsEnumerable()
                    group od by new {od.Order.Employee.EmployeeID, od.Order.Employee.FirstName, od.Order.Employee.LastName } into g
                    //orderby   g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)) 
                     select new { EmployeeId=g.Key.EmployeeID, Name = $"{g.Key.FirstName}.{g.Key.LastName}", TotalPrice = $"{g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)):c2}" }).
                     OrderByDescending(p=>decimal.Parse(p.TotalPrice,System.Globalization.NumberStyles.Currency)).Take(5);

            dataGridView2.DataSource = q.ToList();
            dataGridView1.DataSource = null;
            treeView1.Nodes.Clear();
                    
            }

        private void button9_Click(object sender, EventArgs e)
        {
            var q = from p in dbcontext.Products
                    orderby p.UnitPrice descending
                    select new { p.ProductID, p.ProductName, p.UnitPrice, p.Category.CategoryName };
            dataGridView2.DataSource = q.Take(5).ToList();          
            dataGridView1.DataSource = null;
            treeView1.Nodes.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            HasPrice(300);
            //bool result = dbcontext.Products.Where(p => p.UnitPrice > 300).Any();
            //if (result) MessageBox.Show("NW產品裡有產品單價大於300");
            //else MessageBox.Show("NW產品裡沒有產品單價大於300");
        }
        void HasPrice(int i)
        {
            bool result = dbcontext.Products.Where(p => p.UnitPrice > i).Any();
            if (result) MessageBox.Show($"NW產品裡有產品單價大於{i}");
            else MessageBox.Show($"NW產品裡沒有產品單價大於{i}");
        }
    }
    }


