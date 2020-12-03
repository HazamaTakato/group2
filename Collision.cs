using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace ActionGame
{
    class Collision
    {
        // メンバー変数の宣言
        private Sound sound;        // サウンド
        private Player player;      // プレイヤー
        private CharacterManager characterManager;      // キャラクターマネージャー
        private bool endFlag;//終了フラグ
        private bool stunFlag;//スタンフラグ
        private Random rand;//乱数
        private int KeyCount;//鍵のカウント
        private KeyAttack keyAttack;
        private ArrowAttack arrowAttack;
  //      private Enemy enemy;

        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        /// <param name="souhd"></param>
        /// <param name="player"></param>
        /// <param name=" characterManager "></param>
        public Collision(Sound sound, Player player, CharacterManager characterManager)
        {
            // 情報の登録
            this.sound = sound;// サウンド
            this.player = player;// プレイヤー
            //this.keyAttack = keyAttack;
            //this.arrowAttack = arrowAttack;
       //     this.enemy = enemy;
            this.characterManager = characterManager;// キャラクターマネージャー
            //乱数の生成
            rand = new Random();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // プレイヤーと鍵の衝突判定
            //CollisionPlayerKey();
            // プレイヤーと敵の衝突判定
            CollisionPlayerEnemy();
            // プレイヤー攻撃の衝突判定
            CollisionPlayerAttack();
            //enemy.Update();
        }
        /// <summary>
        /// プレイヤーと金の衝突
        /// </summary>
        private void CollisionPlayerKey()
        {
            // 座標を入れる変数の宣言と生成
            Vector2 playerPosition = new Vector2(0, 0);
            Vector2 keyPosition = new Vector2(0, 0);

            // プレイヤー座標の獲得
            playerPosition = player.GetPosition();

            // シーン継続に設定
            endFlag = false;

            // 敵コレクションの獲得
            List<Key> KeyList = characterManager.GetKeyList();

            // 敵コレクションでループ
            foreach (var k in KeyList)
            {
                // 敵の座標の獲得
                keyPosition = k.GetPosition();

                // ２つの円の中心の距離を求める
                float distance = Vector2.Distance(playerPosition, keyPosition);

                // 一定範囲で衝突
                if (distance < 50)
                {
                    k.Hit(player);
                    //enemy.Stun();
                    //endFlag = true;// シーン終了に設定
                }
            }
        }
        /// <summary>
        /// プレイヤーと敵の衝突
        /// </summary>
        private void CollisionPlayerEnemy()
        {
            // 座標を入れる変数の宣言と生成
            Vector2 playerPosition = new Vector2(0, 0);
            Vector2 keyposition = new Vector2(0, 0);
            Vector2 enemyPosition = new Vector2(0, 0);
            Vector2 enemySpeed = new Vector2(0, 0);
            Vector2 enemytama = new Vector2(0, 0);
            Vector2 enemykey = new Vector2(0, 0);

            // プレイヤー座標の獲得
            playerPosition = player.GetPosition();

            // シーン継続に設定
            endFlag = false;
            stunFlag = false;

            // 敵コレクションの獲得
            List<Enemy> enemyList = characterManager.GetEnemyList();
            List<Key> keyList = characterManager.GetKeyList();
            List<ArrowAttack> arrowAttackList = characterManager.GetArrowAttackList();
            List<KeyAttack> keyAttackList = characterManager.KeyAttackList();
            // 敵コレクションでループ
            foreach (var e in enemyList)
            {
                foreach (var k in keyList)
                {
                    // ダメージ中は衝突しない
                    // （ダメージ中以外で衝突判定処理を行う）
                    if (e.GetMode() != ActionMode.Damage)
                    {
                        // 敵の座標の獲得
                        enemyPosition = e.GetPosition();
                        keyposition = k.GetPosition();
                        enemySpeed = e.GetSpeed();
                        

                        // ２つの円の中心の距離を求める
                        float distance = Vector2.Distance(playerPosition, enemyPosition);
                        float distance2 = Vector2.Distance(playerPosition, keyposition);
                       
                        // 一定範囲で衝突
                        if (distance < 125&&player.GetMode()!=ActionMode.Move)
                        {
                            endFlag = true;// シーン終了に設定
                        }
                        else
                        {
                            endFlag = false;
                        }
                        //if (distance2 < 32)
                        //{
                        //    e.SetStun();
                        //}


                        foreach (var arrow in arrowAttackList)
                        {
                            enemytama = arrow.GetPosition();
                            float distance3 = Vector2.Distance(playerPosition, enemytama);
                            if (distance3 < 23)
                            {
                                endFlag = true;
                            }
                        }

                        foreach(var keyattack in keyAttackList)
                        {
                            enemykey = keyattack.GetPosition();
                            float distance4 = Vector2.Distance(playerPosition, enemykey);
                            if (distance4 < 125)
                            {
                                e.SetStun();
                            }
                        }

                    }
                    if (playerPosition.Y > Screen.Height + 64)
                    {
                        endFlag = true;
                    }
                }    
            } 
        }
        /// <summary>
        /// プレイヤーの攻撃判定
        /// </summary>
        private void CollisionPlayerAttack()
        {
            // 攻撃中のみ実施
            ActionMode mode = player.GetMode();
            if (mode == ActionMode.Attack || mode == ActionMode.JumpAttack)
            {   
                
            }
            else
            {
                return;
            }

            // 座標を入れる変数の宣言と生成
            Vector2 attackPosition = new Vector2(0, 0);
            Vector2 enemyPosition = new Vector2(0, 0);

            // プレイヤー座標の獲得
            // まず攻撃座標にプレイヤー座標を入れる
            attackPosition = player.GetPosition();

            // 方向で攻撃座標の補正
            if (player.GetDirection() == Direction.Right)
            {
                attackPosition.X += 32; // 右向き
                attackPosition.Y += 32;
            }
            else
            {
                attackPosition.X -= 32; // 左向き
                attackPosition.Y += 32;
            }

            // 敵コレクションの獲得
            List<Enemy> enemyList = characterManager.GetEnemyList();

            // 敵コレクションでループ
            foreach (var e in enemyList)
            {
                //ダメージ中は衝突しない
                //(ダメージ中以外で衝突判定処理を行う)
                if (e.GetMode() != ActionMode.Damage)
                {
                    // 敵の座標の獲得
                    enemyPosition = e.GetPosition();

                    // 攻撃位置と敵位置の距離を求める
                    float distance = Vector2.Distance(attackPosition, enemyPosition);

                    // 一定範囲で衝突
                    if (distance < 200)
                    {
                        if (e.GetMode() == ActionMode.Move)
                        {
                            e.Hit(); // 衝突した処理
                            
                        }
                        if (e.GetMode() == ActionMode.Stun)
                        {
                            e.Hit();
                            //e.SetMove();
                            
                            e.Move();
                            //e.SetMove();
                        }
                        //sound.PlaySE("");// ＳＥのプレイ
                        //CollisionKeyBorn(e);   // 金の発生（敵の情報を渡す)
                    }
                }
            }
        }
        /// <summary>
        /// 攻撃での金の発生
        /// </summary>
        /// <param name="e"></param>
        private void CollisionKeyBorn(Enemy e)
        {
            // ３回ループ
            for (int i = 0; i < 3; i++)
            {
                // 鍵の発生座標の決定
                Vector2 position = new Vector2(0, 0);  // 敵の座標の獲得
                position.X += rand.Next(-100, 101);    // Ｘ座標に乱数を加える
                position.Y = rand.Next(-150, -50);     // Ｙ座標は画面上の範囲での乱数

                // 鍵のスピードの決定
                int speedY = rand.Next(2, 7);

                // 鍵の発生
                characterManager.KeyBornPosition(position,speedY);
            }
        }
        /// <summary>
        ///  終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return endFlag;// 終了か？を返す
        }

        public bool StunFlag()
        {
            return stunFlag;
        }
    }
}
