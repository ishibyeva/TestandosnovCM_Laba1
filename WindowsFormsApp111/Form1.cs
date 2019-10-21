using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Windows.Forms;
using System.Text.RegularExpressions;

public struct Point
{
    public int num;
    public double x, y;

    public Point(int nn, double xx, double yy)
    {
        num = nn;
        x = xx;
        y = yy;
    }
};


namespace WindowsFormsApp111
{
    public partial class Form1 : Form
    {
        int maxnum;
        GraphPane pane;
        double a, T, u00, h00, E, xn;
        Form2 form = new Form2();


        public Form1()
        {
            InitializeComponent();
            pane = zedGraphControl1.GraphPane;
            pane.Title = "Graphics";
            h00 = double.Parse(textBox5.Text);
            a = double.Parse(textBox6.Text);
            u00 = double.Parse(textBox1.Text);
            T = double.Parse(textBox7.Text);
            maxnum = int.Parse(textBox4.Text);
            E= double.Parse(textBox3.Text);
            xn = double.Parse(textBox2.Text);
            
            




        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                dataGridView1.Rows.Clear();
                Chisltest();

            }
            if (comboBox1.SelectedIndex == 0)
            {
                dataGridView1.Rows.Clear();
                if (u00 > T)
                {
                    Chisltest();
                }
                else MessageBox.Show("Недопустимые значения параметров");

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                this.label5.Visible = true;
                this.label6.Visible = true;
                this.textBox6.Visible = true;
                this.textBox7.Visible = true;


            }
            if (comboBox1.SelectedIndex == 1)
            {
                this.label5.Visible = false;
                this.label6.Visible = false;
                this.textBox6.Visible = false;
                this.textBox7.Visible = false;


            }

        }
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox1.Text == "-")) return;
            var actual = textBox1.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox1.Text, newText) != 0)
            {
                var sstart = textBox1.SelectionStart;
                textBox1.Text = newText;
                textBox1.SelectionStart = sstart - 1;
            }
            u00 = double.Parse(textBox1.Text);
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            form.Show();
        }

        private void HightStep(Point[] arr, int n)
        {
            double maxh=0, minh=0, temh=0;
            for (int i = 0; i < n-1; i++)
            {
                temh = arr[i + 1].x - arr[i].x;
                dataGridView1.Rows[i + 1].Cells[1].Value = temh;
                maxh = Math.Max(maxh,temh);
                minh = Math.Min(minh,temh);
            }
            form.label10.Text = Convert.ToString(Math.Round(maxh,6));
            form.label11.Text = Convert.ToString(Math.Round(minh,6));

        }

        private void DrawExp(Point[] arr, int n)
        {
            pane.CurveList.Clear();

            PointPairList listexp = new PointPairList();
            PointPairList list = new PointPairList();



            for (int i = 0; i < n ; i++)
            {
                // добавим в список точку
                if (comboBox1.SelectedIndex == 1)
                {
                    listexp.Add(arr[i].x, u00*Math.Exp(2 * arr[i].x));
                    
                        dataGridView1.Rows[i].Cells[11].Value = u00*Math.Exp(2 * arr[i].x);
                    dataGridView1.Rows[i].Cells[12].Value = Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells[11].Value) -Convert.ToDouble(dataGridView1.Rows[i].Cells[8].Value));

                }
                if (comboBox1.SelectedIndex == 0)
                {
                    listexp.Add(arr[i].x, (Math.Exp(((-1)*a* arr[i].x) + Math.Log(Math.Abs(u00 - T))) + T));
                    
                        dataGridView1.Rows[i].Cells[11].Value = Math.Exp((-1)*a * arr[i].x + Math.Log(Math.Abs(u00 - T))) + T;
                    dataGridView1.Rows[i].Cells[12].Value = Math.Abs(Convert.ToDouble(dataGridView1.Rows[i].Cells[11].Value) - Convert.ToDouble(dataGridView1.Rows[i].Cells[8].Value));

                }

                list.Add(arr[i].x, arr[i].y);


            }

            // добавим в список точку

            LineItem myCurve = pane.AddCurve("Численное решение", list, Color.Indigo, SymbolType.None);
            
            LineItem myCurve1 = pane.AddCurve("Точное решение", listexp, Color.Red, SymbolType.VDash);
           
          
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }



        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if ((textBox7.Text == "") || (textBox7.Text == "-")) return;
            var actual = textBox7.Text;
            var disallowed = @"[^0-9,-]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox7.Text, newText) != 0)
            {
                var sstart = textBox7.SelectionStart;
                textBox7.Text = newText;
                textBox7.SelectionStart = sstart - 1;
            }
            T = double.Parse(textBox7.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "") return;
            var actual = textBox4.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox4.Text, newText) != 0)
            {
                var sstart = textBox4.SelectionStart;
                textBox4.Text = newText;
                textBox4.SelectionStart = sstart - 1;
            }
            maxnum = int.Parse(textBox4.Text);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text == "") return;
            var actual = textBox3.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox3.Text, newText) != 0)
            {
                var sstart = textBox3.SelectionStart;
                textBox3.Text = newText;
                textBox3.SelectionStart = sstart - 1;
            }
            E = double.Parse(textBox3.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "") return;
            var actual = textBox2.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox2.Text, newText) != 0)
            {
                var sstart = textBox2.SelectionStart;
                textBox2.Text = newText;
                textBox2.SelectionStart = sstart - 1;
            }
            xn = double.Parse(textBox2.Text);
        }

        private double testov(double x, double u)
        {
            return 2 * u;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "") return;
            var actual = textBox6.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox6.Text, newText) != 0)
            {
                var sstart = textBox6.SelectionStart;
                textBox6.Text = newText;
                textBox6.SelectionStart = sstart - 1;
            }
            a = double.Parse(textBox6.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GraphPane  panel = zedGraphControl1.GraphPane;
            double xmin = Convert.ToDouble(textBox8.Text);
            double xmax = Convert.ToDouble(textBox10.Text);
            double ymin = Convert.ToDouble(textBox9.Text);
            double ymax = Convert.ToDouble(textBox11.Text);
            // Устанавливаем интересующий нас интервал по оси X 
            panel.XAxis.Min= xmin;
            panel.XAxis.Max = xmax;
            panel.YAxis.Min = ymin;
            panel.YAxis.Max = ymax;
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию 
            zedGraphControl1.AxisChange();
            // Обновляем график 
            zedGraphControl1.Invalidate();
        }

        double osnov1(double a, double t, double x, double u)
        {
            return ((-1) * a * (u - t));
        }

        

        Point metodRK(double a, double t, double x0, double u0, double h, int num)
        {
            double x, v;
            x = x0;
            v = u0;

            double k1, k2, k3, k4;

            k1 = osnov1(a, t, x, v);
            k2 = osnov1(a, t, x + h *0.5, v + (h*0.5) * k1);
            k3 = osnov1(a, t, x + h*0.5, v + (h*0.5) * k2);
            k4 = osnov1(a, t, x + h, v + (h) * k3);

            v = v + (h / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4);
            
            x += h;

            Point st;
            st.num = num;
            st.x = x;
            st.y = v;

            return st;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "") return;
            var actual = textBox5.Text;
            var disallowed = @"[^0-9,]";
            var newText = Regex.Replace(actual, disallowed, string.Empty);
            if (string.CompareOrdinal(textBox5.Text, newText) != 0)
            {
                var sstart = textBox5.SelectionStart;
                textBox5.Text = newText;
                textBox5.SelectionStart = sstart - 1;
            }
            h00 = double.Parse(textBox5.Text);
        }
      

        Point metodRKfortest(double x0, double u0, double h, int num)
        {

            double x, v;
            x = x0;
            v = u0;

            double k1, k2, k3, k4;

            k1 = testov(x, v);
            k2 = testov( x + h / 2.0, v + (h * 0.5) * k1);
            k3 = testov( x + h / 2.0, v + (h * 0.5) * k2);
            k4 = testov( x + h, v + (h) * k3);

            v = v + (h / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4);

            x += h;

            Point st1;
            st1.num = num;
            st1.x = x;
            st1.y = v;

            return st1;
        }
        void Chisltest()
        {
            Point[] mas = new Point[maxnum];
            double[] e= new double[maxnum];
            double en = 0;
            e[0] = en;

            int n = maxnum, doubstep=0,unstep=0;
            double x0 = 0.0, u0=u00, h0=h00;
           // Form2 Formtwo = new Form2();

            Point t;
            t.num = 0;
            t.x = x0;
            t.y = u0;
            mas[0] = t;

            int i = 1;
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = 0;
            dataGridView1.Rows[0].Cells[1].Value = h00;


            while (i < n)
            {
                Point t1, t12, t2;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = i;

                //(x(n+1),v(n+1)
                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;
                if (comboBox1.SelectedIndex == 0)
                {
                    t1 = metodRK(a, T, x0, u0, h0, i);
                }
                else t1 = metodRKfortest(x0, u0, h0, i);

                //(x(n+1/2),y(n+1/2))

                x0 = mas[i - 1].x;
                u0 = mas[i - 1].y;
                if (comboBox1.SelectedIndex == 0)
                {
                    t12 = metodRK(a, T, x0, u0, h0*0.5, i);
                }
                else t12 = metodRKfortest(x0, u0, h0*0.5, i);
                dataGridView1.Rows[i].Cells[3].Value = t12.y;


                //(x(n),Y(n))

                x0 = t12.x;
                u0 = t12.y;
                if (comboBox1.SelectedIndex == 0)
                {
                    t2 = metodRK(a, T, x0, u0, h0*0.5, i);
                }
                else t2 = metodRKfortest(x0, u0, h0*0.5, i);
                dataGridView1.Rows[i].Cells[4].Value = t2.y;

                dataGridView1.Rows[i].Cells[5].Value = t2.y - t1.y;

                int p = 4; // порядок метода 
                double S = Math.Abs((t2.y - t1.y) / (Math.Pow(2, p) - 1));

                en = Math.Pow(2, p) * S;
                e[i] = en;
                dataGridView1.Rows[i].Cells[6].Value = Math.Round(e[i],8);

                if (S < E / (Math.Pow(2, p + 1)))
                {
                    h0 = 2.0 * h0;
                    doubstep++;
                    dataGridView1.Rows[i].Cells[9].Value = 1;

                    mas[i] = t2;

                    if (mas[i].x > xn)
                        break;
                    i++;
                    continue;
                }
                if (S > E)
                {
                    h0 = h0 * 0.5;
                    dataGridView1.Rows[i].Cells[10].Value = 1;
                    unstep++;
                }
                if ((S >= E / (Math.Pow(2, p + 1))) && (S <= E))
                {
                    mas[i] = t2;
                   // h0 = h0;
                    if (mas[i].x > xn)
                        break;
                    i++;
                    continue;
                }


            }

            form.label7.Text = Convert.ToString(i);
            form.label8.Text = Convert.ToString(doubstep);
            form.label9.Text = Convert.ToString(unstep);

            for (int d = 0; (d < i) && (mas[d].x < xn); d++)
            {
                dataGridView1.Rows[d].Cells[8].Value = Math.Round(mas[d].y,6);
                dataGridView1.Rows[d].Cells[2].Value = Math.Round(mas[d].x,6);
                dataGridView1.Rows[d].Cells[7].Value = Math.Round(e[d] + mas[d].y, 8);

            }
            dataGridView1.RowCount = i;
            DrawExp(mas, i);
            HightStep(mas,i);
        }

    }
}
