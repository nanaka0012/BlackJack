using Altseed2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public class MainNode : Node
    {
        public int round;

        public TextNode WinnerText { get; set; }
        public TextNode PCardPointSum { get; set; }
        public TextNode DCardPointSum { get; set; }
        public TextNode TurnText { get; set; }
        public Deck Deck { get; set; }

        public Player Player { get; set; }

        public Dealer Dealer { get; set; }

        IEnumerator<int> coroutine;

        public MainNode()
        {
            Deck = new Deck();
            Player = new Player(Deck);
            Dealer = new Dealer(Deck);
            WinnerText = new TextNode();
            WinnerText.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 30);
            PCardPointSum = new TextNode();
            PCardPointSum.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 60);
            PCardPointSum.Position = new Vector2F(0, 500);
            DCardPointSum = new TextNode();
            DCardPointSum.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 60);
            DCardPointSum.Position = new Vector2F(0, 200);
            TurnText = new TextNode();
            TurnText.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 30);

            coroutine = Update();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            PCardPointSum.Text = $"{Player.Point}";
            DCardPointSum.Text = $"{Dealer.Point}";
            if (!(coroutine?.MoveNext() ?? false))
            {
                coroutine = Update();
            }
        }

        IEnumerator<int> Update()
        {
            AddChildNode(PCardPointSum);
            bool finish = false;

            Dealer.DrawCard(this);

            for (int i = 0; i < 20; i++)
            {
                yield return 0;
            }

            Dealer.DrawCard(this);
            for (int i = 0; i < 20; i++)
            {
                yield return 0;
            }

            Player.DrawCard(this);
            for (int i = 0; i < 20; i++)
            {
                yield return 0;
            }
            Player.DrawCard(this);

            while (!finish && Player.Point < 21)
            {
                // ヒット
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push)
                {
                    Player.DrawCard(this);
                    if (Player.Point > 21)
                    {
                        Console.WriteLine("Playerがバースト");
                        for (int i = 0; i < 20; i++)
                        {
                            yield return 0;
                        }
                        break;
                    }
                    else if (Player.Point == 21)
                        break;

                    finish = false;
                }
                // スタンド
                if (Engine.Keyboard.GetKeyState(Key.X) == ButtonState.Push)
                {
                    finish = true;
                    break;
                }

                yield return 0;
            }

            while (!Player.IsBurst && Dealer.Point < 17)
            {
                // ヒット
                if (Dealer.Point < 17)
                {
                    Dealer.DrawCard(this);
                    if (Dealer.Point > 21)
                    {
                        Console.WriteLine("Dealerがバースト");
                        break;
                    }
                }
                // スタンド
                else
                {
                    break;
                }

                yield return 0;
            }

            if (!Player.IsBurst)
            {
                if (Dealer.IsBurst)
                {
                    WinnerText.Text = "Playerの勝ち";
                }
                else
                {
                    if (Player.Point > Dealer.Point)
                    {
                        WinnerText.Text = "Playerの勝ち";
                    }
                    else if (Player.Point == Dealer.Point)
                    {
                        WinnerText.Text = "引き分け";
                    }
                    else
                    {
                        WinnerText.Text = "Dealerの勝ち";
                    }
                }
            }
            else
            {
                WinnerText.Text = "Dealerの勝ち";
            }

            AddChildNode(WinnerText);
            AddChildNode(DCardPointSum);

            while (true)
            {
                //次のラウンドのために初期化
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push)
                {
                    WinnerText.Text = "";
                    Player.Hand.Clear();
                    Dealer.Hand.Clear();
                    foreach (var card in Children.OfType<Card>())
                    {
                        RemoveChildNode(card);
                    }
                    finish = false;
                    RemoveChildNode(DCardPointSum);
                    break;
                }
                yield return 0;
            }
        }
    }
}
