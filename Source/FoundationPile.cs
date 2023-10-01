using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Project_Solitaire
{
    internal class FoundationPile : Pile
    {
        private Texture2D sprite;
        private Vector2 pos;
        private int limit;
        private char sym;
        public FoundationPile(int cap,int index):base(cap,index)
        {
            this.limit = cap;
        }
        public void Copy(FoundationPile f)
        {
            this.sprite = f.sprite;
            this.limit = f.limit;
            this.pos = f.pos;
            for (int i = 0; i < f.pile.Count; i++)
            {
                this.pile.Add(new Card(f.pile[i].GetRank(), f.pile[i].GetColor(), f.pile[i].GetSym()));
                this.pile[i].Copy(f.pile[i]);
            }
        }
        public void SetCard()
        {
            if (this.pile.Count != 0)
            {
                Vector2 index;
                index.X = this.index;
                index.Y = (pos.Y - Global.CardArea.X) / Global.CardArea.X;
                this.pile[this.pile.Count - 1].SetIndex(index);
                this.pile[this.pile.Count - 1].SetPosition();
            }
        }
        public override void AddCards(ref List<Card> TBA)
        {
            if (IsLegalAdd(ref TBA))
            {
                base.AddCards(ref TBA);
                this.sym = this.pile[0].GetSym();
            }
        }
        public override List<Card> RemoveCards(int index)
        {
            if (IsLegalRemove(index))
                return base.RemoveCards(index);
            return new List<Card>();
        }
        protected override bool IsLegalAdd(ref List<Card> TBA)
        {
            if (TBA.Count != 1 || this.pile.Count == 0 && TBA[0].GetRank() != 1 || this.pile.Count != 0 && this.pile[this.pile.Count - 1].GetRank() != TBA[0].GetRank() - 1 || this.pile.Count != 0 && this.pile[this.pile.Count - 1].GetColor() != TBA[0].GetColor() || this.pile.Count !=0 && this.pile[this.pile.Count-1].GetSym() != TBA[0].GetSym())
                return false;
            return true;
        }
        protected override bool IsLegalRemove(int index)
        {
            return this.pile.Count != 0;
        }
        public void Draw(ref SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.sprite, new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)Global.CardH_W.X, (int)Global.CardH_W.Y), Color.White);
        }

        public void LoadContent(ref List<Texture2D> textures, Vector2 index)
        {
            this.pos.X = (int)((Global.CardArea.X / 8) + (index.X * Global.CardArea.X));
            this.pos.Y = (int)((index.Y * Global.CardArea.X) + Global.CardArea.X);
            this.sprite = textures[53];
        }
        public int GetLimit()
        {
            return this.limit;
        }
        public char GetSym()
        {
            return this.sym;
        }
        public int GetTop()
        {
            return this.pile.Count - 1;
        }
    }
}
