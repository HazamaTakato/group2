using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ActionGame
{
    class Load:IScene
    {
        // メンバー変数の宣言
        private bool endFlag; // 終了か？（ver3ではisEnd）
        int timer;           // タイマー
        private Sound sound;//サウンド

        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public Load(Sound sound)
        {
            this.sound = sound;//サウンド
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            endFlag = false;   // シーン継続に設定
            timer = 0;          // タイマーの初期化
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {

        }



        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // タイトルの表示（名前と座標を指定）
            //renderer.DrawTexture("load", new Vector2(20, 20));

            // １ループ表示を行った後でロード処理を行う
            // 本来は表示に処理を書くのは好ましくないが、
            // rendererを使用するので、ここに書く
            timer++;
            if (timer > 2)
            {
                endFlag = true;     // シーン終了に設定

                // 絵の読み込み(ロードを使うならこちらでロード)
                renderer.LoadTexture("Bst");    // タイトル背景
                renderer.LoadTexture("Bblo");
                renderer.LoadTexture("Block");
                renderer.LoadTexture("Player33");
                renderer.LoadTexture("Player333");
                renderer.LoadTexture("Player11");
                renderer.LoadTexture("Player22");
                renderer.LoadTexture("Player111");
                renderer.LoadTexture("Player5");
                renderer.LoadTexture("Player55");
                renderer.LoadTexture("enemytama");
                renderer.LoadTexture("key");
                renderer.LoadTexture("boss");
                renderer.LoadTexture("arrow");
                renderer.LoadTexture("key1");
                renderer.LoadTexture("key1L");
                renderer.LoadTexture("key2");
                renderer.LoadTexture("key2L");
                renderer.LoadTexture("title2");
                renderer.LoadTexture("go");
                renderer.LoadTexture("miss2");
                renderer.LoadTexture("miss5");
                renderer.LoadTexture("ope2");
                renderer.LoadTexture("title3");
                renderer.LoadTexture("story");
                renderer.LoadTexture("story2");
                renderer.LoadTexture("clear");
                renderer.LoadTexture("miss5");

                sound.LoadBGM("TitleBGM");
                //sound.LoadBGM("PlayBGM");
                sound.LoadBGM("PlayBGM");
                sound.LoadBGM("GameOverBGM");
                sound.LoadBGM("GameClearBGM");

                sound.LoadSE("AttackSE01");
                sound.LoadSE("AttackSE02");
                sound.LoadSE("JumpSE");
                sound.LoadSE("DamageSE");
                sound.LoadSE("StartSE");

                //renderer.LoadTexture("player");       // プレイヤー右向き

                //sound.LoadBGM("");

                //sound.LoadSE();
            }
        }

        /// <summary>
        ///  終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            // 終了か？を返す
            return endFlag;
        }



        /// <summary>
        /// 次のシーンを返す
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            // 次のシーンを返す（タイトル）
            return Scene.Title;
        }

    }
}
