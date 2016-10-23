using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreeStone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Panel srcPanel, dstPanel;
        int[] Stones = new int[9];//Taşların dizilişi bulunur.
        int ACnt = 0;//Sıradaki hamle belirlenir;
        bool TS = true;//bilgisayarın ilk hamlesinde TS varmı diye bakar
        int mode = 7;//Oyunun modunu belirler.  0=1=2=3=4=5=Put, 6=Move, 7=Wait,
        int StnX, StnY;//Stone koodinatları tutulur.
        Random R = new Random();
        int Red0, Green0, Blue0, Red1, Green1, Blue1, RedL, GreenL, BlueL;
        private void SelectColor()
        {
            Red0 = R.Next(0, 256);
            Green0 = R.Next(0, 256);
            Blue0 = R.Next(0, 256);
            Red1 = R.Next(0, 256);
            Green1 = R.Next(0, 256);
            Blue1 = R.Next(0, 256);
            RedL = 53;
            GreenL = 164;
            BlueL = 48;
            label1.ForeColor = Color.FromArgb(R.Next(0, 256), Red0, Green0, Blue0);
            label2.ForeColor = Color.FromArgb(R.Next(0, 256), Red1, Green1, Blue1);
            label3.ForeColor = Color.FromArgb(R.Next(0, 256), R.Next(0, 256), R.Next(0, 256), R.Next(0, 256));
        }
        private void GDLine()
        {
            Graphics DLine;
            DLine = this.CreateGraphics();
            Pen PL = new Pen(Color.FromArgb(255, RedL, GreenL, BlueL), 6);
            DLine.Clear(this.BackColor);
            DLine.DrawLine(PL, 100, 100, 400, 400);
            DLine.DrawLine(PL, 400, 100, 100, 400);
            DLine.DrawLine(PL, 250, 100, 250, 400);
            DLine.DrawLine(PL, 400, 250, 100, 250);
            DLine.DrawLine(PL, 100, 100, 100, 400);
            DLine.DrawLine(PL, 400, 100, 400, 400);
            DLine.DrawLine(PL, 100, 100, 400, 100);
            DLine.DrawLine(PL, 100, 400, 400, 400);
            DLine.Dispose();
        }
        private void Starting()
        {
            for (int i = 0; i < 9; i++)
            {
                Stones[i] = 2;
            }
            SelectColor();
            Board();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            mode = 7;
            Starting();
            panel1.Name = "0";
            panel2.Name = "1";
            panel3.Name = "2";
            panel4.Name = "3";
            panel5.Name = "4";
            panel6.Name = "5";
            panel7.Name = "6";
            panel8.Name = "7";
            panel9.Name = "8";
            //for (int i = 0; i <= this.Controls.Count - 1; i++)
            //    if (this.Controls[i] is Label)
            //        MessageBox.Show(i+". "+this.Controls[i].Name.ToString());
                    

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            GDLine();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            mode = 0;
            ACnt = 0;
            Starting();
        }
        private void Board()
        {
            for (int i = 0; i < 9; i++)
            {
                if (Stones[i]==0)
                    this.Controls[8-i].BackgroundImage = Image.FromFile("Blue.png");
                else if(Stones[i]==1)
                    this.Controls[8 - i].BackgroundImage = Image.FromFile("Red.png");
                else
                    switch (this.Controls[8 - i].Name)
                    {
                        case "0": this.Controls[8 - i].BackgroundImage = Image.FromFile("0.png"); break;
                        case "1": this.Controls[8 - i].BackgroundImage = Image.FromFile("1.png"); break;
                        case "2": this.Controls[8 - i].BackgroundImage = Image.FromFile("2.png"); break;
                        case "3": this.Controls[8 - i].BackgroundImage = Image.FromFile("3.png"); break;
                        case "4": this.Controls[8 - i].BackgroundImage = Image.FromFile("4.png"); break;
                        case "5": this.Controls[8 - i].BackgroundImage = Image.FromFile("5.png"); break;
                        case "6": this.Controls[8 - i].BackgroundImage = Image.FromFile("6.png"); break;
                        case "7": this.Controls[8 - i].BackgroundImage = Image.FromFile("7.png"); break;
                        case "8": this.Controls[8 - i].BackgroundImage = Image.FromFile("8.png"); break;
                    }
            }
            if (mode != 7)
            {
                if (ACnt == 0)
                    label3.Text = "Mavi";
                else
                    label3.Text = "Kırmızı";
            }
        }
        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            srcPanel = (Panel)sender;
            if (mode==6 && Stones[Convert.ToInt16(srcPanel.Name)]==ACnt)//sıradaki doğru oyucuyu oynamasını sağlar
            {//panele doğru tıklandığında DragDrop a izin verir
                srcPanel.DoDragDrop(srcPanel.BackgroundImage, DragDropEffects.All);
            }
            if (mode<6)
            {
                Stones[Convert.ToInt16(srcPanel.Name)] = ACnt;
                ACnt = (ACnt + 1) % 2;
                mode++;
                Board();
                if (Three_Stone())
                {
                    MessageBox.Show("Congratulations, " + (ACnt + 1) + ". Player Win");
                    this.Controls[ACnt + 10].Text = Convert.ToString(Convert.ToInt16(this.Controls[ACnt + 10].Text) + 1);
                    mode = 7;
                }
            }
        }
        private void panel_DragEnter(object sender, DragEventArgs e)
        {//izin verildiğinde drag işlemi başlatıldığında çalışır
            if (mode == 6)
            {
                if (e.Data.GetDataPresent(typeof(Bitmap)))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        private void panel_DragOver(object sender, DragEventArgs e)
        {//Nesne üzerin de bulunduğu süre boyunca çalışır
            //if (Mode == 6)
            //{
            //    if (e.Data.GetDataPresent(typeof(Bitmap)))
            //    {
            //        e.Effect = DragDropEffects.Copy;
            //        Stones[Convert.ToInt16((sender as Panel).Name)] = 2;
            //        Board();
            //    }
            //    else
            //    {
            //        e.Effect = DragDropEffects.None;
            //    }
            //}
        }
        private void panel_DragDrop(object sender, DragEventArgs e)
        {//nesne üzerine Drop işlemi yapıldığında çalışır
            dstPanel = (Panel)sender;
            if ((mode==6 && Stones[Convert.ToInt16(dstPanel.Name)]==2) && ((srcPanel.Name=="0" && (dstPanel.Name=="1" || dstPanel.Name=="3")) || (srcPanel.Name=="1" && (dstPanel.Name=="2" || dstPanel.Name=="0")) || (srcPanel.Name=="2" && (dstPanel.Name=="5" || dstPanel.Name=="1")) || (srcPanel.Name=="3" && (dstPanel.Name=="0" || dstPanel.Name=="6")) || (srcPanel.Name=="5" && (dstPanel.Name=="2" || dstPanel.Name=="8")) || (srcPanel.Name=="6" && (dstPanel.Name=="3" || dstPanel.Name=="7")) || (srcPanel.Name=="7" && (dstPanel.Name=="8" || dstPanel.Name=="6")) || (srcPanel.Name=="8" && (dstPanel.Name=="5" || dstPanel.Name=="7")) || srcPanel.Name=="4" || dstPanel.Name=="4"))
            {//doğru oynama alanı seçildi
                dstPanel.BackgroundImage = (Bitmap)e.Data.GetData(typeof(Bitmap));
                Stones[Convert.ToInt16(srcPanel.Name)] = 2;
                Stones[Convert.ToInt16(dstPanel.Name)] = ACnt;
                ACnt = (ACnt + 1) % 2;
                Board();
                if (Three_Stone())
                {
                    MessageBox.Show("Congratulations, " + Convert.ToInt16(ACnt + 1) + ". Player Win");
                    this.Controls[ACnt + 10].Text = Convert.ToString(Convert.ToInt16(this.Controls[ACnt + 10].Text) + 1);
                    mode = 7;
                    label3.Text = "Game Start";
                }
            }
        }
        private bool Three_Stone()
        {
            if (Stones[4] != 2 && ((Stones[0] == Stones[4] && Stones[4] == Stones[8]) || (Stones[2] == Stones[4] && Stones[4] == Stones[6])))//Crash
            {
                return true;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Stones[i * 3 + 1] != 2 && Stones[i * 3] == Stones[i * 3 + 1] && Stones[i * 3 + 1] == Stones[i * 3 + 2])//Horizontel
                    {
                        return true;
                    }
                    if (Stones[i + 3] != 2 && Stones[i] == Stones[i + 3] && Stones[i + 3] == Stones[i + 6])//Horizontel
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
