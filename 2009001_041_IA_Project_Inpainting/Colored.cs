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
    public partial class Colored : Form
    {
        String filename;

        Image<Bgr, byte> cimg1;
        Image<Bgr, byte> cimg2;
        Image<Bgr, byte> cimg3;
        Image<Bgr, byte> cimg4;
        Image<Bgr, byte> cimg5;

        Image<Gray, byte> img1;
        Image<Gray, byte> img2;
        Image<Gray, byte> img3;
        Image<Gray, byte> img4;
        Image<Gray, byte> img5;
        Image<Gray, byte> img6;

        public Colored()
        {
            InitializeComponent();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            filename = openFileDialog1.FileName.ToString();



            cimg1 = new Image<Bgr, byte>(filename);
            imageBox1.Height = cimg1.Height;
            imageBox1.Width = cimg1.Width;

            imageBox1.Image = cimg1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Object Removal".ToString())
                label_maskMethod.Text = "You will need to select the mask manually";

            else if (comboBox1.Text == "Text Removal".ToString())
                label_maskMethod.Text = "The mask will be created automatically";
        }

        private void btn_inpaint_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Object Removal".ToString())
                Colored_ObjectRemoval();

            else if (comboBox1.Text == "Text Removal".ToString())
                Colored_TextRemoval();
        }

        void Colored_ObjectRemoval()
        {
            String file = filename;
            Image img = Image.FromFile(file);

            img.Save("MaskingImage.bmp");

            img.Tag = "MaskingImage.bmp";

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = ("mspaint.exe");
            info.Arguments = img.Tag.ToString(); // returns full path and name
            img.Dispose();
            System.Diagnostics.Process p1 = System.Diagnostics.Process.Start(info);

            while (p1.Responding == true) ;


            Console.WriteLine("After paint");

            Bgr white = new Bgr(255, 255, 255);
            cimg1 = new Image<Bgr, byte>(file);
            cimg3 = new Image<Bgr, byte>("MaskingImage.bmp");

            cimg2 = new Image<Bgr, byte>(cimg1.Size);

            for (int i = 0; i < cimg3.Rows; i++)
            {
                for (int j = 0; j < cimg3.Cols; j++)
                {
                    if (!(cimg1[i, j].Equals(cimg3[i, j])))
                    {

                        cimg2[i, j] = white;
                    }
                }
            }

            cimg2.Save("Mask.bmp");                                                                       //Remove comments
            cimg3 = new Image<Bgr, byte>("MaskingImage.bmp");
            //imageBox1.Image = cimg3;
            //imageBox2.Image = cimg2;



            /************************** Inpainting**********************************/

            cimg3 = cimg1;
            //cimg2 = new Image<Bgr, byte>("Mask.bmp");                                                        // Add comments


            for (int i = 0; i < cimg3.Rows; i++)
            {
                for (int j = 0; j < cimg3.Cols; j++)
                {
                    if (cimg2[i, j].Blue == 255)
                    {
                        cimg3.Data[i, j, 0] = (byte)0;
                    }
                    if (cimg2[i, j].Green == 255)
                    {
                        cimg3.Data[i, j, 1] = (byte)0;
                    }
                    if (cimg2[i, j].Red == 255)
                    {
                        cimg3.Data[i, j, 2] = (byte)0;
                    }
                }
            }


            cimg3.Save("Initialise.bmp");
            cimg4 = cimg3;
            cimg4.Save("New.bmp");

            for (int k = 0; k < 100; k++)
            {
                double sum = 0;
                for (int i = 1; i < cimg3.Rows - 1; i++)
                {
                    for (int j = 1; j < cimg3.Cols - 1; j++)
                    {
                        if (cimg2[i, j].Blue == 255 && cimg2[i, j].Green == 255 && cimg2[i, j].Red == 255)
                        {
                            double c = 0.125;
                            sum = cimg3[i, j].Blue * (0) + cimg3[i - 1, j].Blue * (c) + cimg3[i + 1, j].Blue * (c) + cimg3[i - 1, j - 1].Blue * (c) + cimg3[i - 1, j + 1].Blue * (c) + cimg3[i, j - 1].Blue * (c) + cimg3[i, j + 1].Blue * (c) + cimg3[i + 1, j - 1].Blue * (c) + cimg3[i + 1, j + 1].Blue * (c);

                            cimg3.Data[i, j, 0] = (byte)sum;

                            sum = cimg3[i, j].Green * (0) + cimg3[i - 1, j].Green * (c) + cimg3[i + 1, j].Green * (c) + cimg3[i - 1, j - 1].Green * (c) + cimg3[i - 1, j + 1].Green * (c) + cimg3[i, j - 1].Green * (c) + cimg3[i, j + 1].Green * (c) + cimg3[i + 1, j - 1].Green * (c) + cimg3[i + 1, j + 1].Green * (c);

                            cimg3.Data[i, j, 1] = (byte)sum;

                            sum = cimg3[i, j].Red * (0) + cimg3[i - 1, j].Red * (c) + cimg3[i + 1, j].Red * (c) + cimg3[i - 1, j - 1].Red * (c) + cimg3[i - 1, j + 1].Red * (c) + cimg3[i, j - 1].Red * (c) + cimg3[i, j + 1].Red * (c) + cimg3[i + 1, j - 1].Red * (c) + cimg3[i + 1, j + 1].Red * (c);

                            cimg3.Data[i, j, 2] = (byte)sum;



                        }



                    }
                }
            }

            cimg3.Save("Colored_ObjectRemoval.bmp");

           // imageBox1.Image = (cimg1);


            imageBox2.Image = cimg3;

        }

        void Colored_TextRemoval()
         {

             img1 = new Image<Gray, byte>(filename);

            
             //img1=img1.SmoothBlur(3, 3, true);
             img2 = new Image<Gray, byte>(img1.Size);

             img3 = new Image<Gray, byte>(img1.Size);

             img4 = new Image<Gray, byte>(img1.Size);

             img5 = new Image<Gray, byte>(img1.Size);
             img6 = new Image<Gray, byte>(img1.Size);
             int se_size = 3;
             StructuringElementEx se = new StructuringElementEx(se_size, se_size, se_size / 2, se_size / 2, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);

             CvInvoke.cvMorphologyEx(img1, img2, img4, se, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN, 1);
             CvInvoke.cvMorphologyEx(img1, img6, img4, se, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 2);
             for (int i = 0; i < img1.Rows; i++)
             {
                 for (int j = 0; j < img1.Cols; j++)
                 {
                     img3.Data[i, j, 0] = (byte)(img6.Data[i, j, 0] - img1.Data[i, j, 0]);

                     if (img3.Data[i, j, 0] >= 55)
                     {
                         img5.Data[i, j, 0] = (byte)255;
                     }
                 }
             }
             img5 = img5.SmoothMedian(3);
             img5 = img5.Dilate(2);
             //img5=img5.SmoothGaussian(3,3,1,1)
             img5.Save("Mask.bmp");

             /************************** Inpainting**********************************/

             cimg3 =new Image<Bgr,byte>(filename);

             cimg2 = new Image<Bgr, byte>("Mask.bmp");                                                        // Add comments


             for (int i = 0; i < cimg3.Rows; i++)
             {
                 for (int j = 0; j < cimg3.Cols; j++)
                 {
                     if (cimg2[i, j].Blue == 255)
                     {
                         cimg3.Data[i, j, 0] = (byte)0;
                     }
                     if (cimg2[i, j].Green == 255)
                     {
                         cimg3.Data[i, j, 1] = (byte)0;
                     }
                     if (cimg2[i, j].Red == 255)
                     {
                         cimg3.Data[i, j, 2] = (byte)0;
                     }
                 }
             }


             cimg3.Save("Initialise.bmp");
             cimg4 = cimg3;
             cimg4.Save("New.bmp");

             for (int k = 0; k < 100; k++)
             {
                 double sum = 0;
                 for (int i = 1; i < cimg3.Rows - 1; i++)
                 {
                     for (int j = 1; j < cimg3.Cols - 1; j++)
                     {
                         if (cimg2[i, j].Blue == 255 && cimg2[i, j].Green == 255 && cimg2[i, j].Red == 255)
                         {
                             double c = 0.125;
                             sum = cimg3[i, j].Blue * (0) + cimg3[i - 1, j].Blue * (c) + cimg3[i + 1, j].Blue * (c) + cimg3[i - 1, j - 1].Blue * (c) + cimg3[i - 1, j + 1].Blue * (c) + cimg3[i, j - 1].Blue * (c) + cimg3[i, j + 1].Blue * (c) + cimg3[i + 1, j - 1].Blue * (c) + cimg3[i + 1, j + 1].Blue * (c);

                             cimg3.Data[i, j, 0] = (byte)sum;

                             sum = cimg3[i, j].Green * (0) + cimg3[i - 1, j].Green * (c) + cimg3[i + 1, j].Green * (c) + cimg3[i - 1, j - 1].Green * (c) + cimg3[i - 1, j + 1].Green * (c) + cimg3[i, j - 1].Green * (c) + cimg3[i, j + 1].Green * (c) + cimg3[i + 1, j - 1].Green * (c) + cimg3[i + 1, j + 1].Green * (c);

                             cimg3.Data[i, j, 1] = (byte)sum;

                             sum = cimg3[i, j].Red * (0) + cimg3[i - 1, j].Red * (c) + cimg3[i + 1, j].Red * (c) + cimg3[i - 1, j - 1].Red * (c) + cimg3[i - 1, j + 1].Red * (c) + cimg3[i, j - 1].Red * (c) + cimg3[i, j + 1].Red * (c) + cimg3[i + 1, j - 1].Red * (c) + cimg3[i + 1, j + 1].Red * (c);

                             cimg3.Data[i, j, 2] = (byte)sum;



                         }



                     }
                 }
             }

             cimg3.Save("Colored_TextRemoval.bmp");

             imageBox1.Image = (cimg1);


             imageBox2.Image = cimg3;
         }

        private void Colored_Load(object sender, EventArgs e)
        {

        }
    }
}
