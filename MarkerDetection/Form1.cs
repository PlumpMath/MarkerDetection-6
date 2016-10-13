using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace MarkerDetection
{
    public partial class Form1 : Form
    {
        public List<Marker> markers;
        public List<MarkerPlainData> MarkerPlainDatas;
        public Form1()
        {
            InitializeComponent();
            textBox1.Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
            markers = new List<Marker>();
            MarkerPlainDatas = new List<MarkerPlainData>();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            listBox1.Focus();

        }

        public void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|" + "All files (*.*)|*.*";
            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "My Image Browser";
            DialogResult dr = this.openFileDialog1.ShowDialog();
            var name = Microsoft.VisualBasic.Interaction.InputBox("What's the catogery of marker?(name)", "Name", "AprilTags");
            var input = Microsoft.VisualBasic.Interaction.InputBox("What's the size of marker?(how many square blocks)", "Size", "7");
            int size = Int32.Parse(input);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                
                foreach (String file in openFileDialog1.FileNames)
                {
                    var resultString = Regex.Match(file, @"\d+").Value;
                    int id = Int32.Parse(resultString);
                    Bitmap image = new Bitmap(file);
                    var newmarker = new Marker(image, size, id);
                    newmarker.category = name;
                    markers.Add(newmarker);
                    MarkerPlainDatas.Add(newmarker.PlainData);
                    listBox1.Items.Add(newmarker);
                }
                label1.Text = "All markers read!";
            }

        }



        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            pictureBox1.Image = ((Marker)listBox1.SelectedItem).image;
            textBox1.Text = ((Marker)listBox1.SelectedItem).text;
            label1.Text = "MarkerID:  " + ((Marker)listBox1.SelectedItem).ID.ToString();
        }

        public void writetojson()
        {
            
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string outputJSON = "";
            outputJSON = ser.Serialize(MarkerPlainDatas);
            

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "db (*.db)|*.db|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string name = saveFileDialog1.FileName;
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                File.WriteAllText(name, outputJSON);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            writetojson();
            label1.Text = "Marker Database Saved!";
        }
    }
}
