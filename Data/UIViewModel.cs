using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScantronRevived.Data
{
    /// <summary>
    /// Stores the information that would be used when interacting with user input
    /// </summary>
    public class UIViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler for property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Calls property changes from outside of this class
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// The grader for the program
        /// </summary>
        public Grader Grader { get; init; }

        public Dictionary<int, List<Question>> AnswerKey
        {
            get
            {
                return Grader.AnswerKey;
            }
            set
            {
                Grader.AnswerKey = value;
            }
        }

        /// <summary>
        /// The scanner class for the program
        /// </summary>
        public Scanner Scanner { get; init; }

        // Holds the raw card data from the Scantron.
        public List<string> raw_cards = new List<string>();

        /// <summary>
        /// private backing field for IsGrading property
        /// </summary>
        private bool _isGrading = false;
        /// <summary>
        /// Stores if the user is grading with the program
        /// </summary>
        public bool IsGrading
        {
            get
            {
                return _isGrading;
            }
            set
            {
                _isGrading = value;
            }
        }

        /// <summary>
        /// private backing field for the NumberOfVersions property
        /// </summary>
        private int _numberOfVersions = 3;

        /// <summary>
        /// Stores the number of versions specified by the user
        /// </summary>
        public int NumberOfVersions
        {
            get
            {
                return _numberOfVersions;
            }
            set
            {
                _numberOfVersions = value;
            }
        }

        /// <summary>
        /// private backing field for the NumberOfQuestions property
        /// </summary>
        private int _numberOfQuestions = 40;

        /// <summary>
        /// Stores the number of questions per sheet specified by the user
        /// </summary>
        public int NumberOfQuestions
        {
            get
            {
                return _numberOfQuestions;
            }
            set
            {
                _numberOfQuestions = value;
            }
        }

        /// <summary>
        /// Private backing field for the ExamName property
        /// </summary>
        private string _examName = "Exam";

        /// <summary>
        /// Stores the name of the exam specified by the user, defaults to "Exam"
        /// </summary>
        public string ExamName
        {
            get
            {
                return _examName;
            }
            set
            {
                _examName = value;
            }
        }

        /// <summary>
        /// private backing field for the output string
        /// </summary>
        private string _output = "";

        
        /// <summary>
        /// Stores the output string to be written when the user is finished
        /// </summary>
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }

        public UIViewModel()
        {
            Scanner = new Scanner(this);
            Grader = new Grader(this);

            //for implementing visuals on grading tab.  delete later
            /*
            Grader.AnswerKey.Add(0, new List<Question>());
            Grader.AnswerKey.Add(1, new List<Question>());
            Grader.AnswerKey.Add(2, new List<Question>());
            for(int i = 0; i < 40; i++)
            {
                Grader.AnswerKey[0].Add(new Question("", 1, false));
                Grader.AnswerKey[1].Add(new Question("", 1, false));
                Grader.AnswerKey[2].Add(new Question("", 1, false));
            }
            */
        }

        public void ViewModelBeginScanning()
        {
            try
            {
                Scanner.Scan();
            }
            catch (Exception)
            {
                MessageBox.Show("The scantron machine is not connected properly. Please notify the IT Service Desk to your right and around the corner, and we will come fix it.");
            }
            
        }

        /// <summary>
        /// Close the serial port and update the card list.
        /// </summary>
        public void ViewModelDoneScanning()
        {
            Scanner.StopScanning();
            Scanner.Stop(false);
            try
            {
                raw_cards = Scanner.raw_cards;
                Grader.CreateCards(raw_cards, false);
                //Automatically find number of questions and versions
                
                int maxV = 0;
                List<int> maxQ = new List<int>();
                //It remains to be seen if the response.count is actually the number of questions input by the student on the card. will need to test this when i am in front of the machine.
                foreach(Card c in Grader.Cards)
                {
                    maxQ.Add(c.Response.Count);
                }
                NumberOfQuestions = (int)maxQ.Average();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, please notify the IT Service Desk to your right and around the corner.");
            }
        }



        /// <summary>
        /// Write the file to be uploaded to the Canvas gradebook.
        /// </summary>
        public void WriteFile(string type)
        {
            string file = "";
            string filter = "txt files (*.txt)|*.txt";

            if (type == "gradebook")
            {
                file = Grader.GradebookFile();
                filter = "csv files (*.csv)|*.csv";
            }
            if (type == "single")
            {
                file = Grader.SingleAnswerFile(NumberOfQuestions);
            }
            if (type == "multiple")
            {
                file = Grader.MultipleAnswerFile();
            }

            // Then we have to start a file dialog to save the string to a file.
            SaveFileDialog uxSaveFileDialog = new SaveFileDialog
            {
                // the name given by the user at the beginning.
                FileName = ExamName,

                // Could be used to select the default directory ex. "C:\Users\Public\Desktop".
                InitialDirectory = "c:\\desktop",
                // Filter is the default file extensions seen by the user.
                Filter = filter,
                // FilterIndex sets what the user initially sees ex: 2nd index of the filter is ".txt".
                FilterIndex = 1
            };

            if (uxSaveFileDialog.ShowDialog() == true)
            {
                string path = uxSaveFileDialog.FileName;
                // Stores the location of the file we want to save; use filenames for multiple.
                if (path.Equals(""))
                {
                    MessageBox.Show("You must enter a filename and select\n" +
                                    "a file path for the exam record!");
                    throw new IOException();
                }
                else
                {
                    // "using" opens and close the StreamWriter.
                    using (StreamWriter file_generator = new StreamWriter(path))
                    {
                        // Adds everything in the 'file' given to the streamwriter.
                        file_generator.Write(file);
                    }
                    MessageBox.Show("Student responses have been successfully recorded!");
                }
            }
            
        }

        public void InstantiateAnswerKeys()
        {
            for(int i = 0; i < NumberOfVersions; i++)
            {
                Grader.AnswerKey.Add(i, new List<Question>());
                for(int j = 0; j < NumberOfQuestions; j++)
                {
                    Grader.AnswerKey[i].Add(new Question("", 1, false));
                }
            }
            OnPropertyChanged(nameof(AnswerKey));
            Grader.OnCollectionChanged();
        }

        /// <summary>
        /// Close serial port and fill out the answer key form based on scanned cards.
        /// </summary>
        public void DoneScanningKeys()
        {
            Scanner.StopScanning();
            Scanner.Stop(true);
            try
            {
                raw_cards = Scanner.raw_keys;
                Grader.CreateCards(raw_cards, true);


                for(int i = 0; i < Grader.KeyCards.Count; i++)
                {
                    Card card = Grader.KeyCards[i];
                    int v = card.TestVersion-1;
                    for (int j = 0; j < card.Response.Count; j++)
                    {
                        if (card.Response[j].Answer[0] > 60)
                        {
                            AnswerKey[v][j].Input0 = true;
                        }
                        if (card.Response[j].Answer[1] > 60)
                        {
                            AnswerKey[v][j].Input1 = true;
                        }
                        if (card.Response[j].Answer[2] > 60)
                        {
                            AnswerKey[v][j].Input2 = true;
                        }
                        if (card.Response[j].Answer[3] > 60)
                        {
                            AnswerKey[v][j].Input3 = true;
                        }
                        if (card.Response[j].Answer[4] > 60)
                        {
                            AnswerKey[v][j].Input4 = true;
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                //UpdateScanButtonColor((Button)uxAnswerKeyTabPage.Controls.Find("uxDoneScanningButton", true)[0], Color.Red);
                MessageBox.Show("Something went wrong. Do not click Done Scanning while the machine is running. " +
                                "Stop the machine, click Scan Answer Key, and begin scanning again.");
            }
        }
    }
}
