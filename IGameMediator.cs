using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionGame
{
    interface IGameMediator
    {
        //キャラクターを追加
        void AddActor(Character character);
    }
}
