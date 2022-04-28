using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        int take = 0;
        int takeold = 0;
        int skip = 0;
        bool flag = true;

        public Frm作業_1()
        {
            InitializeComponent();
            ordersTableAdapter1.Fill(nwDataSet1.Orders);
            order_DetailsTableAdapter1.Fill(nwDataSet1.Order_Details);
            productsTableAdapter1.Fill(nwDataSet1.Products);
            AddComboBoxItems();

            //var q = from p in nwDataSet1.Products
            //        select p;
            //take = int.Parse(textBox1.Text);

            //dataGridView1.DataSource = q.Skip(0).Take(take).ToList();



        }

        void AddComboBoxItems()
        {
            var q = from o in nwDataSet1.Orders
                    select o.OrderDate.Year.ToString();
            List<string> years = new List<string>();
            comboBox1.Text = "請選擇年分";
            foreach (string s in q.Distinct().ToList())
            {
                //if (years.Contains(s))
                //{
                //    continue;
                //}
                //else
                //{
                //    years.Add(s);
                //}
                comboBox1.Items.Add(s);
            }
            //foreach (string year in years)
            //{
            //    comboBox1.Items.Add(year);
            //}
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "訂單";
            if (skip > nwDataSet1.Products.Rows.Count)
            {
                MessageBox.Show("已顯示最後一筆");
            }
                else
            {
                if (int.TryParse(textBox1.Text, out take) && int.Parse(textBox1.Text) >= 0 && int.Parse(textBox1.Text) <= nwDataSet1.Products.Rows.Count)
                {
                    if (skip == 0)
                    {
                        ShowProducts();
                        skip = take;
                        takeold = take;
                    }
                    else
                    {

                        if (flag == false)
                        {
                            flag = true;
                            skip = skip + takeold;
                            ShowProducts();
                            skip = skip + take;
                            takeold = take;
                        }
                        else
                        {
                            ShowProducts();
                            skip = skip + take;
                            takeold = take;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("請輸入正確的筆數");
                }
            }
            dataGridView2.DataSource = null;

            //Distinct()
        }

        private void button14_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from n in files
                    where n.Extension == ".log"
                    select n;

            this.dataGridView1.DataSource = q.ToList();
            reSet();
            dataGridView2.DataSource = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from n in files
                    where n.CreationTime.Year == 2019
                    select n;
            dataGridView2.DataSource = null;
            this.dataGridView1.DataSource = q.ToList();
            reSet();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from n in files
                    where n.Extension == ".log"
                    select n;
            dataGridView2.DataSource = null;
            this.dataGridView1.DataSource = files;
            reSet();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "FileInfo";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            System.IO.FileInfo[] files = dir.GetFiles();
            var q = from n in files
                    where n.Length >= 10000
                    select n;
            dataGridView2.DataSource = null;
            this.dataGridView1.DataSource = q.ToList();
            reSet();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "訂單";
            dataGridView1.DataSource = nwDataSet1.Orders;
            dataGridView2.DataSource = nwDataSet1.Order_Details;
            reSet();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "訂單";
            if (comboBox1.SelectedItem != null)
            {
                string year = comboBox1.SelectedItem.ToString();
                var q = from o in nwDataSet1.Orders
                        where o.OrderDate.Year.ToString() == year
                        select o;
                dataGridView1.DataSource = q.ToList();

                var od = from d in nwDataSet1.Order_Details
                         join o in nwDataSet1.Orders
                         on d.OrderID equals o.OrderID
                         where o.OrderDate.Year.ToString() == year
                         select d;
                dataGridView2.DataSource = od.ToList();


            }
            else
            {
                MessageBox.Show("請選擇正確年份");
            }
            reSet();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            lblMaster.Text = "產品";
          
                    if (int.TryParse(textBox1.Text, out take) && int.Parse(textBox1.Text) >= 0 && int.Parse(textBox1.Text) <= nwDataSet1.Products.Rows.Count)
                    {
                        if (skip == 0)
                        {
                            ShowProducts();
                            skip = take;
                            flag = false;
                            takeold = take;
                        }
                        else
                        {
                            if (flag == true)
                            {
                                flag = false;
                                skip = skip - take - takeold;
                                ShowProducts();
                                takeold = take;

                            }
                            else
                            {
                                skip = skip - take;
                                ShowProducts();
                                takeold = take;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("請輸入正確的筆數");
                    }
            dataGridView2.DataSource = null;
        }
        

        void reSet()
        {
            take = 0;
            skip = 0;
        }

        void ShowProducts()
        {
            var q = from n in nwDataSet1.Products
                    select n;
            dataGridView1.DataSource = q.Skip(skip).Take(take).ToList();
        }

    }
}
