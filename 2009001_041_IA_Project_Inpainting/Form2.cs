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
    public partial class Form2 : Form
    {
        Image<Gray, byte> img1;
        Image<Gray, byte> img2;
        Image<Gray, byte> img3;
        Image<Gray, byte> img4;
        Image<Gray, byte> img5;
        Image<Gray, byte> img6;

        Image<Bgr, byte> cimg1;
        Image<Bgr, byte> cimg2;
        Image<Bgr, byte> cimg3;
        Image<Bgr, byte> cimg4;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                cimg1 = new Image<Bgr, byte>("lena512.bmp");

                Image<Gray, byte> img8 = new Image<Gray, byte>(cimg1.Size);

                for (int i = 0; i < cimg1.Rows; i++)
                {
                    for (int j = 0; j < cimg1.Cols; j++)
                    {
                        double y = 0.114 * cimg1[i, j].Blue + 0.299 * cimg1[i, j].Red + 0.587 * cimg1[i, j].Green;

                        img8.Data[i, j, 0] = (byte)y;

                    }
                }

                img2 = new Image<Gray, byte>(img8.Cols + 2, img8.Rows + 2);         //Padded image

                img3 = new Image<Gray, byte>(img8.Cols, img8.Rows);                  //After applying filter


                //for (int i = 1; i < img8.Rows - 1; i++)
                //{
                //    for (int j = 1; j < img8.Cols - 1; j++)
                //    {
                //        double sum = 0;
                //        int c = 2;
                //        sum = img8.Data[i, j, 0] * (2) + img8.Data[i - 1, j, 0] * (c) + img8.Data[i, j - 1, 0] * (c) + img8.Data[i + 1, j, 0] * (c) + img8.Data[i, j + 1, 0] * (c) + img8.Data[i - 1, j - 1, 0] * (c) + img8.Data[i - 1, j + 1, 0] * (c) + img8.Data[i + 1, j - 1, 0] * (c) + img8.Data[i + 1, j + 1, 0] * (c);
                //        img3.Data[i, j , 0] = (byte)(sum/18);
                //    }   
                //}



                for (int i = 1; i <= img8.Rows; i++)
                {

                    for (int j = 1; j <= img8.Cols; j++)
                    {
                        img2.Data[i, j, 0] = img8.Data[i - 1, j - 1, 0];

                    }

                }
                img2.Save("Image2.bmp");




                for (int i = 1; i < img2.Rows - 1; i++)
                {
                    for (int j = 1; j < img2.Cols - 1; j++)
                    {
                        double sum = 0;
                        int c = 1;
                        sum = img2.Data[i, j, 0] * (3) + img2.Data[i - 1, j, 0] * (c) + img2.Data[i, j - 1, 0] * (c) + img2.Data[i + 1, j, 0] * (c) + img2.Data[i, j + 1, 0] * (c) + img2.Data[i - 1, j - 1, 0] * (c) + img2.Data[i - 1, j + 1, 0] * (c) + img2.Data[i + 1, j - 1, 0] * (c) + img2.Data[i + 1, j + 1, 0] * (c);
                        img3.Data[i - 1, j - 1, 0] = (byte)sum;
                    }
                }


                img3.Save("Filter.bmp");

                /************************** low***************************************/

                Image<Gray, byte> ll = new Image<Gray, byte>(img3.Rows / 2, img3.Cols / 2);

                {
                    double[,] C = new Double[img3.Rows / 2, img3.Cols];

                    int k = 0, l = 0;
                    for (int j = 0; j < img3.Cols - 1; j = j + 2)
                    {
                        k = 0;
                        for (int i = 0; i < img3.Rows - 1; i = i + 2)
                        {
                            C[k, l] = img8[i, j].Intensity / 2 + img8[i + 1, j].Intensity / 2;
                            k++;
                        }
                        l++;
                    }

                    double[,] R = new Double[img3.Rows / 2, img3.Cols / 2];
                    k = 0;
                    for (int i = 0; i < img3.Rows / 2 - 3; i = i + 2)
                    {
                        l = 0;
                        for (int j = 0; j < img3.Cols - 3; j = j + 2)
                        {
                            R[k, l] = C[i, j] / 2 + C[i, j + 1] / 2;
                            l++;
                        }
                        k++;
                    }




                    for (int i = 0; i < img3.Rows / 2; i++)
                    {
                        for (int j = 0; j < img3.Cols / 2; j++)
                        {
                            ll.Data[i, j, 0] = (byte)R[i, j];
                        }
                    }

                    imageBox2.Size = ll.Size;
                    imageBox2.Image = ll;


                }









                Image<Gray, byte> hh = new Image<Gray, byte>(img3.Rows / 2, img3.Cols / 2);


                {



                    double[,] R = new Double[img3.Rows, img3.Cols / 2];

                    int k = 0, l = 0;
                    k = 0;
                    for (int i = 0; i < img3.Rows - 3; i = i + 2)
                    {
                        l = 0;
                        for (int j = 0; j < img3.Cols - 3; j = j + 2)
                        {
                            R[k, l] = img3[i, j].Intensity / 2 - img3[i, j + 1].Intensity / 2;
                            l++;
                        }
                        k++;
                    }

                    l = 0;
                    double[,] C = new Double[img3.Rows / 2, img3.Cols / 2];
                    for (int j = 0; j < img3.Cols / 2 - 3; j = j + 2)
                    {
                        k = 0;
                        for (int i = 0; i < img3.Rows - 3; i = i + 2)
                        {
                            C[k, l] = R[i, j] / 2 - R[i + 1, j] / 2;
                            k++;
                        }
                        l++;
                    }


                    for (int i = 0; i < img3.Rows / 2; i++)
                    {
                        for (int j = 0; j < img3.Cols / 2; j++)
                        {
                            hh.Data[i, j, 0] = (byte)R[i, j];
                        }
                    }


                }
                imageBox1.Image = cimg1;
                //imageBox2.Size = hh.Size;
                // imageBox2.Image = hh;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
