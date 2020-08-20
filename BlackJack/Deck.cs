using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlackJack
{
    public class Deck
    {
        public Stack<Card> Stack { get; set; }

        public Deck()
        {
            Stack = new Stack<Card>();
            for (int i = 0; i < 4; i++)
            {
                foreach (Mark mark in Enum.GetValues(typeof(Mark)))
                {
                    foreach (int no in Enumerable.Range(1, 13))
                    {
                        Stack.Push(new Card() { No = no, Mark = mark });
                    }
                }
            }
            Shuffle(Stack);
        }

        public void Shuffle<T>(Stack<T> stack)
        {
            var rnd = new Random();
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                stack.Push(value);
        }

        public Card Draw()
        {
            return Stack.Pop();
        }
    }
}
