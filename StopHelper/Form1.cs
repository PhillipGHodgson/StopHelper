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
        Graphics g, g1;
        Image image, buffer;
        SceneData data = new SceneData();

        BindingList<RegionData> dataPoints = new BindingList<RegionData>();

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = Graphics.FromImage(buffer);
            listBox1.DataSource = dataPoints;
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
             //   listBox1.Items.Add(regionData);
                data.regions = dataPoints.ToArray();
            }

        }

        

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                x2 = e.X;
                y2 = e.Y;
            }
           // pictureBox1.Refresh();
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!checkBox1.Checked)
                return;
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
            
            //data.regions = dataPoints.ToArray();
            //data.clips = new string[] { "Movie_0010", "Movie_0006", "Movie_0009", "Movie_0008" };


            XmlSerializer serializer =
     new XmlSerializer(typeof(SceneData));
            SaveFileDialog d = new SaveFileDialog();
          //  d.CheckFileExists = true;
            if (d.ShowDialog() == DialogResult.OK)
            {
                TextWriter writer = new StreamWriter(d.FileName);
                serializer.Serialize(writer, data);
                writer.Close();
            }
        }
        void open()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 try
                {

                    System.IO.StreamReader sreader = new System.IO.StreamReader(openFile.FileName);
                    // use StreamReader.ReadLine or other methods to read the file data

                    XmlSerializer s = new XmlSerializer(typeof(SceneData));
                    SceneData data = (SceneData)s.Deserialize(sreader);

                    sreader.Close();

                    this.data = data;
                    dataPoints = new BindingList<RegionData>();
                    if (data.regions != null)
                    {
                        foreach (RegionData d in data.regions)
                            dataPoints.Add(d);
                    }
                 }
                 catch (System.IO.FileNotFoundException)
                 {
                     //return null;
                 }
            }
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
             editToolStripMenuItem.Enabled = (listBox1.SelectedItem != null);
             deleteToolStripMenuItem.Enabled = (listBox1.SelectedItem != null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegionData data = (RegionData)listBox1.SelectedItem;
            Form2 editForm = new Form2(data);
            editForm.ShowDialog();
            if (Form2.sucessful)
            {
               // data.look = Form2.data.look;
               // data.regionName = Form2.data.regionName;
             //   ((RegionData)listBox1.SelectedItem).look = Form2.data.look;
                //listBox1.Refresh();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataPoints.Remove((RegionData)listBox1.SelectedItem);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            data.regions = dataPoints.ToArray();

        }

        private void clipNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClipNamesForm clipForm = new ClipNamesForm(data);
            clipForm.ShowDialog();
        }

        private void triggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriggerForm triggerForm = new TriggerForm(data);
            triggerForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

    }
}
