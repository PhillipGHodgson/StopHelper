using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace StopHelper
{
    public partial class Form1 : Form
    {
        int x, y;
        bool dragging = false;
        Graphics g;

        List<RegionData> dataPoints = new List<RegionData>();

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent("Bitmap"))
            {
                object o = Clipboard.GetDataObject().GetData("Bitmap");
                if (o != null)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    pictureBox1.Image = (Image)o;
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            dragging = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                g.DrawRectangle(new Pen(Color.Red), x, y, e.X - x, e.Y - y);
            }
            dragging = false;
            int width = e.X - x;
            int height = e.Y - y;
            if (width < 0)
            {
                width *= -1;
                x = e.X;
            }
            if (height < 0)
            {
                height *= -1;
                y = e.Y;
            }
            RegionData regionData = new RegionData();
            regionData.xPercent = ((double)x) / pictureBox1.Width;
            regionData.yPercent = ((double)y) / pictureBox1.Height;
            regionData.widthPercent = ((double)width) / pictureBox1.Width;
            regionData.heightPercent = ((double)height) / pictureBox1.Height;

            Form2 box = new Form2(regionData);
            box.ShowDialog();
            regionData = Form2.data;
            if (Form2.sucessful)
            {
                dataPoints.Add(regionData);
            }

        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SceneData data = new SceneData();
            data.regions = dataPoints.ToArray();
            data.clips = new string[] { "Movie_0010", "Movie_0006", "Movie_0009", "Movie_0008" };

            XmlSerializer serializer =
     new XmlSerializer(typeof(SceneData));
            SaveFileDialog d = new SaveFileDialog();
            d.ShowDialog();
            TextWriter writer = new StreamWriter(d.FileName);
            serializer.Serialize(writer, data);
            writer.Close();
        }
    }
}
