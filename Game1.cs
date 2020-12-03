// このファイルで必要なライブラリのnamespaceを指定
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// プロジェクト名がnamespaceとなります
/// </summary>
namespace ActionGame
{
    /// <summary>
    /// ゲームの基盤となるメインのクラス
    /// 親クラスはXNA.FrameworkのGameクラス
    /// </summary>
    public class Game1 : Game
    {
        // フィールド（このクラスの情報を記述）
        private GraphicsDeviceManager graphicsDeviceManager;//グラフィックスデバイスを管理するオブジェクト
        private SpriteBatch spriteBatch;//画像をスクリーン上に描画するためのオブジェクト
        private Renderer renderer;  // レンダラー（表示）クラス
        private SceneManager sceneManager;//シーンマネージャー
        private Sound sound;//サウンド

        /// <summary>
        /// コンストラクタ
        /// （new で実体生成された際、一番最初に一回呼び出される）
        /// </summary>
        public Game1()
        {
            //グラフィックスデバイス管理者の実体生成
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            //コンテンツデータ（リソースデータ）のルートフォルダは"Contentに設定
            Content.RootDirectory = "Content";
            // 画面サイズの措定
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.Width;   // 幅
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.Height; // 高さ
        }

        /// <summary>
        /// 初期化処理（起動時、コンストラクタの後に1度だけ呼ばれる）
        /// </summary>
        protected override void Initialize()
        {
            // この下にロジックを記述

            // 上表示タイトルの指定
            base.Window.Title = "アクションゲーム";

            // クラスの生成
            renderer = new Renderer(Content, GraphicsDevice);   // レンダラー
            sceneManager = new SceneManager();//シーンマネージャー
            sound = new Sound(Content);//サウンド

            //シーンクラスの登録
            sceneManager.Add(Scene.Title, new Title(sound));//タイトル
            sceneManager.Add(Scene.Story, new Story(sound));
            sceneManager.Add(Scene.Operation, new Operation(sound));
            sceneManager.Add(Scene.Mission, new Mission(sound));
            sceneManager.Add(Scene.GamePlay, new GamePlay(sound));//ゲームプレイ
            sceneManager.Add(Scene.GameOver, new GameOver(sound));//ゲームオーバー
            sceneManager.Add(Scene.Ending, new Ending(sound));
            sceneManager.Add(Scene.Load, new Load(sound));//ロード

            //最初のシーンへ変更
            sceneManager.Change(Scene.Load);//ロード


            // この上にロジックを記述
            base.Initialize();// 親クラスの初期化処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// コンテンツデータ（リソースデータ）の読み込み処理
        /// （起動時、１度だけ呼ばれる）
        /// </summary>
        protected override void LoadContent()
        {
            // 画像を描画するために、スプライトバッチオブジェクトの実体生成
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // この下にロジックを記述

            // 絵の読み込み
            //renderer.LoadTexture("Bst");
            //renderer.LoadTexture("titlebg");        // タイトル背景
            //renderer.LoadTexture("overtext");       // ゲームオーバー文字
            //renderer.LoadTexture("titletext");      // タイトル文字
            //renderer.LoadTexture("Start");          // ゲームスタート文字
            //renderer.LoadTexture("Stage");            //ステージ
            //renderer.LoadTexture("load");

            // この上にロジックを記述
        }

        /// <summary>
        /// コンテンツの解放処理
        /// （コンテンツ管理者以外で読み込んだコンテンツデータを解放）
        /// </summary>
        protected override void UnloadContent()
        {
            // この下にロジックを記述

            renderer.Unload();//レンダラー
            sound.Unload();//サウンド

            // この上にロジックを記述
        }

        /// <summary>
        /// 更新処理
        /// （1/60秒の１フレーム分の更新内容を記述。音再生はここで行う）
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Update(GameTime gameTime)
        {
            // ゲーム終了処理（ゲームパッドのBackボタンかキーボードのエスケープボタンが押されたら終了）
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                 (Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }

            // この下に更新ロジックを記述

            sceneManager.Update();

            // この上にロジックを記述
            base.Update(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="gameTime">現在のゲーム時間を提供するオブジェクト</param>
        protected override void Draw(GameTime gameTime)
        {
            // 画面クリア時の色を設定
            GraphicsDevice.Clear(Color.Blue);

            // この下に描画ロジックを記述
            renderer.Begin();//表示の開始
            //renderer.DrawTexture("Bst", Vector2.Zero, 1);
            sceneManager.Draw(renderer);
            renderer.End();//表示の終了

            //この上にロジックを記述
            base.Draw(gameTime); // 親クラスの更新処理呼び出し。絶対に消すな！！
        }
    }
}
