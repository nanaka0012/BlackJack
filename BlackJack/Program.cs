using Altseed2;
using System;
using System.Collections;

namespace BlackJack
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        { 
            // エンジンを初期化
            Engine.Initialize("Tutorial", 960, 720);

            Engine.AddNode(new MainNode());

            var bgm = Sound.Load("resources/c17.ogg", false);
            bgm.LoopStartingPoint = 0.0f;
            bgm.LoopEndPoint = bgm.Length;
            bgm.IsLoopingMode = true;
            Engine.Sound.Play(bgm);

            // メインループ
            while (Engine.DoEvents())
            {
                // エンジンを更新
                Engine.Update();

                // Escapeキーでゲーム終了
                if (Engine.Keyboard.GetKeyState(Key.Escape) == ButtonState.Push)
                {
                    break;
                }
            }

            // エンジンの終了処理を行う
            Engine.Terminate();
        }
    }
}
