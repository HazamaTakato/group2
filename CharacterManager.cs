-using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ActionGame
{
    class CharacterManager
    {
        // メンバー変数の宣言
        private Sound sound;        // サウンド
        private Player player;      // プレイヤー
        private Random rand; //乱数
        private Stage stage;
        // キャラクターを複数入れるリスト型のコレクション
        private List<KeyAttack> keyAttacksList;
        private List<ArrowAttack> arrowAttacks;
        private List<Enemy> enemyList;//敵
        private List<Key> keyList;//鍵
        private Enemy e;
        private int ShotTimer;
        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="player"></param>
        public CharacterManager(Sound sound,Player player,Stage stage)
        {
            // 情報の登録
            this.sound = sound;     // サウンド
            this.player = player;   // プレイヤー
            this.stage = stage;
            //コレクションの生成
            enemyList = new List<Enemy>();//敵
            keyList = new List<Key>();//鍵
            keyAttacksList = new List<KeyAttack>();
            arrowAttacks = new List<ArrowAttack>();
            //乱数の生成
            rand = new Random();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //コレクションのクリア
            enemyList.Clear();//敵
            keyList.Clear();
            keyAttacksList.Clear();
            arrowAttacks.Clear();
            Enemy e = new Enemy(stage);
            KeyBornPosition(new Vector2(200, 300), 0);
            enemyList.Add(e);
            ShotTimer = 120;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            // 敵コレクションの更新
            foreach (var e in enemyList)
            {
                e.Update();//敵の更新
            }
            //鍵コレクションの更新
            foreach(var k in keyList)
            {
                k.Update();
            }
            foreach(var ks in keyAttacksList)
            {
                ks.Update();
            }
            foreach(var ass in arrowAttacks)
            {
                ass.Update();
            }
            //発生関係
            EnemyShot();
            //EnemyBorn();//敵
            //KeyBornPosition(new Vector2(),0);
            //不要な実体の削除
            enemyList.RemoveAll(e => e.IsDead() == true);//敵
            if (keyList.Find(k => k.IsDead() )!= null)
            {
                int a = 50;
            }
            keyList.RemoveAll(k => k.IsDead() == true);//鍵
            arrowAttacks.RemoveAll(a => a.IsDead() == true);
            keyAttacksList.RemoveAll(ka => ka.IsDead() == true);
        }
        /// <summary>
        /// 敵の発生 
        /// </summary>
        void EnemyBorn()
        {
            //enemyList.Add(new Enemy(stage));
        }
        /// <summary>
        /// 金の発生
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speedY"></param>
        public void KeyBornPosition(Vector2 position, int speedY)
        {
            keyList.Add(new Key(position, speedY, stage, e));   // 金の発生
            //foreach (var e in enemyList)
            //{
            //    keyList.Add(new Key(position, speedY, stage, e));   // 金の発生
            //}
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //敵コレクションの表示
            foreach(var e in enemyList)
            {
                e.Draw(renderer);//敵の表示
            }
            //鍵コレクションの表示
            foreach(var k in keyList)
            {
                k.Draw(renderer);//鍵の表示
            }
            foreach (var ks in keyAttacksList)
            {
                ks.Draw(renderer);
            }
            foreach (var ass in arrowAttacks)
            {
                ass.Draw(renderer);
            }
        }
        /// <summary>
        /// 金コレクションの獲得
        /// </summary>
        /// <returns></returns>
        public List<Key> GetKeyList()
        {
            return keyList;//鍵コレクションの情報を返す
        }

        public List<Enemy> GetEnemyList()
        {
            return enemyList;//敵コレクションの情報を返す
        }

        public List<ArrowAttack> GetArrowAttackList()
        {
            return arrowAttacks;
        }

        public List<KeyAttack> KeyAttackList()
        {
            return keyAttacksList;
        }

        public void EnemyShot()
        {
            foreach(var e in enemyList)
            {
                Vector2 position = new Vector2(0, 0);
                position = e.GetPosition();
                if (ShotTimer > 0)
                {
                    ShotTimer -= 1;
                }
                if(ShotTimer==0)
                {
                    Random rnd = new Random();
                    int arrowType = rnd.Next(5);
                    arrowAttacks.Add(new ArrowAttack("arrow", position, new Vector2(-8, 8), ShotType.NORMAL));
                    arrowAttacks.Add(new ArrowAttack("arrow", position, new Vector2(-8, 4), ShotType.NORMAL));
                    if (arrowType == 0)
                    {
                        keyAttacksList.Add(new KeyAttack("key", position, new Vector2(-8, 0), ShotType.POISON));
                    }
                    else
                    {
                        arrowAttacks.Add(new ArrowAttack("arrow", position, new Vector2(-8, 0), ShotType.NORMAL));
                    }
                    arrowAttacks.Add(new ArrowAttack("arrow", position, new Vector2(-8, -4), ShotType.NORMAL));
                    arrowAttacks.Add(new ArrowAttack("arrow", position, new Vector2(-8, -8), ShotType.NORMAL));
                    //Console.WriteLine(arrowType);
                    EnemyShotTime();
                }
            }
        }
        public void EnemyShotTime()
        {
            ShotTimer = rand.Next(300, 421);
        }
    }
}
