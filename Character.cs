using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace ActionGame
{
    abstract class Character
    {
        // フィールド
        // 位置
        protected Vector2 position;
        // 画像の名前
        protected string name;
        // 死亡フラグ
        protected bool isDeadFlag;
        // 仲介者
        protected IGameMediator mediator;

        //protected readonly float GRAVITY = 0.5f;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">画像の名前</param>
        public Character(string name)
        {
            // 引数の受け取り
            this.name = name;
            
            // 位置初期化
            position = Vector2.Zero;
            // 死亡フラグ初期化
            isDeadFlag = false;
        }

        // 抽象メソッド（子クラスで必ず再定義しなければならないメソッド）
        public abstract void Initialize();  // 初期化
        public abstract void Update(); // 更新

        public abstract void Shutdown();    // 終了
        public abstract void Hit(Player player);  // ヒット通知
        public abstract Rectangle GetRectangle();   // 自分のいる範囲取得

        /// <summary>
        /// 死んでいるか？
        /// </summary>
        /// <returns>死んでいたらtrue</returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        /// <summary>
        /// 衝突判定（2点間の距離と円の半径）
        /// </summary>
        /// <param name="other">相手キャラクター</param>
        /// <returns>当たっていたらtrue</returns>
        public bool IsCollision(Character other)
        {
            if (GetRectangle().Intersects(other.GetRectangle()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 位置の受け渡し
        /// </summary>
        /// <param name="other">位置を入れたい変数</param>
        public void SetPosition(ref Vector2 other)
        {
            other = position;
        }


        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
