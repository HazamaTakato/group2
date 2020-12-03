using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ActionGame
{
    class Enemy
    {
        // メンバー変数の宣言
        private Vector2 position;//位置
        private Vector2 size;//サイズ
        private Vector2 halfsize;//半分のサイズ
        private Vector2 speed;//スピード
        private Direction dir;//方向
        private int timer;//タイマー
        private bool deadFlag;//削除フラグ
        private ActionMode mode;//モード
        private Stage stage;
        private int HP;//HP
        private Collision collision;
        private bool isJump;
        private Vector2 velocity;
        private bool fallflag;
        private int stunTimer;
        private int dirRand;
        private int jumpTimer;
        private int RandomJump;
        private int DirChange;
        private int ChangeTimer;
        //乱数の生成
        Random rand = new Random();



        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public Enemy(Stage stage)
        {
            // クラスの生成
            position = new Vector2(1100, 500);//位置
            size = new Vector2(256, 256);//サイズ
            halfsize = new Vector2(256 / 2, 256 / 2);//半分のサイズ
            speed = new Vector2();//スピード
            this.stage = stage;

            dir = Direction.Left;
            //// 左から発生
            //if (rand.Next(1) == 0)
            //{
            //    //position.X = -halfsize.X;// Ｘ座標
            //    dir = Direction.Left;// 右向きにする
            //}
            //// 右から発生
            //else
            //{
            //    //position.X = Screen.Width + halfsize.X;// Ｘ座標
            //    dir = Direction.Right;// 左向きにする
            //}


            DirChangeTimer();
            BaseJumpTime();
            BaseSpeed();
            //タイマー初期化
            timer = 0;
            //削除しない
            deadFlag = false;
            fallflag = false;
            // モードの初期化
            mode = ActionMode.Move;

            HP = 20;
        }
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // 切出し用
            int sx = 0, sy = 0;

            //モードにより切り出し位置を求める
            switch (mode)
            {
                case ActionMode.Move:
                    // 移動
                    // ０→１→２→１の順番でアニメーション
                    sx = 0;
                    sy = 0;
                    //sx = timer / 15;
                    //       if (sx == 3)
                    //       {
                    //           sx = 1;
                    //       }
                    //       sy = 1;
                    break;

                // ダメージ
                case ActionMode.Damage:
                    // 順番にアニメーション
                    sx = 0;
                    sy = 0;
                    //sx = timer / 20;
                    //sy = 0;
                    //// ２を超えたら２にする
                    //if (sx > 2)
                    //{
                    //    sx = 2;
                    //}
                    break;
                case ActionMode.Stun:
                    sx = 0;
                    sy = 0;
                    break;
            }



            // 番号から切出し位置へ変換
            sx *= (int)size.X;
            sy *= (int)size.Y;

            //切り出し位置を決定
            Rectangle rect = new Rectangle(sx, sy, (int)size.X, (int)size.Y);
            // 右向きならば
            if (dir == Direction.Left)
            {
                // 表示（名前と座標と切り出す図形を指定）
                renderer.DrawTexture("boss", stage.GetScreenPosition(position) - halfsize, rect);
            }
            // 左向きならば
            else
            {
                // 切出し位置の変更
                rect.X = 256 - rect.X - (int)size.X;

                // 表示（名前と座標と切り出す図形を指定）
                renderer.DrawTexture("boss", stage.GetScreenPosition(position) - halfsize, rect);
            }

        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            MoveStun();
            if (isJump == false && mode != ActionMode.Stun)
            {
                Jump();
            }

            switch (mode)
            {
                case ActionMode.Move:
                    velocity.Y = (velocity.Y > 10.0f) ? (10.0f) : (velocity.Y + 0.55f);//落下速度処理
                    break;
                case ActionMode.Stun:

                    break;
            }

            //移動
            position += velocity;
            //AnimationUpdate();//アニメーション更新
            InScreen();//移動範囲の制限
            DirectionChange();

        }



        public void MoveStun()
        {
            if (mode == ActionMode.Stun)
            {
                Stun();
            }
            else
            {
                Move();
            }
        }
        public void Stun()
        {
            speed.X = 0;
            velocity.Y += 1;
            if (stunTimer > 0)
            {
                stunTimer -= 1;
            }

            if (stunTimer <= 0 && dir == Direction.Right)
            {

                dir = Direction.Right;
                mode = ActionMode.Move;
            }
            else if (stunTimer <= 0 && dir == Direction.Left)
            {
                dir = Direction.Left;
                mode = ActionMode.Move;

            }

            if (stunTimer == 0)
            {
                BaseStunTime();
                BaseSpeed();

            }



        }
        /// <summary>
        /// 移動
        /// </summary>
        public void Move()
        {


            if (dir == Direction.Right)
            {
                MoveRight();    // 右へ移動
            }
            // 左向きなら
            else
            {
                MoveLeft();    // 左へ移動
            }


        }
        /// <summary>
        /// 右へ移動
        /// </summary>
        private void MoveRight()
        {
            position.X += speed.X;// 右へ移動
        }
        /// <summary>
        /// 左へ移動
        /// </summary>
        private void MoveLeft()
        {
            position.X -= speed.X;// 左へ移動
        }
        /// <summary>
        /// アニメーション
        /// </summary>
        private void AnimationUpdate()
        {
            // 加算
            timer++;

            // 移動（4パターンで戻す）
            if (timer >= 15 * 4)
            {
                timer = 0;
            }
        }
        /// <summary>
        /// 移動範囲の制限
        /// </summary>
        private void InScreen()
        {
            // 画面の左に出たら
            if (position.X < -halfsize.X + size.X + 100)
            {

                dir = Direction.Right;      // 削除するように設定
            }

            // 画面の右に出たら
            if (position.X > Screen.Width + halfsize.X - size.X)
            {

                dir = Direction.Left;      // 削除するように設定
            }

            //if (velocity.X < 0 && position.X <= 128)
            //{
            //    velocity.Y = -velocity.Y;
            //}

            // 画面下に出たら
            //if (position.Y > Screen.Height - 64)
            //{
            //    velocity.Y = +velocity.Y;     // 削除するように設定
            //    position.Y = Screen.Height - 194;
            //}
            //if (position.Y < 80)
            //{
            //    velocity.Y = -velocity.Y;     // 削除するように設定
            //}
            // 画面下に出たら
            //if (position.Y > Screen.Height - size.Y)
            //{
            //    isJump = false;     // 削除するように設定
            //}

            position = Vector2.Clamp(position,
                new Vector2(0, 0),
                new Vector2(Screen.Width,
                Screen.Height - 192));
            if (position.Y >= Screen.Height - 192)
            {
                isJump = false;
            }
        }
        // <summary>
        /// 削除するか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return deadFlag;//削除フラグを返す
        }


        /// <summary>
        /// 座標の獲得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return position;// 座標を返す
        }

        public Vector2 GetSpeed()
        {
            return speed;
        }

        public ActionMode Mode()
        {
            return mode;
        }

        /// <summary>
        /// 衝突した処理
        /// </summary>
        public void Hit()
        {
            mode = ActionMode.Damage;   // ダメージ中にする

            HP = HP - 1;
            if (HP == 0)
            {
                deadFlag = true;
            }

            timer = 0;                  // タイマーの初期化

            mode = ActionMode.Move;

            //speed.X *= -1;              // 移動量の反転

            //speed.Y = -16;              // ジャンプの初速
        }
        public void DirChangeTimer()
        {
            ChangeTimer = 200;
        }
        public void DirectionChange()
        {
            if (ChangeTimer > 0)
            {
                ChangeTimer -= 1;
            }
            if (ChangeTimer == 0)
            {
                DirChange = rand.Next(3);
                if (DirChange == 0)
                {
                    if (dir == Direction.Left)
                    {
                        dir = Direction.Right;
                    }
                    else
                    {
                        dir = Direction.Left;
                    }
                }
                DirChangeTimer();
            }
            //Console.WriteLine(DirChange);
           
        }
        /// <summary>
        /// モードの獲得
        /// </summary>
        public ActionMode GetMode()
        {
            return mode;// モードを返す
        }
        public void Initialize()
        {

        }

        public void SetStun()
        {
            mode = ActionMode.Stun;
        }

        public Vector2 Jump()
        {
            if (jumpTimer > 0)
            {
                jumpTimer -= 1;
            }
            if (jumpTimer == 0)
            {
                RandomJump = rand.Next(2);
                if (RandomJump == 1)
                {
                    velocity.Y -= 30;
                    isJump = true;
                    if (isJump == true)
                    {
                        BaseSpeed();
                    }
                }
                BaseJumpTime();
            }

            //float margin = 15.0f;

            //if (840 - margin < position.X && position.X < 840 + margin)
            //{
            //    if (position.Y < 520)
            //    {
            //        velocity.Y = -20.0f;
            //        isJump = true;
            //    }
            //}
            //if (320 - margin < position.X && position.X < 320 + margin)
            //{
            //    if (position.Y < 520)
            //    {
            //        velocity.Y = -20.0f;
            //        isJump = true;
            //    }
            //}

            return position;
        }

        public void BaseStunTime()
        {
            stunTimer = 300;
        }

        public void BaseSpeed()
        {
            //スピードの設定
            speed.X = rand.Next(3, 7);
            //speed.X = 2;
            speed.Y = 0;

        }
        public void BaseJumpTime()
        {
            jumpTimer = 90;
        }
    }
}
