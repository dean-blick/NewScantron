using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScantronRevived.Data
{
    public class Student
    {
         // The student's WID.
        private string wid;
        // The test version
        private int test_version;
        // The student's Scantron card(s).
        private List<Card> cards = new List<Card>();
        // The student's responses compiled from the cards.
        private List<Question> response = new List<Question>();
        
        public string WID
        {
            get
            {
                return wid;
            }
        }

        public int TestVersion
        {
            get
            {
                return test_version;
            }
        }
        
        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        public List<Question> Response
        {
            get
            {
                return response;
            }
        }

        public Student(Card card)
        {
            this.wid = card.WID;
            this.test_version = card.TestVersion;
            cards.Add(card);
        }

        /// <summary>
        /// Convert the student's list of cards to a list of answers.
        /// </summary>
        public void CreateResponse()
        {
            cards.Sort((a, b) => a.SheetNumber.CompareTo(b.SheetNumber));

            foreach (Card card in cards)
            {
                response.AddRange(card.Response);
            }
        }
        
        /// <summary>
        /// Get the student's score.
        /// </summary>
        /// <returns>Student's exam score.</returns>
        public float Score()
        {
            float score = 0;

            foreach (Question question in response)
            {
                score += question.Points;
            }

            return score;
        }

        /// <summary>
        /// Format the student as a string.
        /// </summary>
        /// <returns>The student formatted as a string.</returns>
        public override string ToString()
        {
            return wid + ",," + wid + ",,," + Score() + "\r\n";
        }
        
    }
}
