using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ActionGame
{
    class Sound
    {
        // メンバー変数の宣言
        private ContentManager contentManager;  // コンテンツマネージャー

        // BGMのオブジェクトを複数入れるコンテナ
        // Song クラスはMP3 形式を再生可能
        private Dictionary<string, Song> bgms = new Dictionary<string, Song>();

        //サウンドエフェク（ S E ） のオブジェクトを複数入れるコンテナ
        // SoundEffectクラスではwav形式が再生可能
        private Dictionary<string, SoundEffect> SEs = new Dictionary<string, SoundEffect>();
        private Dictionary<string, SoundEffectInstance> SEInstances =
            new Dictionary<string, SoundEffectInstance>();

        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        /// <param name="content"></param>
        public Sound(ContentManager content)
        {
            // システムの初期設定
            contentManager = content;                   // コンテンツマネージャー
            contentManager.RootDirectory = "Content";   // ルートの設定
            MediaPlayer.IsRepeating = true; // BGMの再生はループ再生
        }



        // BGM関連--------------------------------------

        /// <summary>
        /// BGMファイルのロード
        /// </summary>
        /// <param name="name"></param>
        public void LoadBGM(string name)
        {
            bgms[name] = contentManager.Load<Song>(name);
        }

        /// <summary>
        /// BGMが停止中か？ ( 再生中でないか？ )
        /// </summary>
        /// <returns>再生中でなければtrue</returns>
        public bool IsStopBGM()
        {
            return MediaPlayer.State != MediaState.Playing;
        }

        /// <summary>
        /// BGMの再生
        /// </summary>
        /// <param name="name">アセット名</param>
        public void PlayBGM(string name)
        {
            //もし、BGMが再生されていなければ
            if (IsStopBGM())
            {
                //ボリューム設定
                MediaPlayer.Volume = 0.5f;

                //MediaPlayer で引数のBGMを再生
                MediaPlayer.Play(bgms[name]);
            }
        }

        /// <summary>
        /// BGMの停止
        /// </summary>
        public void StopBGM()
        {
            //MediaPlay e r を停止
            MediaPlayer.Stop();
        }

        /// <summary>
        /// BGMのループ再生を設定
        /// </summary>
        /// <param name="loopFlag">ループ再生フラグ： t r u e ならリピート</param>
        public void SetBGMLoopPlay(bool loopFlag)
        {
            MediaPlayer.IsRepeating = loopFlag;
        }



        // SE関連--------------------------------------

        /// <summary>
        /// SEファイルのロード
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="loopFlag">ループ再生するかどうか？ true ならリピート</param>
        public void LoadSE(string name, bool loopFlag = false)
        {
            //コンテンツ管理者でSEファイルの読み込み
            SEs[name] = contentManager.Load<SoundEffect>(name);
        }

        /// <summary>
        /// SEの再生
        /// </summary>
        /// <param name="name">アセット名</param>
        public void PlaySE(string name)
        {
            SEs[name].Play();
        }

        /// <summary>
        /// SEファイルのインスタンス生成
        /// 一時停止、単一音再生、ループ再生ができる
        /// </summary>
        /// <param name="name">アセット名</param>
        public void CreateInstance(string name, bool loopFlag = false)
        {
            // soundEffectのインスタンスを作成し、コンテナに登録
            SEInstances[name] = SEs[name].CreateInstance();

            //登録したSEのループ再生するか
            //第2引数を指定してない場合はデフォルト値false
            SEInstances[name].IsLooped = loopFlag;
        }

        /// <summary>
        /// SEが停止中か？ （ 再生されていないか？ ）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>再生されていなければtrue</returns>
        public bool IsStopSE(string name)
        {
            //指定されたS E の状態が再生中でなければtrue
            return SEInstances[name].State != SoundState.Playing;
        }



        /// <summary>
        /// SEインスタンスを再生
        /// </summary>
        /// <param name="name"></param>
        public void PlayInstanceSE(string name)
        {
            //再生のためのアセット名が登録されていなければ処理しない
            if (!SEInstances.ContainsKey(name))
            {
                return;
            }

            //もし、S E が再生されていなければ
            if (IsStopSE(name))
            {
                //引数で指定したSEを再生
                SEInstances[name].Play();
            }
        }

        /// <summary>
        /// SEの停止
        /// </summary>
        /// <param name="name">アセット名</param>
        public void StopSE(string name)
        {
            SEInstances[name].Stop();
        }

        /// <summary>
        /// SEのループ再生を設定
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="loopFlag">ループフラグ。リピート再生ならtrue</param>
        public void SetSELoopPlay(string name, bool loopFlag)
        {
            //指定されたSEのループ再生を設定
            SEInstances[name].IsLooped = loopFlag;
        }

        // --------------------------------------

        /// <summary>
        /// 解放処理
        /// </summary>
        public void Unload()
        {
            bgms.Clear();
            SEs.Clear();
            SEInstances.Clear();
            contentManager.Unload();
        }
    }
}
