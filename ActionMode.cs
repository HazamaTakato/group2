using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGame
{
    /// <summary>
    /// アクションモード
    /// </summary>
    public enum ActionMode
    {
        Stand,  // 立ち（待機）
        Move,   // 移動
        Jump,   //ジャンプ
        Attack, //攻撃
        JumpAttack,  //ジャンプ攻撃
        Damage, //ダメージ
        Stun,//スタン
    }
}
