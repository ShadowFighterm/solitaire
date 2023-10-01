using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Solitaire
{
    internal class Card
    {
        private Texture2D Up;
        private Texture2D Down;
        private Vector2 pos;
        private Vector2 index;
        private int rank;
        private string clr;
        private char sym;
        private bool FaceUp;
        public Card(int rank, string clr, char sym)
        {
            this.rank = rank;
            this.clr = clr;
            this.sym = sym;
            FaceUp = false;
        }
        public void Copy(Card c)
        {
            this.Up = c.Up;
            this.Down = c.Down;
            this.pos = new Vector2(c.pos.X, c.pos.Y);
            this.index = new Vector2(c.index.X, c.index.Y);
            this.rank = c.rank;
            this.clr = c.clr;
            this.sym = c.sym;
            this.FaceUp = c.FaceUp;
        }
        public void LoadContent(ref List<Texture2D> textures, Vector2 i)
        {
            this.index = i;
            this.pos.X = (int)((Global.CardArea.X / 8) + (this.index.X * Global.CardArea.X)); this.pos.Y = (int)((this.index.Y * Global.CardArea.Y) + Global.CardArea.X);
            int index = 0;
            switch (this.GetRank())
            {
                case 1:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 0;
                            break;
                        case 's':
                            index = 13;
                            break;
                        case 'd':
                            index = 26;
                            break;
                        case 'c':
                            index = 39;
                            break;
                    }
                    break;
                case 2:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 1;
                            break;
                        case 's':
                            index = 14;
                            break;
                        case 'd':
                            index = 27;
                            break;
                        case 'c':
                            index = 40;
                            break;
                    }
                    break;
                case 3:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 2;
                            break;
                        case 's':
                            index = 15;
                            break;
                        case 'd':
                            index = 28;
                            break;
                        case 'c':
                            index = 41;
                            break;
                    }
                    break;
                case 4:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 3;
                            break;
                        case 's':
                            index = 16;
                            break;
                        case 'd':
                            index = 29;
                            break;
                        case 'c':
                            index = 42;
                            break;
                    }
                    break;
                case 5:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 4;
                            break;
                        case 's':
                            index = 17;
                            break;
                        case 'd':
                            index = 30;
                            break;
                        case 'c':
                            index = 43;
                            break;
                    }
                    break;
                case 6:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 5;
                            break;
                        case 's':
                            index = 18;
                            break;
                        case 'd':
                            index = 31;
                            break;
                        case 'c':
                            index = 44;
                            break;
                    }
                    break;
                case 7:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 6;
                            break;
                        case 's':
                            index = 19;
                            break;
                        case 'd':
                            index = 32;
                            break;
                        case 'c':
                            index = 45;
                            break;
                    }
                    break;
                case 8:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 7;
                            break;
                        case 's':
                            index = 20;
                            break;
                        case 'd':
                            index = 33;
                            break;
                        case 'c':
                            index = 46;
                            break;
                    }
                    break;
                case 9:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 8;
                            break;
                        case 's':
                            index = 21;
                            break;
                        case 'd':
                            index = 34;
                            break;
                        case 'c':
                            index = 47;
                            break;
                    }
                    break;
                case 10:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 9;
                            break;
                        case 's':
                            index = 22;
                            break;
                        case 'd':
                            index = 35;
                            break;
                        case 'c':
                            index = 48;
                            break;
                    }
                    break;
                case 11:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 10;
                            break;
                        case 's':
                            index = 23;
                            break;
                        case 'd':
                            index = 36;
                            break;
                        case 'c':
                            index = 49;
                            break;
                    }
                    break;
                case 12:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 11;
                            break;
                        case 's':
                            index = 24;
                            break;
                        case 'd':
                            index = 37;
                            break;
                        case 'c':
                            index = 50;
                            break;
                    }
                    break;
                case 13:
                    switch (this.GetSym())
                    {
                        case 'h':
                            index = 12;
                            break;
                        case 's':
                            index = 25;
                            break;
                        case 'd':
                            index = 38;
                            break;
                        case 'c':
                            index = 51;
                            break;
                    }
                    break;

            }
            this.Up = textures[index];
            this.Down = textures[52];
        }
        public void Draw(ref SpriteBatch _spriteBatch)
        {
            if (this.FaceUp)
                _spriteBatch.Draw(this.Up, new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)Global.CardH_W.X, (int)Global.CardH_W.Y), Color.White);
            else
                _spriteBatch.Draw(this.Down, new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)Global.CardH_W.X, (int)Global.CardH_W.Y), Color.White);
        }
        public void SetFaceUp(bool b)
        {
            this.FaceUp = b;
        }
        public void UpdatePosition(Vector2 pos, int iy)
        {
            this.pos = pos;
            this.pos.Y = pos.Y + (iy * Global.CardArea.Y);
        }
        public void UpdatePosition(Vector2 pos)
        {
            this.pos = pos;
        }
        public void SetPosition()
        {
            this.pos.X = (int)((Global.CardArea.X / 8) + (this.index.X * Global.CardArea.X));
            if (index.X == 8)
                this.pos.Y = (int)((index.Y * Global.CardArea.X) + Global.CardArea.X);
            else if (index.X == 0 && index.Y != -1)
                this.pos.Y = (index.Y * Global.CardArea.Y) + Global.CardArea.X * 3;
            else if (index.Y == -1)
                this.pos.Y = Global.CardArea.X;
            else
                this.pos.Y = (int)((this.index.Y * Global.CardArea.Y) + Global.CardArea.X);
        }
        public void SetIndex(Vector2 index)
        {
            this.index = index;
        }
        public int GetRank()
        {
            return this.rank;
        }
        public char GetSym()
        {
            return this.sym;
        }
        public string GetColor()
        {
            return this.clr;
        }
        public bool GetFaceUp()
        {
            return this.FaceUp;
        }

    }
}
