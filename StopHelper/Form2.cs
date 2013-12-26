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
    partial class Form2 : Form
    {
        public static bool sucessful = false;
        public static RegionData data;
        public Form2(RegionData data)
        {
            InitializeComponent();
            sucessful = false;
            Form2.data = data;
            comboBox1.Items.Add(MouseLook.ACTION);
            comboBox1.Items.Add(MouseLook.DEFAULT_POINTER);
            comboBox1.Items.Add(MouseLook.DOWN_ARROW);
            comboBox1.Items.Add(MouseLook.DOWNLEFT_ARROW);
            comboBox1.Items.Add(MouseLook.DOWNRIGHT_ARROW);
            comboBox1.Items.Add(MouseLook.LEFT_ARROW);
            comboBox1.Items.Add(MouseLook.RIGHT_ARROW);
            comboBox1.Items.Add(MouseLook.UP_ARROW);
            comboBox1.Items.Add(MouseLook.UPLEFT_ARROW);
            comboBox1.Items.Add(MouseLook.UPRIGHT_ARROW);

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.look = (MouseLook)comboBox1.SelectedItem;
            data.regionName = textBox1.Text;
            sucessful = true;
            this.Close();
        }
    }
}
