using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundWorkerDemo
{
    public partial class Form1 : Form
    {
        public Form1 ()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (e.Cancelled) {
                MessageBox.Show("Word counting cancelled.");
            }
            else {
                MessageBox.Show("Finished counting words.");
            }
        }

        private void backgroundWorker1_ProgressChanged (object sender, ProgressChangedEventArgs e)
        {
            Words.CurrentState state = (Words.CurrentState)e.UserState;
            LinesCounted.Text = state.LinesCounted.ToString();
            WordsCounted.Text = state.WordsMatched.ToString();
        }

        private void backgroundWorker1_DoWork (object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker;
            worker = sender as BackgroundWorker;

            Words WC = (Words)e.Argument;
            try {
                WC.CountWords(worker, e);
            }
            catch(Exception exc) {
                MessageBox.Show(exc.Message);
            }
        }

        private void StartThread()
        {
            WordsCounted.Text = "0";

            Words WC = new Words();
            WC.CompareString = CompareString.Text;
            WC.SourceFile = SourceFile.Text;

            backgroundWorker1.RunWorkerAsync(WC);
        }

        private void Start_Click (object sender, EventArgs e)
        {
            StartThread();
        }

        private void Cancel_Click (object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}
