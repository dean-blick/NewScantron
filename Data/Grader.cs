using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScantronRevived.Data
{
    public class Grader : INotifyCollectionChanged
    {
        UIViewModel viewModel;

        public Grader(UIViewModel v)
        {
            viewModel = v;
        }


        // Holds the exam name to be used in the output file.
        string exam_name = "";
        // Holds the cards used to create the students.
        private List<Card> cards = new List<Card>();

        public List<Card> KeyCards = new List<Card>();
        // Hold the list of students to be graded
        private List<Student> students = new List<Student>();

        // Holds the answer key to compare to student responses.
        private Dictionary<int, List<Question>> answer_key = new Dictionary<int, List<Question>>();
        // Holds the partial misread WID's
        private List<string> partial_wids = new List<string>();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        public List<Student> Students
        {
            get
            {
                return students;
            }
        }
        
        public Dictionary<int, List<Question>> AnswerKey
        {
            get
            {
                return answer_key;
            }
            set
            {
                answer_key = value;
            }
        }

        public int GlobalPointValue
        {
            set
            {
                foreach(KeyValuePair<int, List<Question>> pair in AnswerKey)
                {
                    foreach(Question q in pair.Value)
                    {
                        q.Points = value;
                        
                    }
                }
            }
        }

        public bool GlobalPartialCredit
        {
            set
            {
                foreach (KeyValuePair<int, List<Question>> pair in AnswerKey)
                {
                    foreach (Question q in pair.Value)
                    {
                        q.PartialCredit = value;
                    }
                }
            }
        }

        public List<string> PartialWIDs
        {
            get
            {
                return partial_wids;
            }
        }

        /// <summary>
        /// Create a list of cards from the raw card data.
        /// </summary>
        /// <param name="raw_cards">Raw card data read in from Scantron.</param>
        public void CreateCards(List<string> raw_cards, bool IsKeys)
        {
            if(!IsKeys)
            {
                cards.Clear();
                for (int i = 0; i < raw_cards.Count; i++)
                {
                    cards.Add(new Card(raw_cards[i]));
                }
            }
            else
            {
                KeyCards.Clear();
                for (int i = 0; i < raw_cards.Count; i++)
                {
                    KeyCards.Add(new Card(raw_cards[i]));
                }
            }
            
        }

        /// <summary>
        /// Create a list of cards from the raw card data.
        /// </summary>
        /// <param name="raw_cards">Raw card data read in from Scantron.</param>
        public void CreateKeyCards(List<string> raw_cards)
        {
            KeyCards.Clear();
            for (int i = 0; i < raw_cards.Count; i++)
            {
                KeyCards.Add(new Card(raw_cards[i]));
            }
        }

        /// <summary>
        /// Creates the students based off of the list of cards.
        /// </summary>
        public void CreateStudents()
        {
            students.Clear();

            foreach (Card card in cards)
            {
                // Checks for a partial wid on the card; 
                // We want to create a student regardless to retain the scores read in;
                if (card.WID.Contains('-'))
                {
                    partial_wids.Add(card.WID);
                }

                Student student = new Student(card);

                if (students.Exists(item => item.WID == card.WID))
                {
                    student = students.Find(item => item.WID == card.WID);
                    student?.Cards.Add(card);
                }
                else
                {
                    students.Add(student);
                }
            }

            foreach (Student student in students)
            {
                student.CreateResponse();
            }
        }

        /// <summary>
        /// Check student answers against the answer key.
        /// </summary>
        /// <returns>True if no errors occurred.</returns>
        public void GradeStudents(string exam_name)
        {
            this.exam_name = exam_name;
            string ungraded_students = "";



            foreach (Student student in students)
            {
                int test_version = student.TestVersion;

                

                for (int i = 0; i < answer_key[0].Count; i++)
                {
                    if (student.TestVersion > 0 && student.TestVersion <= answer_key.Count && student.Response.Count >= answer_key[0].Count)
                    {
                        student.Response[i].Grade(answer_key[test_version - 1][i]);
                    }
                    else
                    {
                        ungraded_students += student.WID + "\n";
                        break;
                    }
                }
            }

            if (ungraded_students != "")
            {
                MessageBox.Show("Some students could not be graded. They could have incorrect information written on their card, or the machine did not read the card properly");
            }
        }

        /// <summary>
        /// Convert the students' grades into a CSV file to be uploaded to the Canvas gradebook.
        /// </summary>
        /// <returns></returns>
        public string GradebookFile()
        {
            float points_possible;

            if (answer_key.Count < 1)
            {
                points_possible = 0;
            }
            else
            {
               points_possible = answer_key[0].Sum(question => question.Points);
            }

            string info = "Student,ID,SIS User ID,SIS Login ID,Section," + exam_name + Environment.NewLine +
                            "Points Possible,,,,," + points_possible + Environment.NewLine;

            foreach (Student student in students)
            {
                info += student.ToString();
            }

            return info;
        }

        /// <summary>
        /// Create a string to print to a file for only single answer questions.
        /// </summary>
        /// <returns>File string.</returns>
        public string SingleAnswerFile(int numberofquestions)
        {
            string info = "";

            foreach (Card card in cards)
            {
                info += card.ToSingleAnswerString(numberofquestions);
            }

            return info;
        }

        /// <summary>
        /// Create a string to print to a file that can handle multiple answer questions.
        /// </summary>
        /// <returns>File string.</returns>
        public string MultipleAnswerFile()
        {
            string info = "";

            foreach (Card card in cards)
            {
                info += card.ToMultipleAnswerString();
            }

            return info;
        }
        
    }
}
