using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Altseed2;

namespace BlackJack
{
    public class Dealer : PlayerBase
    {
        public Dealer(Deck deck) : base(deck)
        {
        }

        public Card FirstDraw(MainNode node)
        {
            var card = Deck.Draw();
            Console.WriteLine($"Dealer: {card.Mark}, {card.No}");
            Hand.Add(card);
            card.Position = new Vector2F(100 + 100 * Hand.Count, 150);
            card.ZOrder = Hand.Count;

            card.IsReverse = true;
            node.AddChildNode(card);
            return card;
        }

        public void DrawCard(MainNode node)
        {
            var card = Deck.Draw();
            Console.WriteLine($"Dealer: {card.Mark}, {card.No}");
            Hand.Add(card);
            card.Position = new Vector2F(100 + 100 * Hand.Count, 150);
            card.ZOrder = Hand.Count;
            node.AddChildNode(card);
        }
    }
}
