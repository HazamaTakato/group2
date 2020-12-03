
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ActionGame
{
    class Player
    {
        // メンバー変数の宣言
        private Vector2 position;//位置
        private Vector2 size;//サイズ
        private Vector2 halfsize;//半分のサイズ
        private Direction dir;//方向
        private int timer;//タイマー
        private ActionMode mode;//モード
        private Vector2 speed;
        private Sound sound;
        private Stage stage;//ステージ
        private bool endFlag;//終了フラグ
        float velocityY = 0f;
        static readonly float JumpVelocity = 14f;
        static readonly float Gravity = 0.5f;
        private Vector2 halfsize2;
        private Vector2 halfsizeR;
        private Vector2 halfsizeL;
        private bool ItemGetFlag;

        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public Player(Sound sound,Stage stage)
        { 
            position = new Vector2();//位置
            size = new Vector2(64,128);//サイズ
            halfsize = new Vector2(32,64);//半分のサイズ
            halfsize2 = new Vector2(16, 64);
            halfsizeR = new Vector2(64/2, 64);
            halfsizeL = new Vector2(64/2, 64);
            speed = new Vector2();//スピード
            this.sound = sound;
            this.stage = stage;//ステージ
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 座標の初期化
            position.X = 100;
            position.Y = 200;
            //方向の初期化
            dir = Direction.Right;
            //タイマーの初期化
            timer = 0;
            //モードの初期化
            mode = ActionMode.Stand; //ジャンプモードに設定
            //移動速度の初期化
            speed.Y = 0;
            //ゲーム継続に設定
            endFlag = false;
            ItemGetFlag = false;
        }
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // 切出し用（安全の為に0で初期化）
            float sx = 0, sy = 0,sx2=0;
            //モードにより切り出し位置を求める
            switch (mode)
            {
                // 移動
                case ActionMode.Move:
                    // ０→１→２→１の順番でアニメーション
                    sx = timer / 8;
                    if (sx == 7||sx2==7)
                    {
                        sx = 0;
                        sx2 = 0;
                    }
                    sy = 0;
                    break;
                //立ち
                case ActionMode.Stand:
                    //2パターンでアニメーション
                    sx = 0;
                    sx2 = 0;
                    sy = 1;
                    break;
                //ジャンプ
                case ActionMode.Jump:
                    sx = 0;
                    sx2 = 0;
                    sy = 2;
                    break;
                //攻撃
                case ActionMode.Attack:
                    //順番にアニメーション
                    sx = 0;
                    sx2 = 0;
                    sy = 1;
                    break;
                //それ以外（ありえないはずだが、安全のため)
                default:
                    break;
            }

            // 番号から切出し位置へ変換
            sx *= (int)size.X;
            sx2 *= (int)halfsize.X;
            //sx2 *= (int)halfsize.X;
            sy *= (int)size.Y;

            // 切出し位置を決定
            Rectangle rect = new Rectangle((int)sx, (int)sy, (int)size.X, (int)size.Y);
            // 表示（名前と座標を指定）
            // 右向きならば
            if (dir == Direction.Right)
            {
                //rect.X = 1024 - rect.X + 64;
                renderer.DrawTexture("Player5", stage.GetScreenPosition(position) - halfsize, rect);
            }
            //左向きならば
            else
            {
                //Rectangle rect2 = new Rectangle((int)sx2, (int)sy, (int)size.X, (int)size.Y);
                //切出し位置の変更
                rect.X = 514 - rect.X - 64;

                //表示（名前と座標と切り出す図形を指定）
                renderer.DrawTexture("Player55", stage.GetScreenPosition(position) - halfsize2, rect);
            }
            if (mode == ActionMode.Attack&&dir==Direction.Right)
            {
                renderer.DrawTexture("key2", new Vector2(position.X + 26, position.Y - 12), 1.0f);
            }
            else if (mode == ActionMode.Attack && dir == Direction.Left)
            {
                renderer.DrawTexture("key2L", new Vector2(position.X - 76, position.Y - 12), 1.0f);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            Move();//移動
            InScreen();//移動範囲の制限
            AnimationUpdate();//アニメーション更新
            ChangeMode();//モード切替え
            JumpStart();//ジャンプ開始
            JumpUpdate();//ジャンプ更新
            AttackStart();//攻撃開始
            FallStart();//落下開始
           // position += new Vector2(0, velocityY);
        }
        /// <summary>
        /// 落下開始
        /// </summary>
        private void FallStart()
        {
            if (mode == ActionMode.Jump)
            {
                return;
            }
            Vector2 startposition = position + new Vector2(-halfsize.X / 2, halfsize.Y);

            if (stage.CollisionUpDown(startposition, size.X) == true)
            {
                velocityY = 0;
                mode = ActionMode.Jump;
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Move()
        {
            //攻撃中は移動ができない
            if (mode == ActionMode.Attack)
            {
                return;
            }

            // 右を押したら、右へ移動
            if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {
                MoveRight();    // 右へ移動
            }

            // 左を押したら、左へ移動
            if (Keyboard.GetState().IsKeyDown(Keys.Left) ||
                GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
            {
                MoveLeft();    // 左へ移動
            }
        }
        /// <summary>
        /// 右へ移動
        /// </summary>
        private void MoveRight()
        {
            //1ドットごとに移動処理を行う
            for(int i = 0; i < 4; i++)
            {
                MoveRightOne();//個別
            }
            dir = Direction.Right;//右向きにする
        }
        /// <summary>
        /// 左へ移動
        /// </summary>
        private void MoveLeft()
        {
            //1ドットごとに移動処理を行う
            for(int i = 0; i < 4; i++)
            {
                MoveLeftOne();//個別
            }
            dir = Direction.Left;//左向きにする
        }
        /// <summary>
        /// 右へ移動　個別
        /// </summary>
        private void MoveRightOne()
        {
            // プレイヤーの右上のチェック開始位置を出す
            // 右へ移動するので、１つ右の位置をチェックする
            Vector2 startPosition = position + new Vector2(halfsize.X / 2 + 1, -halfsize.Y);

            // そこにブロックが無ければ
            if (stage.CollisionSide(startPosition,size.Y) == true)
            {
                position.X++;   // 右へ移動
                // スクロール位置なら
                if (stage.GetScreenX(position.X) > 600)
                {
                    stage.ScrollLeft();     // 左へスクロールする
                }
            }
        }
        /// <summary>
        /// 左へ移動　個別
        /// </summary>
        private void MoveLeftOne()
        {
            // プレイヤーの左上のチェック開始位置を出す
            // 左へ移動するので、１つ左の位置をチェックする
            Vector2 startPosition = position + new Vector2(-halfsize.X / 2 - 1  , -halfsize.Y);

            // そこにブロックが無ければ
            if (stage.CollisionSide(startPosition,size.Y) == true)
            {
                position.X--;   // 右へ移動
                //スクロール位置なら
                if (stage.GetScreenX(position.X) < 300)
                {
                    stage.ScrollRight();//右へスクロールする
                }
            }
        }

        /// <summary>
        /// 移動範囲の制限
        /// </summary>
        private void InScreen()
        {
            //画面下に出たら
            if (position.Y > Screen.Height)
            {
                endFlag = true;//削除するように設定
            }
        }
        /// <summary>
        /// アニメーション
        /// </summary>
        private void AnimationUpdate()
        {
            // 加算
            timer++;

            // モードにより対応したアニメーションを行う
            switch (mode)
            {
                // 移動（8パターンで戻す）
                case ActionMode.Move:
                    if (timer >= 7.5f * 8)
                    {
                        timer = 0;
                    }
                    break;

                // 立ち（2パターンで戻す）
                case ActionMode.Stand:
                    if (timer >= 0)
                    {
                        timer = 0;
                    }
                    break;
                // 攻撃（4パターンで終了）
                case ActionMode.Attack:
                    if (timer >= 15)
                    {
                        // 立ちへ変更
                        mode = ActionMode.Stand;
                        timer = 0;
                    }
                    break;
                // ジャンプ攻撃（0.5秒で終了）
                case ActionMode.JumpAttack:
                    if (timer >= 30)
                    {
                        // ジャンプへ変更
                        mode = ActionMode.Jump;
                        timer = 0;
                    }
                    break;
            }
        }
        /// <summary>
        /// モード切替え
        /// </summary>
        private void ChangeMode()
        {
            // 左右を押していれば
            if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Left) ||
                GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
            {
                // 立ちモードなら、移動モードへ切替え
                if (mode == ActionMode.Stand)
                {
                    mode = ActionMode.Move;   // 移動モードに切替え
                    timer = 0;                  // タイマーの初期化
                }
            }
            // 押してなければ
            else
            {
                // 移動モードなら、立ちモードへ切り替え
                if (mode == ActionMode.Move)
                {
                    mode = ActionMode.Stand; // 立ちモードに切替え
                    timer = 0;
                }
            }
        }
        /// <summary>
        /// ジャンプ開始
        /// </summary>
        void JumpStart()
        {
            if (mode == ActionMode.Jump)
            {
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //Console.WriteLine("aaa");
                //speed.Y = -16;
                mode = ActionMode.Jump;
                velocityY = -JumpVelocity;
                sound.PlaySE("JumpSE");//SE
            }
        }
        /// <summary>
        /// ジャンプ更新
        /// </summary>
        void JumpUpdate()
        {
            if (mode == ActionMode.Jump)
            {
                //velocityY += Gravity;
                //position.Y += velocityY;
                for (float i = 0; i < Math.Abs(velocityY); i += 0.5f)
                {
                    if (velocityY > 0)
                    {
                        MoveDownOne();
                    }
                    else
                    {
                        MoveUpOne();
                    }
                }
            }
            velocityY += 0.5f;
        }
        /// <summary>
        /// 座標の獲得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return position;    // 座標を返す
        }

        /// <summary>
        /// 攻撃開始
        /// </summary>
        void AttackStart()
        {
            // 移動か立ちなら攻撃開始
            if (mode == ActionMode.Stand || mode == ActionMode.Move)
            {
                // ＸキーかＢボタンキーで攻撃開始
                if (Keyboard.GetState().IsKeyDown(Keys.X) ||
                    GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    mode = ActionMode.Attack;// モードを攻撃に設定
                    sound.PlaySE("AttackSE01");        // 攻撃ＳＥプレイ
                    timer = 0;                        // タイマーのクリア
                    //Console.WriteLine("a");
                }
            }
        }

        /// <summary>
        /// モードの獲得
        /// </summary>
        public ActionMode GetMode()
        {
            // モードを返す
            return mode;
        }

        /// <summary>
        /// 方向の獲得
        /// </summary>
        public Direction GetDirection()
        {
            return dir;// 方向を返す
        }
        /// <summary>
        /// 下へ移動の実施
        /// </summary>
        private void MoveDownOne()
        {
            // プレイヤーの左下のチェック開始位置を出す
            Vector2 startPosition = position + new Vector2(-halfsize.X / 2, halfsize.Y);
            // そこにブロックが無ければ
            if (stage.CollisionUpDown(startPosition, halfsize.X) == true)
            {
                position.Y += 0.5f;	// 下へ移動
            }
            // そこにブロックが有れば着地
            else
            {
                mode = ActionMode.Stand;// モードを立ちに設定
                timer = 0;// タイマーを初期化
            }
        }
        /// <summary>
        /// 上へ移動の実施
        /// </summary>
        private void MoveUpOne()
        {
            // プレイヤーの左上のチェック開始位置を出す
            Vector2 startPosition = position + new Vector2(halfsize.X / 2, -halfsize.Y);
            {

                // そこにブロックが無ければ
                if (stage.CollisionUpDown(startPosition, halfsize.X) == true)
                {
                    position.Y -= 0.5f; // 上へ移動
                }
                // そこにブロックが有れば
                else
                {
                    velocityY = 0;  // 落下に設定
                }
            }
        }
        /// <summary>
        /// ゲーム終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return endFlag;// 終了フラグを返す
        }

        public void Hit(Character other)
        {
            if(other is KeyAttack)
            {
                ItemGetFlag = true;
                Console.WriteLine("a");
            }
        }
    }
}
