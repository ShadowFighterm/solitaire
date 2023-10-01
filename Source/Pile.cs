using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Solitaire
{
    internal abstract class Pile
    {
        protected List<Card> pile;
        protected int index;
        public Pile(int cap, int index)
        {
            this.pile = new List<Card>(cap);
            this.index = index;
        }
        public virtual void AddCards(ref List<Card> TBA)
        {
            for(int i=0;i<TBA.Count;i++)
            {
                this.pile.Add(TBA[i]);
            }
            TBA.Clear();
        }
        public virtual List<Card> RemoveCards(int index)
        {
            int cap = this.pile.Count - index;
            int li = this.pile.Count - 1;
            List<Card> TBR = new List<Card>(cap);
            for(int i = index;i<this.pile.Count;i++)
            {
                TBR.Add(this.pile[i]);
            }
            while (li >= index)
            {
                this.pile.RemoveAt(li);
                li--;
            }
            return TBR;
        }
        protected abstract bool IsLegalAdd(ref List<Card> TBA);
        protected abstract bool IsLegalRemove(int index);
        public Card this[int index]
        {
            get
            {
                return this.pile[index];
            }
        }
        public int GetSize()
        {
            return this.pile.Count;
        }
        public int GetIndex()
        {
            return this.index;
        }
        public int GetKingIndex()
        {
            for(int i=0;i<this.pile.Count;i++)
            {
                if (this.pile[i].GetRank() == 13)
                    return i;
            }
            return -1;
        }
        public virtual void Clear()
        {
            this.pile.Clear();
        }
    }
}
