using Altseed2;
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
            var se = Sound.Load(@"resources/card-put.ogg", true);
            Engine.Sound.Play(se);

            var card = Deck.Draw();
            Hand.Add(card);
            card.Position = new Vector2F(100 + 100 * Hand.Count, 370);
            card.ZOrder = Hand.Count;
            node.AddChildNode(card);
        }
    }
}
