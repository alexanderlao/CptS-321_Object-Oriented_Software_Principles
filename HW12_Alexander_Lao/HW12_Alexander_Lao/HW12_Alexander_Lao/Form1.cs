// Alexander Lao
// 11481444
// 4/14/2017
// CptS 321 - HW12 Threading

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Diagnostics;

namespace HW12_Alexander_Lao
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // the user clicked on the download string from URL button
        private void downloadButton_Click(object sender, EventArgs e)
        {
            // if there's no URL in the text box, do nothing
            if (urlTextBox.Text == "") return;

            // disable the appropriate interface components
            downloadButton.Enabled = false;
            urlTextBox.Enabled = false;
            resultTextBox.Enabled = false;

            // instantiate a new web client for downloading
            // and a byte array to hold the data
            WebClient webClient = new WebClient();
            byte[] dataBuffer;

            // instantiate a new thread with the download function
            Thread thread = new Thread(() =>
            {
                // download the data based on the URL in the URL text box
                dataBuffer = webClient.DownloadData(urlTextBox.Text);

                // convert the result of the download to a string
                String result = Encoding.ASCII.GetString(dataBuffer);

                // display the downloaded data in the text box
                this.Invoke(new Action(() =>
                {
                    resultTextBox.Text = result;
                }));
            });

            // start the thread
            thread.Start();

            // re-enable the interface components
            downloadButton.Enabled = true;
            urlTextBox.Enabled = true;
            resultTextBox.Enabled = true;
        }

        // the user clicked on the sorting button
        private void sortingButton_Click(object sender, EventArgs e)
        {
            // disable the sorting button
            sortingButton.Enabled = false;

            // instantiate a stopwatch for timing
            Stopwatch stopwatch = new Stopwatch();

            // generate the eight lists
            List<int>[] eightLists = GenerateLists();

            // instantiate a single thread for sorting the lists
            Thread singleThread = new Thread(() =>
            {
                // do the single thread's work
                try
                {
                    // sort all eight lists
                    for (var i = 0; i < 8; i++)
                    {
                        eightLists[i].Sort();
                    }

                    // stop the timer when done
                    // and calculate the elapsed time
                    stopwatch.Stop();
                    long elapsedTime = stopwatch.ElapsedMilliseconds;

                    // display the single thread time data
                    this.Invoke(new Action(() =>
                    {
                        singleTimeLabel.Text = "Single-threaded time: " + elapsedTime.ToString();
                    }));
                }
                finally
                {
                    // reset the timer
                    stopwatch.Reset();

                    // generate new random lists
                    eightLists = GenerateLists();

                    // instantiate an array of eight threads
                    Thread[] eightThreads = new Thread[8];

                    for (var j = 0; j < 8; j++)
                    {
                        // prevent the threads from accessing the same j variable
                        var temp = j;

                        eightThreads[temp] = new Thread(() =>
                        {
                            // give each thread a list to sort
                            eightLists[temp].Sort();
                        });
                    }

                    // start the timer
                    stopwatch.Start();

                    // start each thread
                    for (var k = 0; k < 8; k++)
                    {
                        eightThreads[k].Start();
                    }

                    // wait until all threads complete?
                    // http://stackoverflow.com/questions/5290006/when-all-threads-are-complete
                    foreach (var thread in eightThreads)
                    {
                        thread.Join();
                    }

                    // display the multi thread time data
                    this.Invoke(new Action(() =>
                    {
                        // stop the timer when done
                        // and calculate the elapsed time
                        stopwatch.Stop();
                        long elapsedTime = stopwatch.ElapsedMilliseconds;
                        multiTimeLabel.Text = "Multi-threaded time: " + elapsedTime.ToString();
                    }));
                }
            });

            // start the timer and thread
            stopwatch.Start();
            singleThread.Start();

            // re-enable the sorting button
            sortingButton.Enabled = true;
        }

        // generates and returns an array of eight lists each with 1,000,000 items
        private List<int>[] GenerateLists()
        {
            // array to hold the eight lists
            List<int>[] masterList = new List<int>[8];

            Random random = new Random();

            // create eight lists
            for (var i = 0; i < 8; i++)
            {
                // instantiate the new list
                masterList[i] = new List<int>();

                // populate the list with 1,000,000 random numbers
                for (var j = 0; j < 1000000; j++)
                {
                    masterList[i].Add(random.Next());
                }
            }

            return masterList;
        }
    }
}