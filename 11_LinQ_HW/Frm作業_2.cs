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
    public partial class Frm作業_2 : Form
    {
        public Frm作業_2()
        {
            InitializeComponent();
            productTableAdapter1.Fill(advWDataSet1.Product);
            productPhotoTableAdapter1.Fill(advWDataSet1.ProductPhoto);
            productProductPhotoTableAdapter1.Fill(advWDataSet1.ProductProductPhoto);
            YearsComboBoxItems();
            MaxDate_MinDate();
            comboBox2.Text = "--請選擇季數--";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = advWDataSet1.Product.Select(n => n);
           
            dataGridView1.DataSource = q.ToList();
        }
        void YearsComboBoxItems()
        {
            var q = advWDataSet1.Product.Select(n => n.SellStartDate.Year);
            comboBox3.Text = "--請選擇年份--";
            foreach (int i  in q.ToList().Distinct())
            {
                comboBox3.Items.Add(i);
            }        
        }
        void  MaxDate_MinDate()
        {
            var q = advWDataSet1.Product.OrderBy(n => n.SellStartDate).Select(n => n.SellStartDate);
            foreach(DateTime i in q.Take(1).ToList())
            {
                dateTimePicker1.MinDate = i;
                dateTimePicker2.MinDate = i;
            }
            foreach(DateTime i in q.Skip(q.ToList().Count - 1))
            {
                dateTimePicker1.MaxDate = i;
                dateTimePicker2.MaxDate = i;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BetweenDateProduct(dateTimePicker1.Value, dateTimePicker2.Value);
        }
        void BetweenDateProduct(DateTime d1 , DateTime d2)
        {
            if (d1 >= d2)
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate >= d2 && n.SellStartDate <= d1).Select(n => new { n.SellStartDate, n.ProductID });
                dataGridView1.DataSource = q.ToList();
            }
            else 
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate >= d1 && n.SellStartDate <= d2).
                    Select(n => new { n.SellStartDate, n.ProductID });
                dataGridView1.DataSource = q.ToList();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                YearProduct(comboBox3.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("請選擇年份");

            }
        }
        void SeasonProducts(string y,string s)
        {
            if (s =="第一季")
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate.Year.ToString() == y && n.SellStartDate.Month >= 1 && n.SellStartDate.Month <= 3).
                    Select(n => new { n.SellStartDate, n.ProductID, });
                dataGridView1.DataSource = q.ToList();
            }
          else if (s == "第二季")
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate.Year.ToString() == y && n.SellStartDate.Month >= 3 && n.SellStartDate.Month <= 6).
                    Select(n => new { n.SellStartDate, n.ProductID, });
                dataGridView1.DataSource = q.ToList();
            }
           else if (s == "第三季")
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate.Year.ToString() == y && n.SellStartDate.Month >= 7 && n.SellStartDate.Month <= 9).
                    Select(n => new { n.SellStartDate, n.ProductID, });
                dataGridView1.DataSource = q.ToList();
            }
           else  
            {
                var q = advWDataSet1.Product.Where(n => n.SellStartDate.Year.ToString() == y && n.SellStartDate.Month >= 10 && n.SellStartDate.Month <= 12).
                    Select(n => new { n.SellStartDate, n.ProductID, });
                dataGridView1.DataSource = q.ToList();
            }
        }


        void YearProduct(string s)
        {
            var q = advWDataSet1.Product.Where(n => n.SellStartDate.Year.ToString() == s).Select(n => new { n.SellStartDate, n.ProductID });
            dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                SeasonProducts(comboBox3.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("請選擇年份及季數!!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells["ProductID"].ToString();
             var q = advWDataSet1.ProductProductPhoto.Where(n => n.ProductID.ToString() == id).Select(n => new { n.ProductPhotoID });
         int a;
            foreach(var i in q.ToList())
            {
                a = i.ProductPhotoID;
            }
            //var q1 = advWDataSet1.ProductPhoto.Where(n => n.ProductPhotoID.ToString() == a).Select(n => new { n.LargePhoto });
            //byte[] bytes;
            //foreach (var p in q1.ToList())
            //{
            //    bytes=(byte[]) p
            //}
         
        }
    }
}
