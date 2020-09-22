using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public abstract class PlayerBase
    {
        /// <summary>
        /// 手札
        /// </summary>
        public List<Card> Hand { get; }

        /// <summary>
        /// 持ち点
        /// </summary>
        public int score;

        /// <summary>
        /// 勝った回数
        /// </summary>
        public int WinCount;

        /// <summary>
        /// バーストしたかどうか
        /// </summary>
        public bool IsBurst => Point > 21;

        public Deck Deck { get; set; }

        public PlayerBase(Deck deck)
        {
            this.Hand = new List<Card>();
            Deck = deck;
        }

        public int Point
        {
            get
            {
                int CalcRec(int point, int index)
                {
                    if (Hand.Count <= index) return point;
                    var card = Hand[index];
                    switch (card.No)
                    {
                        case 1:
                            {
                                int p1 = CalcRec(point + 11, index + 1);
                                if (p1 > 21) return CalcRec(point + 1, index + 1);
                                return p1;
                            }
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            return CalcRec(point + 10, index + 1);
                        default:
                            return CalcRec(point + card.No, index + 1);
                    }
                }
                return CalcRec(0, 0);
            }
        }
    }
}
