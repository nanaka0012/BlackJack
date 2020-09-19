using Altseed2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public class MainNode : Node
    {
        public int Round = 1;

        public TextNode WinnerText { get; set; }
        public TextNode PCardPointSum { get; set; }
        public TextNode DCardPointSum { get; set; }
        public TextNode RoundText { get; set; }
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
            WinnerText.Color = new Color(255, 155, 39);
            WinnerText.CenterPosition = WinnerText.ContentSize / 2;
            WinnerText.Position = Engine.WindowSize / 2;
            WinnerText.ZOrder = 20;
            WinnerText.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 100);

            PCardPointSum = new TextNode();
            PCardPointSum.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 60);
            PCardPointSum.CenterPosition = PCardPointSum.ContentSize / 2;
            PCardPointSum.Position = new Vector2F(520, 665);
            PCardPointSum.ZOrder = 5;
            AddChildNode(PCardPointSum);

            DCardPointSum = new TextNode();
            DCardPointSum.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 60);
            DCardPointSum.CenterPosition = DCardPointSum.ContentSize / 2;
            DCardPointSum.Position = new Vector2F(520, 83);
            DCardPointSum.ZOrder = 5;
            DCardPointSum.Text = "?";
            AddChildNode(DCardPointSum);

            RoundText = new TextNode();
            RoundText.Text = Round.ToString();
            RoundText.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 70);
            RoundText.CenterPosition = RoundText.ContentSize / 2;
            RoundText.Position = new Vector2F(837, 645);
            RoundText.ZOrder = 2;
            AddChildNode(RoundText);

            coroutine = Update();
        }

        protected override void OnAdded()
        {
            // 背景
            var backGround = new SpriteNode();
            backGround.Texture = Texture2D.Load("resources/BackGround.png");
            backGround.ZOrder = -10;
            AddChildNode(backGround);

            var key = new SpriteNode();
            key.Texture = Texture2D.Load("resources/Key.png");
            key.Position = new Vector2F(50, 600);
            key.ZOrder = 1;
            AddChildNode(key);

            // ラウンド表示の背景
            var roundBackground = new SpriteNode();
            roundBackground.Texture = Texture2D.Load("resources/RoundBackground.png");
            roundBackground.Position = new Vector2F(800, 575);
            roundBackground.ZOrder = 1;
            AddChildNode(roundBackground);

            // プレイヤーの持ち点表示の背景
            var playerPointBackground = new SpriteNode();
            playerPointBackground.Texture = Texture2D.Load("resources/PlayerPointBackground.png");
            playerPointBackground.CenterPosition = playerPointBackground.ContentSize / 2;
            playerPointBackground.Position = new Vector2F(460, 650);
            playerPointBackground.ZOrder = 1;
            AddChildNode(playerPointBackground);

            // ディーラーの持ち点表示の背景
            var dealerPointBackground = new SpriteNode();
            dealerPointBackground.Texture = Texture2D.Load("resources/DealerPointBackground.png");
            dealerPointBackground.CenterPosition = dealerPointBackground.ContentSize / 2;
            dealerPointBackground.Position = new Vector2F(460, 70);
            dealerPointBackground.ZOrder = 1;
            AddChildNode(dealerPointBackground);

            base.OnAdded();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            PCardPointSum.Text = $"{Player.Point}";
            if (!(coroutine?.MoveNext() ?? false))
            {
                coroutine = Update();
            }
        }

        IEnumerator<int> Update()
        {
            bool finish = false;

            //Dealerの最初のカードだけ伏せカードに
            var firstCard = Dealer.FirstDraw(this);


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
            firstCard.IsReverse = false;

            while (!Player.IsBurst && Dealer.Point < 17)
            {
                // ヒット
                if (Dealer.Point < 17)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        yield return 0;
                    }
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

            WinnerText.CenterPosition = WinnerText.ContentSize / 2;
            DCardPointSum.Text = $"{Dealer.Point}";
            AddChildNode(WinnerText);

            for (int i = 0; i < 20; i++)
            {
                yield return 0;
            }

            while (true)
            {
                //次のラウンドのために初期化
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push || (Engine.Keyboard.GetKeyState(Key.X) == ButtonState.Push))
                {
                    if (Round == 10)
                    {
                        Engine.RemoveNode(this);
                        Engine.AddNode(new MainNode());
                    }

                    DCardPointSum.Text = "?";
                    WinnerText.Text = "";
                    Player.Hand.Clear();
                    Dealer.Hand.Clear();
                    foreach (var card in Children.OfType<Card>())
                    {
                        RemoveChildNode(card);
                    }
                    finish = false;

                    Round++;
                    RoundText.Text = Round.ToString();
                    RoundText.CenterPosition = RoundText.ContentSize / 2;

                    break;
                }
                yield return 0;
            }
        }
    }
}
