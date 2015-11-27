using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; //Advanced Image Functionalities
using System.IO;
using System.Xml;
using System.Configuration;
using IsaacAnimator;
namespace Isaac_Animator
{
    public partial class Animatron : Form
    {
        public Animatron()
        {
            InitializeComponent();
        }
        private Bitmap originalPic;

        private void CreateMyListView()
        {
            // Create a new ListView control.
            ListView listView1 = new ListView();
            listView1.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));

            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.Ascending;

            // Create three items and three sets of subitems for each item.
            ListViewItem item1 = new ListViewItem("item1", 0);
            // Place a check mark next to the item.
            item1.Checked = true;
            item1.SubItems.Add("1");
            item1.SubItems.Add("2");
            item1.SubItems.Add("3");
            ListViewItem item2 = new ListViewItem("item2", 1);
            item2.SubItems.Add("4");
            item2.SubItems.Add("5");
            item2.SubItems.Add("6");
            ListViewItem item3 = new ListViewItem("item3", 0);
            // Place a check mark next to the item.
            item3.Checked = true;
            item3.SubItems.Add("7");
            item3.SubItems.Add("8");
            item3.SubItems.Add("9");

            // Create columns for the items and subitems. 
            // Width of -2 indicates auto-size.
            listView1.Columns.Add("Item Column", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Column 2", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Column 3", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Column 4", -2, HorizontalAlignment.Center);

            //Add the items to the ListView.
            listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });

            // Create two ImageList objects.
            ImageList imageListSmall = new ImageList();
            ImageList imageListLarge = new ImageList();

            /* Initialize the ImageList objects with bitmaps.
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\MySmallImage1.bmp"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\MySmallImage2.bmp"));
            imageListLarge.Images.Add(Bitmap.FromFile("C:\\MyLargeImage1.bmp"));
            imageListLarge.Images.Add(Bitmap.FromFile("C:\\MyLargeImage2.bmp"));*/

            //Assign the ImageList objects to the ListView.
            listView1.LargeImageList = imageListLarge;
            listView1.SmallImageList = imageListSmall;

            // Add the ListView to the control collection. 
            this.Controls.Add(listView1);
        }
        //create Coloring object for frame edits
        private Coloring _colorMatrix = new Coloring();
        //Read XML File
        private void XmlReadFile()
        {
            string location = ConfigurationManager.AppSettings["ANM2Location"];
            if (!File.Exists(Path.ChangeExtension(location, ".xml")))
            {
                File.Copy(location, Path.ChangeExtension(location, ".xml"));
            }
            var result = Path.ChangeExtension(location, ".xml");

            XmlReader reader = XmlReader.Create(result);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                    switch (reader.Name)
                    {
                        case "Info":
                            AuthorTextBox.Text = reader.GetAttribute("CreatedBy");
                            VersionTextBox.Text = reader.GetAttribute("Version");
                            FPSTextBox.Text = reader.GetAttribute("Fps");
                            break;
                        case "Animation":
                            FunctionDropdown.Items.Add(reader.GetAttribute("Name"));
                            break;
                        case "Spritesheet":
                            ListViewItem item = new ListViewItem(reader.GetAttribute("Id"));
                            item.SubItems.Add(reader.GetAttribute("Path"));
                            listViewSpritesheets.Items.Add(item);
                            break;
                    }
            }
            string image = Path.GetDirectoryName(location)+ "/" + listViewSpritesheets.Items[0].SubItems[1].Text;
            pictureBox1.Load(image);
            originalPic = new Bitmap(pictureBox1.Image);
        }
        //Set Frame Tint ------------------- Coloring
        private void frameTintButton_Click(object sender, EventArgs e)
        {
            if (colorDialogTint.ShowDialog() == DialogResult.OK) //open dialog
            {
                pictureBox1.Image = _colorMatrix.Tint(originalPic, colorDialogTint.Color); //Apply Tint
                frameTintButton.BackColor = colorDialogTint.Color;
            }
        }
        //Set Width
        private void frameWidth_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Padding = new Padding(pictureBox1.Padding.Left, pictureBox1.Padding.Top, (int)frameWidth.Value, pictureBox1.Padding.Bottom);
        }
        //Set Height
        private void frameHeight_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Padding = new Padding(pictureBox1.Padding.Left, pictureBox1.Padding.Top, pictureBox1.Padding.Right, (int)frameHeight.Value);
        }
        //Load Anm2 FIle
        private void loadamn2FileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenFileDialogLoadANM2.ShowDialog() == DialogResult.OK) //open dialog
            {
                setConfig("ANM2Location", OpenFileDialogLoadANM2.FileName);
                XmlReadFile();
            }

        }
        //Set Frame Offset ----------------- Coloring
        private void frameOffset_Click(object sender, EventArgs e)
        {
            if (colorDialogOffset.ShowDialog() == DialogResult.OK) //open dialog
            {
                // funktioniert
                pictureBox1.Image = _colorMatrix.Offset(originalPic, colorDialogOffset.Color); //Apply Tint
                frameOffset.BackColor = colorDialogOffset.Color;
            }
        }
        //Load Image
        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogLoadImage.ShowDialog() == DialogResult.OK) //open dialog
            {
                pictureBox1.Load(openFileDialogLoadImage.FileName);
                originalPic = new Bitmap(pictureBox1.Image);
            }
        }
        //Set Frame transparency------------ Coloring
        private void trackBarTransparency_Scroll(object sender, EventArgs e)
        {
            numericUpDownTransparency.Value = trackBarTransparency.Value;
            pictureBox1.Image = _colorMatrix.setTransparent(originalPic, trackBarTransparency.Value); //Apply Tint
        }

        private void numericUpDownTransparency_ValueChanged(object sender, EventArgs e)
        {
            trackBarTransparency.Value = (int)numericUpDownTransparency.Value;
        }
        //Reset Coloring settings ---------- Coloring
        private void ButtonColorReset_Click(object sender, EventArgs e)
        {
            numericUpDownTransparency.Value = 255; 
            frameOffset.BackColor = Color.Transparent;
            frameTintButton.BackColor = Color.Transparent;
            pictureBox1.Image = originalPic;
            _colorMatrix.Reset();
        }
        //****
        //TODO: Select stylesheetsource
        //****
        private void selectStylesheetSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                
            }
        }
        //****
        //TODO: config file generation for permanent saving 
        //****
        private void setConfig(string setting, string value) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[setting].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        //****
        //no idea what that is
        //****
        private void FunctionDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            string path = ConfigurationManager.AppSettings["ANM2Location"];
            doc.LoadXml(path);

            XmlNode root = doc.DocumentElement;

            //Create a new title element.
            XmlElement elem = doc.CreateElement("title");
            elem.InnerText = "The Handmaid's Tale";

            //Replace the title element.
            root.ReplaceChild(elem, root.FirstChild);

            Console.WriteLine("Display the modified XML...");
            doc.Save(Console.Out);
        }
        //Change scaling x
        private void FrameScaleX_ValueChanged(object sender, EventArgs e)
        {
            if (FrameScaleCheck.Checked) {
                FrameScaleY.Value = FrameScaleX.Value;
            }
            pictureBox1.Image = _colorMatrix.Resize((Bitmap)originalPic, (int)(FrameScaleX.Value), (int)(FrameScaleY.Value));
        }
        //Change scaling y
        private void FrameScaleY_ValueChanged(object sender, EventArgs e)
        {
            if (FrameScaleCheck.Checked)
            {
                FrameScaleX.Value = FrameScaleY.Value;
            }
            pictureBox1.Image = _colorMatrix.Resize((Bitmap)originalPic, (int)(FrameScaleX.Value), (int)(FrameScaleY.Value));
        }
        //About window
        private void somethingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Software by Wofsauge \n\n  www.wofsauge.com \n\n Developed 2015 ", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //****
        //TODO: Change Used Spritesheet
        //****
        private void listViewSpritesheets_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
