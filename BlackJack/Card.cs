using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public enum Mark
    {
        Diamond,
        Spade,
        Heart,
        Club,
    }

    public class Card : SpriteNode
    {
        private int no;
        private Mark mark;

        public Mark Mark
        {
            get => mark;
            set
            {
                mark = value;
                UpdateTexture();
            }
        }

        /// <summary>
        /// トランプの数値
        /// </summary>
        public int No
        {
            get => no;
            set
            {
                if (value >= 1 && value <= 13)
                {
                    no = value;
                    UpdateTexture();
                }
            }
        }

        /// <summary>
        /// トランプの表示
        /// </summary>
        public string NoString
        {
            get
            {
                switch (No)
                {
                    case 1:
                        return "A";
                    case 11:
                        return "J";
                    case 12:
                        return "Q";
                    case 13:
                        return "K";
                }

                return No.ToString();
            }
        }

        public Card()
        {
            no = 1;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Card card)
                return card.Mark == Mark && card.No == No;
            return false;
        }

        private void UpdateTexture()
        {
            string prefix;
            switch (Mark)
            {
                case Mark.Diamond:
                    prefix = "d";
                    break;
                case Mark.Spade:
                    prefix = "s";
                    break;
                case Mark.Heart:
                    prefix = "h";
                    break;
                case Mark.Club:
                    prefix = "c";
                    break;
                default:
                    prefix = null;
                    break;
            }

            Texture = Texture2D.Load($"resources/tramps/{prefix}_{No}.png");
        }
    }
}
