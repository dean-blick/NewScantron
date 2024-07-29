using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScantronRevived.Data
{
    public class Question : INotifyPropertyChanged
    {
        
         // A char array that holds the answer a student gives or the answer on the answer key.
        public string answer = "";
        // How many points this question is worth.
        private float points;
        // Holds if this questions can be graded for partial credit on multiple answer questions.
        public bool partial_credit;

        public string actualAnswers = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _input0 = false;

        private bool _input1 = false;

        private bool _input2 = false;

        private bool _input3 = false;

        private bool _input4 = false;

        public bool Input0
        {
            get
            {
                return _input0;
            }
            set
            {
                _input0 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input0)));
            }
        }
        public bool Input1
        {
            get
            {
                return _input1;
            }
            set
            {
                _input1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input1)));
            }
        }

        public bool Input2
        {
            get
            {
                return _input2;
            }
            set
            {
                _input2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input2)));
            }
        }

        public bool Input3
        {
            get
            {
                return _input3;
            }
            set
            {
                _input3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input3)));
            }
        }

        public bool Input4
        {
            get
            {
                return _input4;
            }
            set
            {
                _input4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input4)));
            }
        }

        public string Answer
        {
            get
            {
                return answer;
            }
        }

        public float Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Points)));
            }
        }

        public bool PartialCredit
        {
            get
            {
                return partial_credit;
            }
            set
            {
                partial_credit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartialCredit)));
            }
        }

        public Question(string answer, float points, bool partial_credit)
        {
            this.answer = answer;
            this.Points = points;
            this.points = points;
            this.partial_credit = partial_credit;
            this.PartialCredit = partial_credit;
        }

        /// <summary>
        /// Method for grading this question. https://community.canvaslms.com/docs/DOC-6674-understanding-multiple-answers-questions
        /// </summary>
        /// <param name="answer_key">Answer key to grade this questions against.</param>
        public void Grade(Question answer_key)
        {
            for(int i = 0; i <= 4; i++)
            {
                if (answer[i] > 60) actualAnswers += i+1;
                else actualAnswers += " ";
            }
            if (answer_key.PartialCredit)
            {
                float total_answers = 0;
                float correct_answers = 0;
                float incorrect_answers = 0;

                

                for (int i = 0; i < answer_key.Answer.Length; i++)
                {
                    if (answer_key.answer[i] != ' ')
                    {
                        total_answers++;

                        if (actualAnswers[i] == ' ')
                        {
                            // No points are deducted.
                        }
                        else if (actualAnswers[i] == answer_key.answer[i])
                        {
                            correct_answers++;
                        } 
                        else
                        {
                            incorrect_answers++;
                        }
                    }
                    
                    if(answer_key.answer[i] == ' ')
                    {
                        if (actualAnswers[i] != ' ')
                        {
                            incorrect_answers++;
                        }
                    }
                }
                
                points = ((1 / total_answers) * correct_answers - (1 /total_answers) * incorrect_answers ) * answer_key.points;

                if (points < 0 || float.IsNaN(points))
                {
                    points = 0;
                }
            }
            else
            {
                for (int i = 0; i < answer_key.Answer.Length; i++)
                {
                    if (actualAnswers[i] != answer_key.Answer[i])
                    {
                        points = 0;
                        return;
                    }
                }

                points = answer_key.Points;
                return;
            }
        }
        
        public void ManualAnswerKeyToNormalAnswer()
        {
            if(Input0)
            {
                answer += "1";
            } else
            {
                answer += " ";
            }
            if (Input1)
            {
                answer += "2";
            }
            else
            {
                answer += " ";
            }
            if (Input2)
            {
                answer += "3";
            }
            else
            {
                answer += " ";
            }
            if (Input3)
            {
                answer += "4";
            }
            else
            {
                answer += " ";
            }
            if (Input4)
            {
                answer += "5";
            }
            else
            {
                answer += " ";
            }
        }
    }
}
