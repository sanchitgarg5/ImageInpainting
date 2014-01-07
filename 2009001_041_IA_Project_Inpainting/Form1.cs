using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
namespace _2009001_041_IA_Project_Inpainting
{
    public partial class Form1 : Form
    {
        Image<Gray, byte> img1;
        Image<Gray, byte> img2;
        Image<Gray, byte> img3;
        Image<Gray, byte> img4;
        Image<Gray, byte> img5;
        Image<Gray, byte> img6;

        String[,] farray;
        double[,] tarray;

        public Form1()
        {
            InitializeComponent();
        }

        private void bnt_grayscale_Click(object sender, EventArgs e)
        {
            GrayScale ob = new GrayScale();

            ob.Show();
        }

        private void btn_Colouredimages_Click(object sender, EventArgs e)
        {
            Colored ob = new Colored();

            ob.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // applying sobel operator to get gradient of the image

            img1 = new Image<Gray, byte>("text.bmp");
            img2 = new Image<Gray, byte>(img1.Cols + 2, img1.Rows + 2);//padded image

            //img3 = new Image<Gray, byte>(img1.Cols, img1.Rows);
            img3 = new Image<Gray, byte>(img1.Size);
            img4 = new Image<Gray, byte>(img1.Size);
            img5 = new Image<Gray, byte>(img1.Size);

            /************* Padding***********************/
            for (int i = 0; i < img1.Cols + 2; i++)
            {
                img2.Data[0, i, 0] = (byte)0;


            }
            for (int i = 0; i < img1.Cols + 2; i++)
            {
                img2.Data[img1.Rows + 1, i, 0] = (byte)0;

            }
            for (int i = 0; i < img1.Rows + 2; i++)
            {
                img2.Data[i, 0, 0] = (byte)0;

            }
            for (int i = 0; i < img1.Rows + 2; i++)
            {
                img2.Data[i, img1.Cols + 1, 0] = (byte)0;

            }
            int j = 1;

            for (int i = 1; i <= img1.Rows; i++)
            {

                for (j = 1; j <= img1.Cols; j++)
                {
                    img2.Data[i, j, 0] = img1.Data[i - 1, j - 1, 0];

                }


            }

            /********************* Sobel operator****************************/

            for (int i = 1; i < img2.Rows - 1; i++)
            {
                for (int k = 1; k < img2.Cols - 1; k++)
                {
                    double sumx = (-1) * img2.Data[i + 1, k - 1, 0] + 0 * img2.Data[i + 1, k, 0] + 1 * img2.Data[i + 1, k + 1, 0] + (-2) * img2.Data[i, k - 1, 0] + 0 * img2.Data[i, k, 0] + 2 * img2.Data[i, k + 1, 0] + (-1) * img2.Data[i - 1, k - 1, 0] + 0 * img2.Data[i - 1, k, 0] + 1 * img2.Data[i - 1, k + 1, 0];
                    double sumy = (-1) * img2.Data[i + 1, k - 1, 0] + (-2) * img2.Data[i + 1, k, 0] + (-1) * img2.Data[i + 1, k + 1, 0] + 0 * img2.Data[i, k - 1, 0] + 0 * img2.Data[i, k, 0] + 0 * img2.Data[i, k + 1, 0] + 1 * img2.Data[i - 1, k - 1, 0] + 2 * img2.Data[i - 1, k, 0] + 1 * img2.Data[i - 1, k + 1, 0];

                    img3.Data[i - 1, k - 1, 0] = (byte)sumx;
                    img4.Data[i - 1, k - 1, 0] = (byte)sumy;
                    img5.Data[i - 1, k - 1, 0] = (byte)(Math.Sqrt(Math.Pow(sumx, 2) + Math.Pow(sumy, 2)));
                }
            }
            //img2.Save("padded.bmp");
            //imageBox1.Width = img2.Cols;
            //imageBox1.Height = img2.Rows;
            //imageBox1.Image = img2;
            CvInvoke.cvShowImage("x", img3);
            CvInvoke.cvShowImage("y", img4);
            CvInvoke.cvShowImage("both", img5);


            //now we have calculated the gradient of the image our task now is to calculate its mask initialization and other stuff
            //Image<Gray, byte> img7 = new Image<Gray, byte>("text4.bmp");  //image with objct marked which need to be removed

            Image<Gray, byte> img8 = new Image<Gray, byte>("text2.bmp"); //binary mask of that image

            Image<Gray, byte> img9 = new Image<Gray, byte>(img8.Size);

            Image<Gray, byte> img10 = new Image<Gray, byte>(img8.Size);//boundary of the mask

            /******************** boundary extraction**********************/

            StructuringElementEx se = new StructuringElementEx(3, 3, 3 / 2, 3 / 2, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);

            CvInvoke.cvErode(img8, img9, se, 1);

            //img8.Save("Erode.bmp");

            img10 = img8 - img9;

            //img10.Save("Boundary.bmp");

            //imageBox2.Image = img4;

            farray = new string[img10.Rows, img10.Cols];
            tarray = new double[img10.Rows, img10.Cols];
            List<Point> point_list = new List<Point>();
            List<double> distance = new List<double>();
            List<double> distance2 = new List<double>();
            //ArrayList point = new ArrayList();
            // initialize farray and tarray

            for (int i = 0; i < img10.Rows; i++)
            {
                for (int k = 0; k < img10.Cols; k++)
                {
                    if (img10.Data[i, k, 0] == 255)
                    {

                        point_list.Add(new Point(i, k));
                        farray[i, k] = "BAND";
                        tarray[i, k] = 0;
                        distance.Add(tarray[i, k]);
                        distance2.Add(tarray[i, k]);
                    }

                    if (img8.Data[i, k, 0] == 0)
                    {
                        //Console.WriteLine("idhar aya kya");
                        farray[i, k] = "KNOWN";
                        tarray[i, k] = 0;
                    }

                    if (img8.Data[i, k, 0] == 255 && img10.Data[i, k, 0] == 0)
                    {
                        //Console.WriteLine("why here");
                        farray[i, k] = "INSIDE";
                        tarray[i, k] = 1000000;
                    }
                }
            }
            distance.Sort();

            /****************From boundary move inside the masked region in shortest direction ********************/

            while (point_list.Count != 0)
            {
                //Console.WriteLine("here");
                double dist = distance[0];

                int index = distance2.IndexOf(dist);
                Point p = point_list[index];
                point_list.Remove(p);
                distance.Remove(dist);
                distance2.Remove(dist);
                farray[p.X, p.Y] = "KNOWN";
                //1 neighbour
                int k = p.X - 1;
                int l = p.Y;
                inpaint(p.X, p.Y);
                if (farray[k, l] != "KNOWN")
                {
                    Console.WriteLine("here5");
                    if (farray[k, l].Equals("INSIDE"))
                    {
                        farray[k, l] = "BAND";
                        inpaint(k, l);
                    }
                    //tarray[k,l]=solve(k-1,l,k,l-1);
                    double temp1 = Math.Min(solve(k - 1, l, k, l - 1), solve(k + 1, l, k, l - 1));
                    double temp2 = Math.Min(solve(k - 1, l, k, l + 1), solve(k + 1, l, k, l + 1));
                    tarray[k, l] = Math.Min(temp1, temp2);
                    point_list.Add(new Point(k, l));
                    distance.Add(tarray[k, l]);
                    distance2.Add(tarray[k, l]);
                }

                //2 neighbour
                k = p.X;
                l = p.Y - 1;

                if (!(farray[k, l].Equals("KNOWN")))
                {

                    if (farray[k, l].Equals("INSIDE"))
                    {
                        farray[k, l] = "BAND";
                        inpaint(k, l);
                    }
                    //tarray[k,l]=solve(k-1,l,k,l-1);
                    double temp1 = Math.Min(solve(k - 1, l, k, l - 1), solve(k + 1, l, k, l - 1));
                    double temp2 = Math.Min(solve(k - 1, l, k, l + 1), solve(k + 1, l, k, l + 1));
                    tarray[k, l] = Math.Min(temp1, temp2);
                    point_list.Add(new Point(k, l));
                    distance.Add(tarray[k, l]);
                    distance2.Add(tarray[k, l]);
                }

                //3 neighbour
                k = p.X + 1;
                l = p.Y;

                if (!(farray[k, l].Equals("KNOWN")))
                {

                    if (farray[k, l].Equals("INSIDE"))
                    {
                        farray[k, l] = "BAND";
                        inpaint(k, l);
                    }
                    //tarray[k,l]=solve(k-1,l,k,l-1);
                    double temp1 = Math.Min(solve(k - 1, l, k, l - 1), solve(k + 1, l, k, l - 1));
                    double temp2 = Math.Min(solve(k - 1, l, k, l + 1), solve(k + 1, l, k, l + 1));
                    tarray[k, l] = Math.Min(temp1, temp2);
                    point_list.Add(new Point(k, l));
                    distance.Add(tarray[k, l]);
                    distance2.Add(tarray[k, l]);
                }


                //4 neighbour
                k = p.X;
                l = p.Y + 1;

                if (!(farray[k, l].Equals("KNOWN")))
                {

                    if (farray[k, l].Equals("INSIDE"))
                    {
                        farray[k, l] = "BAND";
                        inpaint(k, l);
                    }
                    //tarray[k,l]=solve(k-1,l,k,l-1);
                    double temp1 = Math.Min(solve(k - 1, l, k, l - 1), solve(k + 1, l, k, l - 1));
                    double temp2 = Math.Min(solve(k - 1, l, k, l + 1), solve(k + 1, l, k, l + 1));
                    tarray[k, l] = Math.Min(temp1, temp2);
                    point_list.Add(new Point(k, l));
                    distance.Add(tarray[k, l]);
                    distance2.Add(tarray[k, l]);
                }


                distance.Sort();
            }

            CvInvoke.cvShowImage("finally", img1);
        }

        public double solve(int i1, int j1, int i2, int j2)
        {
            double sol = 1.0e6;

            if (farray[i1, j1].Equals("KNOWN"))
            {
                if (farray[i2, j2].Equals("KNOWN"))
                {
                    double r = Math.Sqrt(2 - ((tarray[i1, j1] - tarray[i2, j2]) * (tarray[i1, j1] - tarray[i2, j2])));   //error in function have to check
                    double s = (tarray[i1, j1] + tarray[i2, j2] - r) / 2;
                    if (s >= tarray[i1, j1] && s >= tarray[i2, j2])
                    {
                        sol = s;
                    }
                    else
                    {
                        s = s + r;
                        if (s >= tarray[i1, j1] && s >= tarray[i2, j2])
                        {
                            sol = s;
                        }
                    }
                }
                else
                {
                    sol = 1 + tarray[i1, j1];
                }
            }
            else
            {
                if (farray[i2, j2].Equals("KNOWN"))
                {
                    sol = 1 + tarray[i2, j2];   //doubt in this
                }
            }
            return sol;
        }



        //inpaint function
        public void inpaint(int a, int b)
        {
            //Console.WriteLine("here3");
            // for all neighbours of the point
            //neighbour 1
            int x = a + 1;
            int y = b;
            double IA = 0;
            Double S = 0;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * img5.Data[a, b, 0] / Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = w;
            }

            //neighbour 2
            x = a + 1;
            y = b + 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 3
            x = a;
            y = b + 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 4
            x = a - 1;
            y = b + 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 5
            x = a - 1;
            y = b;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 6
            x = a - 1;
            y = b - 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 7
            x = a;
            y = b - 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            //neighbour 8
            x = a + 1;
            y = b - 1;
            if (!(farray[x, y].Equals("INSIDE")))
            {
                double direction = (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1)) * (img5.Data[a, b, 0] / Math.Sqrt(2));
                double dst = 1 / (Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)) * Math.Sqrt(Math.Pow(a - x, 2) + Math.Pow(b - y, 2)));
                double lev = 1 / (1 + Math.Abs(tarray[x, y] - tarray[a, b]));

                double w = direction * dst * lev;

                IA = IA + w * (img1.Data[x, y, 0] + (img5.Data[x, y, 0] * (img4.Data[x, y, 0] / (img3.Data[x, y, 0] + 1))));

                S = S + w;
            }

            img1.Data[a, b, 0] = (byte)(IA / S);
        }
    }
}
