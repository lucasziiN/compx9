using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Practical
{
    public partial class Form1 : Form
    {
        //Filter for csv files and all files
        const string FILTER = "CSV files|*.csv|All Files|*.*";

        //Maximum number of student data
        const int MAX_STUDENTS = 100;

        //Width of each bar
        const int BAR_WIDTH = 7;

        //An array to store all id numbers
        string[] idArray = new string[MAX_STUDENTS];

        //An array to store all the exam marks
        int[] markArray = new int[MAX_STUDENTS];

        //The number of students read from the file
        int numStudents = 0;

        
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Will load a csv file of marks and store the data in appropriate arrays.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadMarkFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private int CalculateBarHeight(int mark)
        {
            int barHeight = pictureBoxDisplay.Height*mark/100;
            return barHeight;
        }

        /// <summary>
        /// This function calculates the letter grade based on the given mark. 
        /// The input parameter is an integer value representing the mark.
        /// The function returns a string representing the corresponding letter grade.
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        private string CalcLetterGrade(int mark)
        {
            
            if (mark>=80 && mark<=100)
            {
                return "A";
            }
            else if (mark>=65 && mark <=79)
            {
                return "B";
            }
            else if (mark>=50 && mark <=64)
            {
                return "C";
            }
            else if (mark>=35 && mark <=49)
            {
                return "D";
            }
            else if (mark>=0 && mark <=34)
            {
                return "E";
            }
            else
            {
                return "Error";
            }
        }


        private void loadMarkFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            StreamReader reader;
            string line = "";
            string[] csvArray;
            string id = "";
            int mark = 0;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //Set filter for dialog control
            openFileDialog1.Filter = FILTER;
            //Check if the user has selected a file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the selected file
                reader = File.OpenText(openFileDialog1.FileName);

                listBoxData.Items.Add("ID".PadRight(10) + "Mark");
                //Repeat while it is not the end of the file
                while (!reader.EndOfStream)
                {
                    try
                    {
                        //Read a line from the file
                        line = reader.ReadLine();
                        //Split the line using an array
                        csvArray = line.Split(',');
                        //Check if the array has the correct number of elements
                        if (csvArray.Length == 2)
                        {
                            //Extract the values into separate variables
                            id = csvArray[0];
                            mark = int.Parse(csvArray[1]);
                            //Display the data in the listbox
                            listBoxData.Items.Add(id.PadRight(10) + mark);
                            //Store the values into the array using numStudents
                            //for the index position
                            idArray[numStudents] = id;
                            markArray[numStudents] = mark;
                            //Increase the number of students
                            numStudents++;
                        }
                        else
                        {
                            Console.WriteLine("Error: " + line);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error: " + line);
                    }
                }
                //Close the file

                reader.Close();
            }
        }


        /// <summary>
        /// Clears PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBoxDisplay.Refresh();
        }


        /// <summary>
        /// Exits application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// This event handler is triggered when the "graphMarksToolStripMenuItem" menu item is clicked.
        /// It creates a graph of marks using the Graphics class and displays it in the pictureBoxDisplay control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graphMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a Graphics object for drawing on the pictureBoxDisplay.
            Graphics paper = pictureBoxDisplay.CreateGraphics();

            // Creates a pen and a brush to draw bars
            Pen pen1 = new Pen(Color.Black);
            SolidBrush br1 = new SolidBrush(Color.Gray);

            // The starting x and y coordinates for drawing the bars.
            int x = 0;
            int y = 0;

            for (int i = 0; i<markArray.Length; i++)
            {
                // Draw and fill a rectangle representing each mark. The height of the rectangle is determined by the CalculateBarHeight function.
                paper.FillRectangle(br1, x, (pictureBoxDisplay.Height - CalculateBarHeight(markArray[i])), BAR_WIDTH, CalculateBarHeight(markArray[i]));
                paper.DrawRectangle(pen1, x, (pictureBoxDisplay.Height - CalculateBarHeight(markArray[i])), BAR_WIDTH, CalculateBarHeight(markArray[i]));

                // Move the x-coordinate to the right for the next bar.
                x += BAR_WIDTH;
            }
        }


        /// <summary>
        /// This event handler is triggered when the "generateReportToolStripMenuItem" menu item is clicked.
        /// It generates a report and saves it to a file using a StreamWriter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creates writer and file dialog
            StreamWriter writer;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // Set the filter for the save file dialog.
            saveFileDialog1.Filter = FILTER;

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Create a new StreamWriter using the selected file.
                writer = File.CreateText(saveFileDialog1.FileName);

                // Write the header line of the report.
                writer.WriteLine("ID number".PadRight(15) + "Mark".PadRight(15) + "Letter Grade");

                for (int i = 0; i<markArray.Length;i++)
                {
                    // Write a line for each mark, including the ID number, mark, and corresponding letter grade.
                    writer.WriteLine(idArray[i].PadRight(15) + markArray[i].ToString().PadRight(15) + CalcLetterGrade(markArray[i]));
                }
                // Close the StreamWriter to save the file.
                writer.Close();
            }
        }
    }
}
