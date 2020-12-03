using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionGame
{
    class GameOver:IScene
    {
        // メンバー変数の宣言
        private bool endFlag; // 終了フラグ
        private bool isPressKey;//キーを押したか？
        private int timer;//タイマー(文字点滅用)
        private Sound sound;//サウンド

        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public GameOver(Sound sound)
        {
            this.sound = sound;//サウンド
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 変数の初期化
            // シーン継続に設定
            endFlag = false;
            isPressKey = true;//「押した」に設定
            timer = 0;//タイマー
            //sound.PlayBGM("");//BGMのプレイ
            //sound.PlaySE("");//SEプレイ
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            sound.PlayBGM("GameOverBGM");
            // スペースキーを押したら
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                //前回が押してなければ
                if (isPressKey == false)
                {
                    sound.PlaySE("StartSE");
                    endFlag = true;//シーン終了に設定
                    isPressKey = true;//「押した」に設定
                    //sound.StopBGM();//BGM停止
                }
            }
            else
            {
                isPressKey = false;//「押してない」に設定
            }
            timer++;//タイマーのカウントアップ
        }



        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // ゲームオーバーの表示（名前と座標を指定）
            renderer.DrawTexture("go", new Vector2(0, 0), 1f);

            //文字の表示
            //renderer.DrawTexture("titletext", new Vector2(220, 0), 0.6f);
        }

        /// <summary>
        ///  終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            // 終了フラグを返す
            return endFlag;
        }

        /// <summary>
        /// 次のシーンを返す
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            sound.StopBGM();
            // 次のシーンを返す
            return Scene.Title;
        }

    }
}
