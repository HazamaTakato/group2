using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGame
{
    static class StageMap
    {
        public const int XMax = 20;  // 横の最大数
        public const int YMax = 11;  // 縦の最大数
        public const int BlockSize = 64;  // ブロックの大きさ

        public const int Width = StageMap.XMax * StageMap.BlockSize; // マップの幅
    }
}
