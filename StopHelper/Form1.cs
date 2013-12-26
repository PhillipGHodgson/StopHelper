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
        int x1, y1;
        int x2, y2;
        bool dragging = false;
        bool waiting = true;
        Graphics g;
        Image image;

        List<RegionData> dataPoints = new List<RegionData>();

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = e.X;
            y1 = e.Y;
            dragging = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            waiting = true;
            dragging = false;
            
            int width = x2 - x1;
            int height = y2 - y1;
            if (width < 0)
            {
                width *= -1;
                x1 = x2;
            }
            if (height < 0)
            {
                height *= -1;
                y1 = y2;
            }
            RegionData regionData = new RegionData();
            regionData.xPercent = ((double)x1) / pictureBox1.Width;
            regionData.yPercent = ((double)y1) / pictureBox1.Height;
            regionData.widthPercent = ((double)width) / pictureBox1.Width;
            regionData.heightPercent = ((double)height) / pictureBox1.Height;

            Form2 box = new Form2(regionData);
            box.ShowDialog();
            waiting = false;
            regionData = Form2.data;
            if (Form2.sucessful)
            {
                dataPoints.Add(regionData);
                listBox1.Items.Add(regionData);
            }

        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                x2 = e.X;
                y2 = e.Y;
            }
            pictureBox1.Refresh();
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Image buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g1 = Graphics.FromImage(buffer);
            if (image != null)
                g1.DrawImage(image, 0, 0);
            if (dragging || waiting)
            {
                g1.DrawRectangle(new Pen(Color.Red), x1, y1, x2 - x1, y2 - y1);
            }
            foreach (RegionData r in dataPoints)
            {
                Pen p;
                if (r.Equals(listBox1.SelectedItem))
                    p = new Pen(Color.Blue);
                else
                    p = new Pen(Color.Green);
                g1.DrawRectangle(p, (int)(r.xPercent * pictureBox1.Width),
                                                       (int)(r.yPercent * pictureBox1.Height),
                                                       (int)(r.widthPercent * pictureBox1.Width),
                                                       (int)(r.heightPercent * pictureBox1.Height));
            }
            pictureBox1.Image = buffer;
        }

        void save()
        {
            SceneData data = new SceneData();
            data.regions = dataPoints.ToArray();
            data.clips = new string[] { "Movie_0010", "Movie_0006", "Movie_0009", "Movie_0008" };

            XmlSerializer serializer =
     new XmlSerializer(typeof(SceneData));
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                TextWriter writer = new StreamWriter(d.FileName);
                serializer.Serialize(writer, data);
                writer.Close();
            }
        }
        void open()
        {

        }
        void paste()
        {
            //program likes to hang here
            if (Clipboard.GetDataObject().GetDataPresent("Bitmap"))
            {
                object o = Clipboard.GetDataObject().GetData("Bitmap");
                if (o != null)
                {
                    // pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                    // pictureBox1.Image = (Image)o;
                    image = (Image)o;
                }
                else { MessageBox.Show("Failed to paste! No image"); }
            }
            else { MessageBox.Show("Failed to paste!"); }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paste();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                editToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataPoints.Remove((RegionData)listBox1.SelectedItem);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);

        }
    }
}
