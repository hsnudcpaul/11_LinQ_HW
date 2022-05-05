using _11_LinQ_HW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs
{
    public partial class Frm考試 : Form
    {
        public Frm考試()
        {
            InitializeComponent();

            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

                                          };
        }

        List<Student> students_scores;
        NorthwindEntities dbcontext = new NorthwindEntities();
        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public int Chi { get; set; }
            public int Eng { get; internal set; }
            public int Math { get; set; }
            public string Gender { get; set; }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績

            // 
            // 共幾個 學員成績 ?						
            listBox1.Items.Add($"共{students_scores.Count()}個學員成績");
            // 找出 前面三個 的學員所有科目成績					
            // 找出 後面兩個 的學員所有科目成績					

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績						

            // 找出學員 'bbb' 的成績	                          

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |				
            // 數學不及格 ... 是誰 
            #endregion

        }

        private void button37_Click(object sender, EventArgs e)
        {

            //個人 sum, min, max, avg

            //各科 sum, min, max, avg
        }
        List<Student> students_100 = new List<Student>();
        private void button33_Click(object sender, EventArgs e)
        {
            Clear();
            Random random = new Random(DateTime.Now.Millisecond);
            students_100.Clear();
            int m = random.Next(0, 3);
            string ma = "";
            if (m == 1)
            {
                ma = "Male";
            }
            else
            {
                ma = "Female";
            }

            for (int i = 0; i < 100; i++)
            {
                Student s = new Student
                {
                    Name = "Student" + (i + 1),
                    Chi = random.Next(0, 101),
                    Class = "cs_" + random.Next(101, 103),
                    Eng = random.Next(0, 101),
                    Math = random.Next(0, 101),
                    Gender = ma
                };
                students_100.Add(s);
            };
            dataGridView1.DataSource = students_100;
            Dictionary<string, double> dic = new Dictionary<string, double>();
            foreach(var n in students_100)
            {
                double avg = (n.Chi + n.Eng + n.Math)/3;
                dic.Add(n.Name, Math.Round(avg, 2));
            }
            var q = from stu in dic
                    orderby stu.Value descending
                    group stu by MyGroup(stu.Value) into g
                    select new
                    {
                        學習狀況 = g.Key,
                        人數 = g.Count()
                    };
            dataGridView2.DataSource = q.ToList();
            chart2.DataSource = q.ToList();
            chart2.Series.Add("學習狀況人數");
            chart2.Series[0].XValueMember = "學習狀況";
            chart2.Series[0].YValueMembers = "人數";
            chart2.Series[0].IsValueShownAsLabel = true;
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
        }

        private string MyGroup(double value)
        {
            if (value >= 90) return "優良";
            else if (value >= 70) return "佳";
            else if (value >= 60) return "待加強";
            else return "不及格";
        }

        // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
        // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)



        private void button35_Click(object sender, EventArgs e)
        {
            Clear();
            List<int> sc = new List<int>();
            foreach (var i in students_100)
            {
                sc.Add(i.Chi);
                sc.Add(i.Eng);
                sc.Add(i.Math);
            }
            var q = from s in sc
                    group s by s into g
                    orderby g.Key
                    select new { Scores=g.Key, Count=g.Count() };
            dataGridView1.DataSource = q.ToList();
            chart2.DataSource = q.ToList();
            chart2.Series.Add("分數百分比(%)");
            chart2.Series[0].XValueMember = "Scores";
            chart2.Series[0].YValueMembers = "Count";
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // 統計 :　所有隨機分數出現的次數/比率; sort ascending or descending
            // 63     7.00%
            // 100    6.00%
            // 78     6.00%
            // 89     5.00%
            // 83     5.00%
            // 61     4.00%
            // 64     4.00%
            // 91     4.00%
            // 79     4.00%
            // 84     3.00%
            // 62     3.00%
            // 73     3.00%
            // 74     3.00%
            // 75     3.00%
        }

        private void button34_Click(object sender, EventArgs e)
        {
            Clear();
            var q = from od in dbcontext.Orders.AsEnumerable()
                    group od by od.OrderDate.Value.Year into g                 
                    select new { Year = g.Key, TotalPrice ="NT "+decimal.Round(g.Sum(o=>o.Order_Details.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount))),2),
                        Max=  decimal.Round(g.Max(o => o.Order_Details.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount))), 2 ),Min=decimal.Round( g.Min(o=>o.Order_Details.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount))),2)};
            //MessageBox.Show(q.Max().Year.ToString());
            chart1.DataSource = q.ToList();
            chart1.Series.Add("訂單最低銷售額");
            chart1.Series.Add("訂單最高銷售額");
            chart1.Series[0].XValueMember = "Year";
            chart1.Series[0].YValueMembers = "Min";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[1].XValueMember = "Year";
            chart1.Series[1].YValueMembers = "Max";
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[0].IsValueShownAsLabel = true;
            chart1.Series[1].IsValueShownAsLabel = true;
            var q1 = from od in dbcontext.Order_Details.AsEnumerable()
                    group od by new { od.Order.OrderDate.Value.Year,od.Order.OrderDate.Value.Month } into g
                    select new { Year_Month = $"{ g.Key.Year,-4}/{g.Key.Month,-2}", TotalPrice =decimal.Round( g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount)),2) };
            chart2.DataSource = q1.ToList();
            chart2.Series.Add("月銷售額");
            chart2.Series[0].XValueMember = "Year_Month";
            chart2.Series[0].YValueMembers = "TotalPrice";
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            dataGridView1.DataSource = q.ToList();
            dataGridView2.DataSource = q1.ToList();
            // 年度最高銷售金額 年度最低銷售金額
           
            // 那一年總銷售最好 ? 那一年總銷售最不好 ?  
            // 那一個月總銷售最好 ? 那一個月總銷售最不好 ?

            // 每年 總銷售分析 圖
            // 每月 總銷售分析 圖
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clear();
            Dictionary<decimal, decimal> yearup = new Dictionary<decimal, decimal>();
            var q = from od in dbcontext.Order_Details.AsEnumerable()
                    group od by od.Order.OrderDate.Value.Year into g
                    select new { year = g.Key, Total = g.Sum(p => p.UnitPrice * p.Quantity * (decimal)(1 - p.Discount))};
            for(int i = 1; i < q. ToList().Count(); i++)
            {
                decimal up=decimal.Round( (q.ToList()[i].Total - q.ToList()[i - 1].Total)*100 / q.ToList()[i - 1].Total,2) ;
                yearup.Add(q.ToList()[i].year, up);
            }
            chart1.Series.Add("年成長率(%)");
            chart1.DataSource = yearup;
            chart1.Series[0].XValueMember = "key";
            chart1.Series[0].YValueMembers = "value";
            //chart1.Series[0].Name = "年成長率(%)";
            chart1.Series[0].IsValueShownAsLabel = true;
            dataGridView1.DataSource = yearup;
        }
        void Clear()
        {
            listBox1.Items.Clear();
            chart1.Series.Clear();
            chart2.Series.Clear();
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
        }
    }
}
