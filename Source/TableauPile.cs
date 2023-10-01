using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Project_Solitaire
{
    internal class TableauPile : Pile
    {
        private bool check;
        private int limit;
        private int top;
        public TableauPile(int cap,int index) : base(cap,index)
        {
            this.limit = cap;
            this.top = this.limit - 1;
            this.check = true;
        }
        public void Copy(TableauPile t)
        {
            this.check = t.check;
            this.limit = t.limit;
            this.top = t.top;
            for(int i=0;i<t.pile.Count;i++)
            {
                this.pile.Add(new Card(t.pile[i].GetRank(), t.pile[i].GetColor(), t.pile[i].GetSym()));
                this.pile[i].Copy(t.pile[i]);
            }
        }
        public void SetCards()
        {
            for(int i=0;i<this.pile.Count;i++)
            {
                this.pile[i].SetIndex(new Vector2(this.index, i));
                this.pile[i].SetPosition();
            }
        }
        public override void AddCards(ref List<Card> TBA)
        {
            if (this.pile.Count == this.limit)
                this.check = false;
            if (this.check)
                base.AddCards(ref TBA);
            else if(IsLegalAdd(ref TBA))
                base.AddCards(ref TBA);
            this.top = this.pile.Count -1;
        }
        public override List<Card> RemoveCards(int index)
        {
            List<Card> res = new List<Card>();
            if(IsLegalRemove(index))
                res =  base.RemoveCards(index);
            this.top = this.pile.Count - 1;
            return res;
        }
        protected override bool IsLegalAdd(ref List<Card> TBA)
        {
            if (this.pile.Count == 0)
                return TBA[0].GetRank() == 13;
            else
                return this.pile[top].GetFaceUp() == false || TBA[0].GetRank() == this.pile[this.pile.Count - 1].GetRank() - 1 && TBA[0].GetColor() != this.pile[this.pile.Count - 1].GetColor();
        }
        protected override bool IsLegalRemove(int index)
        {
            if (this.pile[index].GetFaceUp() == false)
                return false;
            int rank = this.pile[index].GetRank();
            for(int i = index + 1;i<this.pile.Count;i++)
            {
                if (this.pile[i].GetRank() != rank - 1)
                    return false;
                rank = this.pile[i].GetRank(); 
            }
            return true;
        }
        public Card GetTopCard()
        {
            return this.pile[this.top];
        }
        public int GetLimit()
        {
            return this.limit;
        }
    }
}
