using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ActionGame
{
    class Stage
    {
        //ステージのマップ
        private int[,] mapData = new int[StageMap.YMax, StageMap.XMax];
        private int[,] mapData_Stage3 =
        {
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1 },
            //①{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            //②{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //①と②はコピペ用
        };
        private bool deadFlag;
        private float stageX;//ステージ座標
        /// <summary>
        /// コンストラクタ（生成時に自動的に呼び出される）
        /// </summary>
        public Stage()
        {

        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            stageX = 0; // スクロール位置は左
            // 縦横にループして、コピーする
            for (int y = 0; y < StageMap.YMax; y++)
            {
                for (int x=0;x<StageMap.XMax;x++)
                {
                    // 内容をコピーする
                    mapData[y, x] = mapData_Stage3[y, x];
                }
            }

        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            // マップでのブロック表示
            renderer.DrawTexture("Bst", new Vector2(0, 0));
            //変数の宣言
            int ct;//表示する地形番号
            int px, py;//表示する座標
            int tx, ty;//切り出す座標
            // 縦横の順で２重ループ
            for (int y = 0; y < StageMap.YMax; y++)
            {
                for (int x = 0; x < StageMap.XMax; x++)
                {
                    // マップからキャラクター（地形）番号を取り出す
                    // 縦、横の順番で指定
                    ct = mapData[y, x];
                    //0以外で描画する
                    if (ct != 0)
                    {
                        // 表示座標を計算する
                        px = x * StageMap.BlockSize;
                        py = y * StageMap.BlockSize;
                        //絵は８ブロックで１行なのでｘｙの切り出し位置を算出する
                        tx = ct % 8;// 余りでｘ位置を算出
                        ty = ct / 8;// 商でY位置を算出（切捨て）
                        //数値にブロックサイズを掛けて、切り出し位置座標を算出する
                        tx *= StageMap.BlockSize;
                        ty *= StageMap.BlockSize;
                        // １ブロックを表示する
                        renderer.DrawTexture("Block", new Vector2(GetScreenX(px),py),
                            new Rectangle(tx, ty, StageMap.BlockSize, StageMap.BlockSize));
                    }
                }
            }
        }
        public bool CollisionPoint(Vector2 position)
        {
            int bx, by;

            // 座標をブロックのサイズで割って、どのブロックかを求める
            bx = (int)(position.X / StageMap.BlockSize);
            by = (int)(position.Y / StageMap.BlockSize);

            // マップの上下は自由移動
            if (by < 0 || by >= StageMap.YMax)
            {
                return true;   // 移動できる
            }

            // マップの右左は移動不可
            if (position.X < 0 || bx >= StageMap.XMax)
            {
                return false;   // 移動できない
            }

            // 0なら移動できる
            if (mapData[by, bx] == 0)
            {
                return true;   // 移動できる
            }
            // 0以外なら移動できない
            else
            {
                return false;  // 移動できない
            }
        }
        /// <summary>
        /// 横方向のブロックとの衝突判定
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool CollisionSide(Vector2 position, float size)
        {
            // Ｙを１づづ増やしながら、上から下へ判定する
            for (int i = 0; i < size; i++)
            {
                // その座標にブロックがあれば
                if (CollisionPoint(position) == false)
                {
                    return false; // 移動できない
                }
                position.Y++;
            }
            return true;   // 移動できる
        }
        /// <summary>
        /// 縦方向のブロックとの衝突判定
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool CollisionUpDown(Vector2 position, float size)
        {
            // Ｘを１づづ増やしながら、上から下へ判定する
            for (int i = 0; i < size; i++)
            {
                // その座標にブロックがあれば
                if (CollisionPoint(position) == false)
                {
                    return false;// 移動できない
                }
                position.X++;
            }
            return true;// 移動できる
        }
        /// <summary>
        /// 左へのスクロール
        /// </summary>
        public void ScrollLeft()
        {
            // 画面端でなければ（スクロールできる範囲ならば）
            if (stageX < StageMap.Width - Screen.Width)
            {
                stageX++;       // 左へスクロール
            }
        }
        /// <summary>
        /// 右へのスクロール
        /// </summary>
        public void ScrollRight()
        {
            // 画面端でなければ（スクロールできる範囲ならば）
            if (stageX > Screen.Width - StageMap.Width)
            {
                stageX--;// 右へスクロール
            }
        }
        /// <summary>
        /// マップ上の座標から画面上の座標への変換
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float GetScreenX(float x)
        {
            // マップ上の座標からマップスクロール座標を引き、画面上の座標を出す
            x -= stageX;
            return x;
        }
        /// <summary>
        /// マップ上の座標から画面上の座標への変換
        /// （Vector2形式）
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 GetScreenPosition(Vector2 position)
        {
            // マップ上の座標からマップスクロール座標を引き、画面上の座標を出す
            position.X -= stageX;
            return position;
        }

    }
}
