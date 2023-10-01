using SharpDX.MediaFoundation.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Project_Solitaire
{
    internal class Global
    {
        static public Vector2 CardArea;
        static public Vector2 CardH_W;
        static public int level;
        static public void Initialize(Vector2 cardArea,Vector2 cardH_W)
        {
            CardArea = cardArea;
            CardH_W = cardH_W;
        }
    }
}
