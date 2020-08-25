﻿using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJack
{
    public class Player : PlayerBase
    {
        public Player(Deck deck) : base(deck)
        {
        }

        public void DrawCard(MainNode node)
        {
            var card = Deck.Draw();
            Console.WriteLine($"Player: {card.Mark}, {card.No}");
            Hand.Add(card);
            card.Position = new Vector2F(100 + 100 * Hand.Count, 370);
            card.ZOrder = Hand.Count;
            node.AddChildNode(card);
        }
    }
}
