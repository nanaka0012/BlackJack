using Altseed2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    public class MainNode : Node
    {
        public int Round = 1;

        public TextNode Score { get; set; }

        public SpriteNode Title { get; set; }
        public SpriteNode Win { get; set; }
        public SpriteNode Lose { get; set; }
        public SpriteNode Draw { get; set; }
        public SpriteNode Bust { get; set; }
        public SpriteNode Blackjack { get; set; }

        public TextNode PCardPointSum { get; set; }
        public TextNode DCardPointSum { get; set; }
        public TextNode RoundText { get; set; }
        public Deck Deck { get; set; }

        public Player Player { get; set; }

        public Dealer Dealer { get; set; }

        IEnumerator coroutine;

        public MainNode()
        {
            Deck = new Deck();
            Player = new Player(Deck);
            Dealer = new Dealer(Deck);

            //タイトル
            Title = new SpriteNode();
            Title.Texture = Texture2D.Load("resources/title.png");
            Title.Position = Engine.WindowSize / 2;
            Title.CenterPosition = Title.ContentSize / 2;
            Title.ZOrder = 5;
            AddChildNode(Title);

            //勝ち表示
            Win = new SpriteNode();
            Win.Texture = Texture2D.Load("resources/win.png");
            Win.Position = Engine.WindowSize / 2;
            Win.CenterPosition = Win.ContentSize / 2;
            Win.ZOrder = 10;
            Win.IsDrawn = false;
            AddChildNode(Win);

            //負け表示
            Lose = new SpriteNode();
            Lose.Texture = Texture2D.Load("resources/lose.png");
            Lose.Position = Engine.WindowSize / 2;
            Lose.CenterPosition = Lose.ContentSize / 2;
            Lose.ZOrder = 10;
            Lose.IsDrawn = false;
            AddChildNode(Lose);

            //引き分け
            Draw = new SpriteNode();
            Draw.Texture = Texture2D.Load("resources/draw.png");
            Draw.Position = Engine.WindowSize / 2;
            Draw.CenterPosition = Draw.ContentSize / 2;
            Draw.ZOrder = 10;
            Draw.IsDrawn = false;
            AddChildNode(Draw);

            //バスト
            Bust = new SpriteNode();
            Bust.Texture = Texture2D.Load("resources/bust.png");
            Bust.Position = Engine.WindowSize / 2;
            Bust.CenterPosition = Bust.ContentSize / 2;
            Bust.ZOrder = 10;
            Bust.IsDrawn = false;
            AddChildNode(Bust);

            //ブラックジャック
            Blackjack = new SpriteNode();
            Blackjack.Texture = Texture2D.Load("resources/jack.png");
            Blackjack.Position = Engine.WindowSize / 2;
            Blackjack.CenterPosition = Blackjack.ContentSize / 2;
            Blackjack.ZOrder = 10;
            Blackjack.IsDrawn = false;
            AddChildNode(Blackjack);

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

            //Score = new TextNode();
            //Score.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 60);
            //Score.CenterPosition = new Vector2F(0, Score.ContentSize.Y / 2);
            //Score.Position = new Vector2F(10, 50);
            //Score.ZOrder = 5;
            //Score.Text = "○　×";
            //AddChildNode(Score);

            RoundText = new TextNode();
            RoundText.Text = Round.ToString();
            RoundText.Font = Font.LoadDynamicFont("resources/mplus-1m-regular.ttf", 70);
            RoundText.CenterPosition = RoundText.ContentSize / 2;
            RoundText.Position = new Vector2F(837, 645);
            RoundText.ZOrder = 2;
            AddChildNode(RoundText);

            coroutine = PlayCoroutine(Update());
        }

        protected override void OnAdded()
        {
            // 背景
            var backGround = new SpriteNode();
            backGround.Texture = Texture2D.Load("resources/BackGround.png");
            backGround.ZOrder = -10;
            AddChildNode(backGround);

            //キー表示
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
                coroutine = PlayCoroutine(Update());
            }
        }

        IEnumerator<IEnumerator> Update()
        {
            while (true)
            {
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push)
                {
                    RemoveChildNode(Title);
                    break;
                }
                yield return null;
            }

            while (true)
            {
                yield return RoundRoutine();

                while (true)
                {
                    yield return null;
                    //次のラウンドのために初期化
                    if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push || (Engine.Keyboard.GetKeyState(Key.X) == ButtonState.Push))
                    {
                        if (Round == 10)
                        {
                            Engine.RemoveNode(this);
                            Engine.AddNode(new MainNode());
                        }

                        DCardPointSum.Text = "?";
                        Win.IsDrawn = false;
                        Lose.IsDrawn = false;
                        Draw.IsDrawn = false;
                        Player.Hand.Clear();
                        Dealer.Hand.Clear();
                        foreach (var card in Children.OfType<Card>())
                        {
                            RemoveChildNode(card);
                        }

                        Round++;
                        RoundText.Text = Round.ToString();
                        RoundText.CenterPosition = RoundText.ContentSize / 2;

                        break;
                    }
                }
            }
        }

        IEnumerator<IEnumerator> RoundRoutine()
        {
            //プレイヤー手番の終了フラグ
            bool finish = false;

            //Dealerの最初のカードだけ伏せカードに
            var firstCard = Dealer.FirstDraw(this);
            yield return Delay(20);

            Dealer.DrawCard(this);
            yield return Delay(20);

            Player.DrawCard(this);
            yield return Delay(20);

            Player.DrawCard(this);

            while (!finish && Player.Point < 21)
            {
                // ヒット
                if (Engine.Keyboard.GetKeyState(Key.X) == ButtonState.Push)
                {
                    Player.DrawCard(this);
                    if (Player.Point > 21)
                    {
                        Console.WriteLine("Playerがバースト");
                        yield return Delay(20);
                        Bust.IsDrawn = true;
                        yield return Delay(50);
                        Bust.IsDrawn = false;
                        break;
                    }
                    else if (Player.Point == 21)
                        break;

                    finish = false;
                }
                // スタンド
                if (Engine.Keyboard.GetKeyState(Key.Z) == ButtonState.Push)
                {
                    finish = true;
                    break;
                }

                yield return null;
            }

            if (Player.Point == 21)
            {
                Blackjack.IsDrawn = true;
                yield return Delay(50);
                Blackjack.IsDrawn = false;
            }

            var se = Sound.Load(@"resources/card-turn-over.ogg", true);
            Engine.Sound.Play(se);
            firstCard.IsReverse = false;
            
            DCardPointSum.Text = $"{Dealer.Point}";

            yield return Delay(20);


            while (!Player.IsBurst && Dealer.Point < 17)
            {
                // ヒット
                if (Dealer.Point < 17)
                {
                    yield return Delay(20);

                    Dealer.DrawCard(this);
                    DCardPointSum.Text = $"{Dealer.Point}";

                    if (Dealer.Point > 21)
                    {
                        Console.WriteLine("Dealerがバースト");
                        yield return Delay(20);
                        Bust.IsDrawn = true;
                        yield return Delay(50);
                        Bust.IsDrawn = false;
                        break;
                    }
                }
                // スタンド
                else
                {
                    break;
                }

                yield return null;
            }

            if (Dealer.Point == 21)
            {
                Blackjack.IsDrawn = true;
                yield return Delay(50);
                Blackjack.IsDrawn = false;
            }

            if (!Player.IsBurst)
            {
                if (Dealer.IsBurst)
                {
                    Win.IsDrawn = true;
                }
                else
                {
                    if (Player.Point > Dealer.Point)
                    {
                        Win.IsDrawn = true;
                    }
                    else if (Player.Point == Dealer.Point)
                    {
                        Draw.IsDrawn = true;
                    }
                    else
                    {
                        Lose.IsDrawn = true;
                    }
                }
            }
            else
            {
                Lose.IsDrawn = true;
            }

            DCardPointSum.Text = $"{Dealer.Point}";

            yield return Delay(20);
        }

        IEnumerator<IEnumerator> Delay(int frame)
        {
            for (int i = 0; i < frame; i++)
            {
                yield return null;
            }
        }

        IEnumerator PlayCoroutine(IEnumerator coroutine)
        {
            var currentCoroutine = coroutine;
            Stack<IEnumerator> stackCroutine = new Stack<IEnumerator>();
            while (true)
            {
                if (currentCoroutine?.MoveNext() ?? false)
                {
                    if (currentCoroutine?.Current is IEnumerator sub)
                    {
                        stackCroutine.Push(currentCoroutine);
                        currentCoroutine = sub;
                    }
                }
                else
                {
                    if (stackCroutine.Count == 0) yield break;
                    currentCoroutine = stackCroutine.Pop();
                }
                yield return null;
            }
        }
    }
}
