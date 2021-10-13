using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using ExcelLibrary.SpreadSheet;

namespace Folder_Mover
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void search(string path, string check, string destination)
        {
            try
            {
                var regex = new Regex(check);

                foreach (string d in Directory.GetDirectories(path))
                {
                    var dirName = d.Substring(d.LastIndexOf('\\') + 1);
                    if (regex.IsMatch(dirName))
                    {
                        string foundFolder = d.Substring(d.LastIndexOf('\\') + 1);
                        move(d, destination);
                        progressBar1.PerformStep();
                        textBox3.Text += d + "\r\n";
                    }
                    else
                    {
                        search(d, check, destination);
                    }
                }
            }
            catch (Exception e)
            {
                textBox4.Text += e.Message + "\r\n";
                progressBar1.PerformStep();
            }

        }


        public void move(string path, string destination){
            try
            {
                string folderName = path.Substring(path.LastIndexOf('\\') + 1);
                if (Directory.Exists(destination + "\\" + folderName) == false)
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.MoveTo(destination + "\\" + folderName);
                }
                else
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 1000);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    dir.MoveTo(destination + "\\" + folderName+"("+randomNumber+")");
                }
            }
            catch (Exception e)
            {
                textBox4.Text += e.Message + "\r\n";
            }
        }
 

        
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button1.BackColor = System.Drawing.Color.Red;
            string searchPath = textBox1.Text;
            string movePath = textBox2.Text;
            string xlsPath = textBox5.Text;

            
            Workbook book = Workbook.Load(xlsPath);
            Worksheet sheet = book.Worksheets[0];
            int rowCount = sheet.Cells.Rows.Count();
            string[] jobs = new string[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                
                jobs[i] = sheet.Cells[i, 0].StringValue;
                
            }

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            int jobcount = jobs.Count();
            progressBar1.Step = (100 / jobcount);
            foreach (string j in jobs){ 
                string job = j;
                search(searchPath, job, movePath);
            }
            textBox3.Text += "DONE!";
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button1.BackColor = System.Drawing.Color.Green;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Comma Delimited|*.csv";
            ofd.Filter = "Excel File|*.xls";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string filename = "";
                filename = ofd.FileName;
                textBox5.Text = filename;
                progressBar1.Value = 0;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string folderpath = "";
                folderpath = fbd.SelectedPath;
                textBox1.Text = folderpath;
                progressBar1.Value = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string folderpath = "";
                folderpath = fbd.SelectedPath;
                textBox2.Text = folderpath;
                progressBar1.Value = 0;
            }
        }
    }
}
