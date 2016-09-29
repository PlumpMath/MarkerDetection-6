using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using System.Web.Script.Serialization;


namespace DataBaseMaker
{
    public partial class Form1 : Form
    {
        public List<Marker> markers;
        public List<MarkerPlainData> MarkerPlainDatas;
        public Form1()
        {
            InitializeComponent();
            //Marker _marker = new Marker(new Bitmap(img), 7, 1);
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
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                
                foreach (String file in openFileDialog1.FileNames)
                {
                    var resultString = Regex.Match(file, @"\d+").Value;
                    int id = Int32.Parse(resultString);
                    Bitmap image = new Bitmap(file);
                    var newmarker = new Marker(image, 7, id);
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
            File.WriteAllText("output.db", outputJSON);

            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            writetojson();
            label1.Text = "Marker Database Saved!";
        }
    }
}
