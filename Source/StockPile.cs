using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Project_Solitaire
{
    internal class StockPile:Pile
    {
        private List<Card> show;
        private Vector2 spos;
        private bool check;
        private int limit;
        public StockPile(int cap, int index) : base(cap, index)
        {
            this.spos = new Vector2(Global.CardArea.X / 8,Global.CardArea.X * 3);
            this.limit = cap;
            this.show = new List<Card>();
            check = true;
        }
        public void Copy(StockPile s)
        {
            this.check = s.check;
            this.limit = s.limit;
            this.spos = s.spos;
            for (int i = 0; i < s.pile.Count; i++)
            {
                this.pile.Add(new Card(s.pile[i].GetRank(), s.pile[i].GetColor(), s.pile[i].GetSym()));
                this.pile[i].Copy(s.pile[i]);
            }
            for(int i=0;i<s.show.Count;i++)
            {
                this.show.Add(new Card(s.show[i].GetRank(), s.show[i].GetColor(), s.show[i].GetSym()));
                this.show[i].Copy(s.show[i]);
            }
        }
        public void SetCards()
        {
            int index;
            int size;
            if (this.show.Count < Global.level)
            {
                size = this.show.Count;
                index = 0;
            }
            else
            {
                size = Global.level;
                index = this.show.Count - Global.level;
            }
            for (int j = 0; j < size; j++)
            {
                this.show[index + j].SetIndex(new Vector2(0, j));
                this.show[index + j].SetPosition();
            }
        }
        public override void AddCards(ref List<Card> TBA)
        {
            if(this.check)
            {
                base.AddCards(ref TBA);
                if (this.pile.Count == this.limit)
                    this.check = false;
            }
            else if (IsLegalAdd(ref TBA))
            {
                this.show.Add(TBA[0]);
                TBA.RemoveAt(0);
            }
        }
        public override List<Card> RemoveCards(int index)
        {
            int RealIndex;
            if (this.show.Count < Global.level)
                RealIndex = index;
            else
                RealIndex = this.show.Count - (Global.level - index);
            List<Card> TBR = new List<Card>();
            if (IsLegalRemove(RealIndex))
            {
                TBR.Add(this.show[RealIndex]);
                this.show.RemoveAt(RealIndex);
            }
            return TBR;
        }
        protected override bool IsLegalAdd(ref List<Card> TBA)
        {
            return TBA.Count == 1;
        }
        protected override bool IsLegalRemove(int index)
        {
            if (show.Count == 0 || index != show.Count - 1)
                return false;
            return true;
        }
        private void DisplayCard(ref SpriteBatch _spriteBatch)
        {
            for (int i = this.show.Count - 1; i >= 0; i--)
            {
                this.show[i].Draw(ref _spriteBatch);
            }
        }
        public void OnClick()
        {
            if(this.pile.Count == 0)
            {
                Vector2 index;
                for (int i = this.show.Count - 1; i >= 0; i--) 
                {
                    index.X = 0; index.Y = -1;
                    this.pile.Add(this.show[i]);
                    this.pile[this.show.Count - 1 - i].SetIndex(index);
                    this.pile[this.show.Count - 1 - i].SetPosition();
                    this.pile[this.show.Count - 1 - i].SetFaceUp(false);
                }
                this.show.Clear();
            }
            else
            {
                int size;
                if (this.pile.Count < Global.level)
                    size = this.pile.Count;
                else
                    size = Global.level;
                Vector2 index;
                for (int i = 0; i < size; i++) 
                {
                    index.X = 0; index.Y = i; 
                    this.show.Add(this.pile[this.pile.Count - 1 - i]);
                    this.pile[this.pile.Count - 1 - i].SetIndex(index);
                    this.pile[this.pile.Count - 1 - i].SetPosition();
                    this.pile[this.pile.Count - 1 - i].SetFaceUp(true);
                }
                int PileSize = this.pile.Count;
                for(int i=0;i<size;i++)
                {
                    this.pile.RemoveAt(PileSize - 1 - i);
                }
            }
        }
        public Vector2 GetSPos()
        {
            return this.spos;
        }

        public int GetShowSize()
        {
            return this.show.Count;
        }
        public Card GetShowCardAt(int index)
        {
            return this.show[index];
        }
        public int GetLimit()
        {
            return this.limit;
        }
    }
}
