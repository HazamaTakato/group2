using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ActionGame
{
    class Key
    {
        // メンバー変数の宣言
        private Vector2 position;   // 位置
        private Vector2 size;       // サイズ
        private Vector2 halfSize;   // 半分のサイズ
        private bool deadFlag;      // 削除フラグ
        private int timer;          // タイマー
        private Vector2 speed;
        private Stage stage;
        private Player player;
        private Enemy enemy;
        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public Key(Vector2 setPosition, int speedY,Stage stage,Enemy enemy)
        {
            // クラスの生成
            //position = new Vector2(200,500);          // 位置
            size = new Vector2(32, 32);       // サイズ
            halfSize = new Vector2(size.X / 2, size.Y / 2);// 半分のサイズ
            this.stage = stage;
            this.enemy = enemy;
            //this.speed = speed;
            // 初期設定

            //// スピードの設定
            speed.Y = 2;
            speed.X = 0;

            //タイマー初期化
            timer = 0;

            //存在させる
            deadFlag = false;
        }

        public void Update()
        {
           // InTimer();
            timer++;//タイマーカウントアップ
        }

        public void Hit(Player character)
        {
            if (character is Player)
            {
                //enemy.Stun();
            }
        }

        private void InTimer()
        {
            if (timer == 7)
            {
                deadFlag = true;
            }
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // 切出し位置を決定
            Rectangle rect = new Rectangle(0, 0, (int)size.X, (int)size.Y);

            // 表示（名前と座標を指定）
            renderer.DrawTexture("key",stage.GetScreenPosition(position)-halfSize,rect);
        }

        public bool IsDead()
        {
            //deadFlag = true;
            return deadFlag;
        }

        /// <summary>
        /// 座標の獲得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return position;// 座標を返す
        }
    }
}
