using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StopHelper
{
    public partial class ClipNamesForm : Form
    {
        SceneData data;
        BindingList<String> clips;
        public ClipNamesForm(SceneData data)
        {
            this.data = data;
            InitializeComponent();
            clips = new BindingList<string>();
            if (data.clips != null)
                foreach (string s in data.clips)
                    clips.Add(s);
            listBox1.DataSource = clips;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clips.Add(textBox1.Text);
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clips.Remove((string)listBox1.SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listBox1.SelectedIndex != -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data.clips = clips.ToArray();
            this.Close();
        }

    }
}
