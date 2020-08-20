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
            Console.WriteLine("Zキーでカードを引く");

            Engine.AddNode(new MainNode());

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
