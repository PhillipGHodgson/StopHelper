﻿using System;
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

            if (data.regionName != null)
            {
                textBox1.Text = data.regionName;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (comboBox1.Items[i].Equals(data.look))
                    {
                        comboBox1.SelectedIndex = i;
                        return;
                    }
                }
            }
            else
                comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data.look = (MouseLook)comboBox1.SelectedItem;
            data.regionName = textBox1.Text;
            sucessful = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sucessful = false;
            this.Close();
        }
    }
}
