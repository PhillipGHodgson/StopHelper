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
    public partial class TriggerForm : Form
    {
        SceneData data;
        BindingList<TriggerData> trigs;

        BindingList<Condition> currConditions;
        BindingList<Effect> currEffects;
        TriggerData currData;
        public TriggerForm(SceneData data)
        {
            this.data = data;
            InitializeComponent();
            trigs = new BindingList<TriggerData>();
            if (data.triggers != null)
                foreach (TriggerData t in data.triggers)
                    trigs.Add(t);
            listBox1.DataSource = trigs;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = listBox1.SelectedIndex != -1;
            button2.Enabled = groupBox1.Enabled;

            if (groupBox1.Enabled)
            {

                currConditions = new BindingList<Condition>();
                currEffects = new BindingList<Effect>();
                currData = (TriggerData)listBox1.SelectedItem;
                if (currData.conditions != null)
                    foreach (Condition c in currData.conditions)
                        currConditions.Add(c);
                if (currData.effects != null)
                    foreach (Effect eff in currData.effects)
                        currEffects.Add(eff);
                listBox2.DataSource = currConditions;
                listBox3.DataSource = currEffects;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trigs.Add(new TriggerData());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trigs.Remove((TriggerData)listBox1.SelectedItem);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data.triggers = trigs.ToArray();
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            editConditionToolStripMenuItem.Enabled = listBox2.SelectedIndex != -1;
            deleteToolStripMenuItem.Enabled = listBox2.SelectedIndex != -1;
        }

        private void effectMenu_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItem2.Enabled = listBox3.SelectedIndex != -1;
            toolStripMenuItem3.Enabled = listBox3.SelectedIndex != -1;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Effect effect = new Effect();
            new MiniTriggerForm(null, effect, data).ShowDialog();
            currEffects.Add(effect);
            currData.effects = currEffects.ToArray();
        }

        private void newConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Condition condition = new Condition();
            new MiniTriggerForm(condition, null, data).ShowDialog();
            currConditions.Add(condition);
            currData.conditions = currConditions.ToArray();
        }

        private void editConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MiniTriggerForm((Condition)listBox2.SelectedItem, null, data).ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new MiniTriggerForm(null, (Effect)listBox3.SelectedItem, null).ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            currEffects.Remove((Effect)listBox3.SelectedItem);
            currData.effects = currEffects.ToArray();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currConditions.Remove((Condition)listBox2.SelectedItem);
            currData.conditions = currConditions.ToArray();
        }
    }
}
