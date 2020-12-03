using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace ActionGame
{
    class ArrowAttack : Character
    {
        private Vector2 velocity;
        private ShotType type;
        private Player player;
        
        
        public ArrowAttack(string name, Vector2 position, Vector2 velocity, ShotType type) : base("enemytama")
        {
            this.name = name;
            this.position = position;
            this.velocity = velocity;
            this.type = type;

            Initialize();
        }

        

        public override void Hit(Player player)
        {
            isDeadFlag = true;
        }

        public override void Initialize()
        {
            //死亡していないと設定
            isDeadFlag = false;
        }

        public override void Update()
        {
            //移動
            position += velocity;

            //画面外に出たら
            if(position.X < - 32 || Screen.Width < position.X ||
                position.Y < - 16 || Screen.Height < position.Y)
            {
                //生存フラグオフ
                isDeadFlag = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            Color color = Color.White;
            renderer.DrawTexture("enemytama", new Vector2(position.X + 37, position.Y + 16), color);
        }

        public override Rectangle GetRectangle()
        {
            return new Rectangle(
                (int)position.X, (int)position.Y,
                32, 32);
        }

        public override void Shutdown()
        {
            
        }

        
    }
}
