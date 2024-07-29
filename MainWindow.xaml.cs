using ScantronRevived.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScantronRevived
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The viewmodel object that handles all of the changing data for the program
        /// </summary>
        UIViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            ///Gives the Main window the ability to see an instance of the UIViewModel class which communicates with the data classes 
            viewModel = new UIViewModel();
            DataContext = viewModel;
        }

        #region Start Page

        /// <summary>
        /// Handles the input change for the NumberOfVersionsUpButton
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void NumberOfVersionsUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.NumberOfVersions <= 2)
            {
                viewModel.NumberOfVersions++;
                viewModel.OnPropertyChanged(nameof(viewModel.NumberOfVersions));
            }
        }

        /// <summary>
        /// Handles the input change for the NumberOfVersionsDownButton
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void NumberOfVersionsDownButton_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.NumberOfVersions >= 2)
            {
                viewModel.NumberOfVersions--;
                viewModel.OnPropertyChanged(nameof(viewModel.NumberOfVersions));
            }
            
        }

        /// <summary>
        /// Handles the input change for the NumberOfQuestionsUpButton
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void NumberOfQuestionsUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.NumberOfQuestions <= 49)
            {
                viewModel.NumberOfQuestions++;
                viewModel.OnPropertyChanged(nameof(viewModel.NumberOfQuestions));
            }
            
        }

        /// <summary>
        /// Handles the input change for the NumberOfQuestionsDownButton
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void NumberOfQuestionsDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.NumberOfQuestions >= 2)
            {
                viewModel.NumberOfQuestions--;
                viewModel.OnPropertyChanged(nameof(viewModel.NumberOfQuestions));
            }
            
        }

        /// <summary>
        /// Handles the input change for the text box
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void ExamNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.ExamName = ExamNameTextBox.Text;
            //viewModel.OnPropertyChanged(nameof(viewModel.ExamName));
        }

        /*
        /// <summary>
        /// Handles the input change for the grading check box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsGrading_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.IsGrading = (bool)IsGrading.IsChecked;
            viewModel.OnPropertyChanged(nameof(viewModel.IsGrading));
        }
        */

        /// <summary>
        /// Changes the tab to the Scan tab from the start tab
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">routedeventargs</param>
        private void StartContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if(ExamNameTextBox.Text != "")
            {
                StartTab.IsSelected = false;
                PlaceCardsInTrayTab.IsSelected = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //Connected but not implemented.
            //Set all relevent values in the UIViewModel to the defaults
            //Close serial port
            //Do other functions that the reset button in the old program did
        }


        #endregion

        #region Place Cards In Tray Tab

        /// <summary>
        /// Changes the tab to the Scan tab from the place cards in tray tab
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">routedeventargs</param>
        private void PlaceCardsInTrayContinueButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceCardsInTrayTab.IsSelected = false;
            ScanTab.IsSelected = true;
        }

        #endregion

        #region Scan Tab

        /// <summary>
        /// Handles the continue button click on the Scan Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ScanTab.IsSelected = false;
            if(viewModel.IsGrading)
            {
                AskIfGradingTab.IsSelected = true;
            } else
            {
                FinishTab.IsSelected = true;
            }
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            ScanTabReadyButton.IsEnabled = false;
            ScanDoneButton.IsEnabled = true;
            viewModel.ViewModelBeginScanning();
        }

        private void ScanDoneButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ViewModelDoneScanning();
            ScanTab.IsSelected = false;
            AskIfGradingTab.IsSelected = true;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }


        #endregion

        #region Ask If Grading Tab
        private void UserIsGradingButton_Click(object sender, RoutedEventArgs e)
        {
            AskIfGradingTab.IsSelected = false;
            GradingQuestionTab.IsSelected = true;
        }

        private void UserIsNotGradingButton_Click(object sender, RoutedEventArgs e)
        {
            AskIfGradingTab.IsSelected = false;
            FinishTab.IsSelected = true;
        }

        #endregion

        #region Grading Question Tab

        private void GoToScanKeyButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.InstantiateAnswerKeys();
            GradingQuestionTab.IsSelected = false;
            ScanKeyTab.IsSelected = true;
        }

        private void GoToInputKeyManuallyButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.InstantiateAnswerKeys();
            GradingQuestionTab.IsSelected = false;
            InputKeyManuallyTab.IsSelected = true;
        }

        #endregion

        #region Scan Key Tab

        private void ScanKeyButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Scanner.Scan();
            ScanKeyButton.IsEnabled = false;
            ScanKeyDoneButton.IsEnabled = true;
        }

        private void ScanKeyDoneButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DoneScanningKeys();
            ScanKeyTab.IsSelected = false;
            InputKeyManuallyTab.IsSelected = true;
        }

        #endregion

        #region Input Key Manually Tab

        private void GetGradeBookFromManualInputButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(KeyValuePair<int, List<Question>> p in viewModel.Grader.AnswerKey)
            {
                foreach (Question q in viewModel.Grader.AnswerKey[p.Key])
                {
                    q.ManualAnswerKeyToNormalAnswer();
                }
            }
            
            viewModel.Grader.CreateStudents();
            viewModel.Grader.GradeStudents(viewModel.ExamName);

            viewModel.WriteFile("gradebook");
        }

        #endregion

        #region Not Grading Finish Tab

        private void SingleAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.WriteFile("single");
        }

        private void MultipleAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.WriteFile("multiple");
        }

        #endregion

        private void ResetProgramButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.raw_cards.Clear();
            viewModel.Grader.Cards.Clear();
            viewModel.Grader.Students.Clear();
            viewModel.Grader.PartialWIDs.Clear();
            viewModel.Grader.AnswerKey.Clear();
            viewModel.Grader.KeyCards.Clear();
            //viewModel = new UIViewModel();
            FinishTab.IsSelected = false;
            InputKeyManuallyTab.IsSelected = false;
            StartTab.IsSelected = true;
            ScanTabReadyButton.IsEnabled = true;
            ScanDoneButton.IsEnabled = false;
            ScanKeyButton.IsEnabled = true;
            ScanKeyDoneButton.IsEnabled = false;
        }

        private void SetAllPartialCreditBox_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
