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
    public partial class GrayScale : Form
    {
        Image<Gray, byte> img1;
        Image<Gray, byte> img2;
        Image<Gray, byte> img3;
        Image<Gray, byte> img4;
        Image<Gray, byte> img5;
        Image<Gray, byte> img6;

        String filename;

        public GrayScale()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Object Removal".ToString())
                label_maskMethod.Text = "You will need to select the mask manually";

            else if (comboBox1.Text == "Text Removal".ToString())
                label_maskMethod.Text = "The mask will be created automatically";

        }

        private void GrayScale_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            filename = openFileDialog1.FileName.ToString();

            

            img1 = new Image<Gray, byte>(filename);
            imageBox1.Height = img1.Height;
            imageBox1.Width = img1.Width;

            imageBox1.Image = img1;
        }

        private void btn_inpaint_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Object Removal".ToString())
                Gray_ObjectRemoval();

            else if (comboBox1.Text == "Text Removal".ToString())
                Gray_TextRemoval();
        }


        void Gray_TextRemoval()
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


            img5.Save("Mask.bmp");

            img2 = img5;

            img3 = img1;


            for (int k = 0; k < 100; k++)
            {
                double sum = 0;
                for (int i = 0; i < img3.Rows-1; i++)
                {
                    for (int j = 0; j < img3.Cols-1; j++)
                    {
                        if (img2[i, j].Intensity == 255)
                        {
                            double c = 0.125;
                            sum = img3.Data[i, j, 0] * (0) + img3.Data[i - 1, j, 0] * (c) + img3.Data[i, j - 1, 0] * (c) + img3.Data[i + 1, j, 0] * (c) + img3.Data[i, j + 1, 0] * (c) + img3.Data[i - 1, j - 1, 0] * (c) + img3.Data[i - 1, j + 1, 0] * (c) + img3.Data[i + 1, j - 1, 0] * (c) + img3.Data[i + 1, j + 1, 0] * (c);

                            img3.Data[i, j, 0] = (byte)sum;
                        }

                    }
                }
            }
            imageBox2.Image = img3;

            img3.Save("Gray_TextRemoval.bmp");

        }
        void Gray_ObjectRemoval()
        {
            String file = openFileDialog1.FileName.ToString();
            Image img = Image.FromFile(file);

            img.Save("MaskingImage.bmp");

            img.Tag = "MaskingImage.bmp";

            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            info.FileName = ("mspaint.exe");
            info.Arguments = img.Tag.ToString(); // returns full path and name
            img.Dispose();
            System.Diagnostics.Process p1 = System.Diagnostics.Process.Start(info);

            while (p1.Responding == true) ;
            img1 = new Image<Gray, byte>(openFileDialog1.FileName.ToString());
            img2 = new Image<Gray, byte>("MaskingImage.bmp");




            img3 = new Image<Gray, byte>("MaskingImage.bmp");

            img4 = new Image<Gray, byte>(img1.Size);

            for (int i = 0; i < img3.Rows; i++)
            {
                for (int j = 0; j < img3.Cols; j++)
                {
                    if (!(img1[i, j].Equals(img3[i, j])))
                    {

                        img4.Data[i, j, 0] = (byte)255;
                    }
                }
            }

            img4.Save("Mask.bmp");

            img2 = img4;

            img3 = img1;


            for (int k = 0; k < 100; k++)
            {
                double sum = 0;
                for (int i = 0; i < img3.Rows; i++)
                {
                    for (int j = 0; j < img3.Cols; j++)
                    {
                        if (img2[i, j].Intensity == 255)
                        {
                            double c = 0.125;
                            sum = img3.Data[i, j, 0] * (0) + img3.Data[i - 1, j, 0] * (c) + img3.Data[i, j - 1, 0] * (c) + img3.Data[i + 1, j, 0] * (c) + img3.Data[i, j + 1, 0] * (c) + img3.Data[i - 1, j - 1, 0] * (c) + img3.Data[i - 1, j + 1, 0] * (c) + img3.Data[i + 1, j - 1, 0] * (c) + img3.Data[i + 1, j + 1, 0] * (c);

                            img3.Data[i, j, 0] = (byte)sum;
                        }

                    }
                }
            }
            imageBox2.Image = img3;

            img3.Save("Gray_ObjectRemoval.bmp");
        }


    }
}
