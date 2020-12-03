using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ActionGame
{
    class GamePlay:IScene
    {
        // メンバー変数の宣言
        private bool endFlag;       // 終了フラグ
        private bool stunFlag;
        private Stage stage;
        private Sound sound;//サウンド
        private Player player;//プレイヤー
        private CharacterManager characterManager;//キャラクターマネージャー
        private Collision collision;//衝突判定クラス
        private Enemy enemy;
        private KeyAttack keyAttack;
        private ArrowAttack arrowAttack;
        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public GamePlay(Sound sound)
        {
            stage = new Stage();
            this.sound = sound;//サウンド
            player = new Player(sound,stage);
            enemy = new Enemy(stage);
            //keyAttack = new KeyAttack();
            //arrowAttack = new ArrowAttack();
            characterManager = new CharacterManager(sound, player,stage);//キャラクターマネージャー
            collision = new Collision(sound, player, characterManager);
            // 落下でゲーム終了
            if (collision.IsEnd()==true)
            {
                endFlag = true;//シーン終了に設定
                sound.StopBGM();//BGM停止
            }
            if (collision.StunFlag() == true)
            {
                stunFlag = true;
                //enemy.Stun();
            }
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 変数の初期化
            endFlag = false;                 // シーン継続に設定
            stunFlag = false;
            //sound.PlayBGM("");//BGMプレイ
            //sound.PlaySE("");//SEプレイ
            //各クラスの初期化
            player.Initialize();
            characterManager.Initialize();
            stage.Initialize();
        }


        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            sound.PlayBGM("PlayBGM");
            player.Update();//プレイヤー
            collision.Update();//衝突判定
           // enemy.Update();
            characterManager.Update();

            // Aキー、Bキー同時に押したら
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                endFlag = true;                // シーン終了に設定
                sound.StopBGM();//BGM停止
            }
            // ゲーム終了
            if (collision.IsEnd() == true)
            {
                endFlag = true; // シーン終了に設定
                sound.StopBGM();// ＢＧＭ停止
            }
            //if (collision.StunFlag() == true)
            //{
            //    enemy.Stun();
            //}
        }



        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            stage.Draw(renderer);//ステージ
            player.Draw(renderer);//プレイヤー
            characterManager.Draw(renderer);
            //player.Draw(renderer);//プレイヤー
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

        public bool StunFlag()
        {
            return stunFlag;
        }

        /// <summary>
        /// 次のシーンを返す
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            sound.StopBGM();
            // 次のシーンを返す
            if (collision.IsEnd()==true&&endFlag==true)
            {
                return Scene.GameOver;
            }
            else if(collision.IsEnd()==false&&endFlag==true)
            {
                return Scene.Ending;
            }
            else
            {
                return Scene.GameOver;
            }
        }
    }
}
