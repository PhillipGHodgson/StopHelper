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
    public partial class MiniTriggerForm : Form
    {
        private bool isCondition;

        Condition condition;
        Effect effect;

        Label[] labels;
        TextBox[] textBoxes;

        const int NUM_BOXES = 4;

        SceneData data;

        public MiniTriggerForm(Condition c, Effect e, SceneData dat)
        {
            data = dat;
            this.isCondition = e == null;
            InitializeComponent();
            textBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4 };
            labels = new Label[] { label1, label2, label3, label4 };
            if (isCondition)
            {
                condition = c;
                comboBox1.Items.Add(ConditionType.CLICK);
                comboBox1.Items.Add(ConditionType.CLICK_WITH_ITEM);
                comboBox1.Items.Add(ConditionType.CLIP_END);
                comboBox1.Items.Add(ConditionType.INTRO);



                if (c.condition_args != null)
                {
                    comboBox1.SelectedItem = c.type;
                    for (int i = 0; i < NUM_BOXES; i++)
                        textBoxes[i].Text = c.condition_args.Length > i ? c.condition_args[i] : "";
                }
            }
            else
            {
                effect = e;

                comboBox1.Items.Add(EffectType.PLAY_CLIP);
                comboBox1.Items.Add(EffectType.LOAD_SCENE);
                comboBox1.Items.Add(EffectType.START_SCENE);
                comboBox1.Items.Add(EffectType.GET_ITEM);
                comboBox1.Items.Add(EffectType.REMOVE_ITEM);
                comboBox1.Items.Add(EffectType.SPRITE);
                comboBox1.Items.Add(EffectType.SPECIAL);

                if (e.effect_args != null)
                {
                    comboBox1.SelectedItem = e.type;
                    for (int i = 0; i < NUM_BOXES; i++)
                        textBoxes[i].Text = e.effect_args.Length > i ? e.effect_args[i] : "";
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                TrigTypeDescriptor des;
                Object item = comboBox1.SelectedItem;
                if (item is ConditionType)
                    des = TrigTypeDescriptor.getTrigTypeDes((ConditionType)item);
                else
                    des = TrigTypeDescriptor.getTrigTypeDes((EffectType)item);

                useTypeDes(des);
            }
            else
            {

            }
        }

        private void useTypeDes(TrigTypeDescriptor des)
        {
            DescriptionLabel.Text = des.description;


            for (int i = 0; i < NUM_BOXES; i++)
            {
                labels[i].Text = des.args.Length > i ? des.args[i] : "arg" + (i + 1);
                textBoxes[i].Enabled = des.args.Length > i;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> args = new List<string>();
            for(int i = 0; i < NUM_BOXES; i++)
            {
                if(textBoxes[i].Enabled && textBoxes[i].Text != null && textBoxes[i].Text != "")
                    args.Add(textBoxes[i].Text);
                else
                    break;
            }

            if (isCondition)
            {
                condition.type = (ConditionType)comboBox1.SelectedItem;
                condition.condition_args = args.ToArray();
                condition.OnDisplayPropertyChanged();
            }
            else
            {
                effect.type = (EffectType)comboBox1.SelectedItem;
                effect.effect_args = args.ToArray();
                effect.OnDisplayPropertyChanged();
            }

            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
            populate_list(contextMenuStrip1, label1.Text);
        }

        private void populate_list(ContextMenuStrip menu, string labelText)
        {
            menu.Items.Clear();
            if (labelText == "region_clicked_name")
            {
                foreach(RegionData r in data.regions)
                    menu.Items.Add(r.regionName);
            }
            else if (labelText == "clip_name")
            {
                foreach (string s in data.clips)
                {
                    menu.Items.Add(s);
                }
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            textBox1.Text = e.ClickedItem.ToString();
        }

    }


    public class TrigTypeDescriptor
    {
        public string description;
        public string[] args;

        //Conditions
        private static TrigTypeDescriptor click = new TrigTypeDescriptor(
                                                  "triggered by clicking specified region",
                                                  new string[] { "region_clicked_name" });

        private static TrigTypeDescriptor click_WITH_ITEM = new TrigTypeDescriptor(
                                          "triggered by clicking specified region with specified item",
                                          new string[] { "region_clicked_name", "item_name" });

        private static TrigTypeDescriptor clip_end = new TrigTypeDescriptor(
                                          "triggered by the specified movie ending",
                                          new string[] { "clip_name" });

        private static TrigTypeDescriptor intro = new TrigTypeDescriptor(
                                          "triggered by switching from prev_scene to this scene",
                                          new string[] { "prev_scene_name (optional)" });

        //Effects
        private static TrigTypeDescriptor load_scene = new TrigTypeDescriptor(
                                          "starts loading a scene into memory",
                                          new string[] { "scene_name" });

        private static TrigTypeDescriptor start_scene = new TrigTypeDescriptor(
                                          "starts playing the scene loaded into memory, or displays \"loading\" if it isn't loaded yet",
                                          new string[] {});

        private static TrigTypeDescriptor play_clip = new TrigTypeDescriptor(
                                          "starts playing the specified clip",
                                          new string[] { "clip_name" });

        private static TrigTypeDescriptor special = new TrigTypeDescriptor(
                                          "Does something unique",
                                          new string[] { "arg1", "arg2", "arg3", "arg4" });

        private static TrigTypeDescriptor sprite = new TrigTypeDescriptor(
                                          "shows a 2D sprite. repeat_count = 0 for infite loop",
                                          new string[] {"sprite_name", "x=0", "y=0", "repeat_count=1", "delay_seconds=0.00" });

        private static TrigTypeDescriptor get_item = new TrigTypeDescriptor(
                                          "Adds item to inventory",
                                          new string[] { "item_name" });

        private static TrigTypeDescriptor remove_item = new TrigTypeDescriptor(
                                          "Removes item from inventory",
                                          new string[] { "item_name" });



        public static TrigTypeDescriptor getTrigTypeDes(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.CLICK:
                    return click;
                case ConditionType.CLICK_WITH_ITEM:
                    return click_WITH_ITEM;
                case ConditionType.CLIP_END:
                    return clip_end;
                case ConditionType.INTRO:
                    return intro;
                default:
                    return null;
            }
        }
        public static TrigTypeDescriptor getTrigTypeDes(EffectType type)
        {
            switch (type)
            {
                case EffectType.GET_ITEM:
                    return get_item;
                case EffectType.LOAD_SCENE:
                    return load_scene;
                case EffectType.PLAY_CLIP:
                    return play_clip;
                case EffectType.REMOVE_ITEM:
                    return remove_item;
                case EffectType.SPRITE:
                    return sprite;
                case EffectType.START_SCENE:
                    return start_scene;
                default:
                    return null;
            }
        }
        public TrigTypeDescriptor(string description, string[] args)
        {
            this.args = args;
            this.description = description;

        }
    }

}
