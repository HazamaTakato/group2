using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGame
{
    interface IScene
    {
        // 初期化
        void Initialize();

        // 更新
        void Update();

        // 表示
        void Draw(Renderer renderer);

        // 終了か？
        bool IsEnd();

        // 次のシーンへ
        Scene Next();
    }
}
