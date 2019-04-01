using KMZI_lab5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KMZI_laba7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnGetKey_Click(object sender, RoutedEventArgs e)
        {
            tblRes2.Text = "";
            try
            {
                int p = int.Parse(tbm.Text.Trim());
                int t = int.Parse(tbt2.Text.Trim());
                int n = int.Parse(tbn2.Text.Trim());
                string[] s = tbXiYi.Text.Trim().Split(' ');
                int[,] ki = new int[s.Length, 2];
                if (s.Length < t) throw new Exception("Не достаточно данных!");
                for (int i = 0; i < t; i++)
                {
                    string buf = s[i];
                    string[] si = s[i].Split(',');
                    ki[i, 0] = int.Parse(si[0]);
                    ki[i, 1] = int.Parse(si[1]);
                  //  tblRes2.Text += ki[i, 0] + " - " + ki[i, 1] + "\n";
                }
                int[,] matrix = new int[t, t + 1];
                for (int i = 0; i < t; i++)
                {
                    int c = 0;
                    while (c <= t - 1)
                    {
                        matrix[i, c] = (int)Math.Pow(ki[i, 0], c);
                        matrix[i, c] %= p;
                        c++;
                    }
                    int buf = matrix[i, 0]; buf %= p;
                    matrix[i, 0] = matrix[i, t - 1]; matrix[i, 0] %= p;
                    matrix[i, t - 1] = buf;
                    matrix[i, t] = ki[i, 1]; matrix[i, t] %= p;
                }

                matrix = gauss(matrix, t, p);
                //for (int i = 0; i < t; i++)
                //{
                //    for (int j = 0; j < t + 1; j++)
                //        tblRes2.Text += numMod(matrix[i, j],p) + " ";
                //    tblRes2.Text += "\n";
                //}
                //matrix[t - 1, t - 1] = numMod(matrix[t - 1, t - 1], p);
                //matrix[t - 1, t] = numMod(matrix[t - 1, t], p);
                tblRes2.Text +="Key = "+ tchmk.DecisionCompare(matrix[t - 1, t - 1], p, matrix[t - 1, t]);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка"); }
        }
        static int[,] gauss(int[,] A, int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                A[i, i] = numMod(A[i, i], m);
                int buf = tchmk.InverseNumber(A[i, i], m, 1);
                for (int j = i; j < n + 1; j++)
                {
                    A[i, j] *= buf;
                    numMod(A[i,j], m);
                }

                for (int k = i + 1; k < n; k++)
                {
                    buf = A[k, i]; numMod(buf, m);
                    for (int j = i; j < n + 1; j++)
                    {
                        A[k, j] = buf * A[i, j] - A[k, j];
                        numMod(A[k, j], m);
                    }
                }
            }
            return A;
        }
        private static int numMod(int num,int mod)
        {
            while (num > mod)
            {
                num -= mod;
            }
            while (num < 0)
            {
                num += mod;
            }
            if (num == mod) return 0;
            return num;
        }
        private void BtnCutKey_Click(object sender, RoutedEventArgs e)
        {
            int t = int.Parse(tbt.Text.Trim());//т
            int k = int.Parse(tbk.Text.Trim());//ключ
            int n = int.Parse(tbn.Text.Trim());//кол-во участников

            int p = k + 2;//m+2; модуль - первое простое число большее, чем 
            while (!tchmk.CheckForSimplicity(p)) p++;
            // MessageBox.Show(p.ToString());

            List<int> a = new List<int>();//коэфф 
                                          //  List<int> ki = new List<int>();//части ключей, которые даются каждому участнику
            int[,] ki = new int[n, 2];
            //ki.Add(0);
            for (int i = 0; i < t - 1; i++)
                a.Add(rand.Next(1, p));
            a.Add(k);//а0=м

            //генерация долей ключа
            //int proizv = 1;
            tblRes1.Text = "(xi,yi) (mod " + p.ToString() + ")\n";
            for (int i = 0; i < n; i++)
            {
                ki[i, 0] = 1 + i;
                ki[i, 1] = numMod(tchmk.Gorner(a, i + 1, p),p);
                tblRes1.Text += ki[i, 0] + " - " + ki[i, 1] + "\n";
            }
        }
    }
}
