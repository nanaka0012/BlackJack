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

        public void FirstDraw(MainNode node)
        {
            var card = Deck.Draw();
            Console.WriteLine($"Dealer: {card.Mark}, {card.No}");
            Hand.Add(card);
            card.Position = new Vector2F(200 * Hand.Count, 200);
            card.IsReverse = true;
            node.AddChildNode(card);
        }

        public void DrawCard(MainNode node)
        {
            var card = Deck.Draw();
            Console.WriteLine($"Dealer: {card.Mark}, {card.No}");
            Hand.Add(card);
            card.Position = new Vector2F(200 * Hand.Count, 200);
            node.AddChildNode(card);
        }
    }
}
