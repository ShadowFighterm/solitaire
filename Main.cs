using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using var game = new Project_Solitaire.Main();
game.Run();

namespace Project_Solitaire
{
    struct Storage
    {
        public int score;
        public List<TableauPile> TPiles;
        public List<FoundationPile> FPiles;
        public List<StockPile> SPile;
        public Storage(int s,List<TableauPile>tp,List<FoundationPile>fp,List<StockPile>sp)
        {
            this.score = 0;
            this.TPiles = new List<TableauPile>();
            this.FPiles = new List<FoundationPile>();
            this.SPile = new List<StockPile>();
            Copy(s, tp, fp, sp);
        }
        public void Copy(int s,List<TableauPile>tp,List<FoundationPile>fp,List<StockPile>sp)
        {
            for(int i=0;i<tp.Count;i++)
            {
                this.TPiles.Add(new TableauPile(0,i+1));
                this.TPiles[i].Copy(tp[i]);
            }
            for(int i=0;i<fp.Count;i++)
            {
                this.FPiles.Add(new FoundationPile(0,8));
                this.FPiles[i].Copy(fp[i]);
            }
            this.SPile.Add(new StockPile(0,0));
            this.SPile[0].Copy(sp[0]);
            this.score = s;
        }
    }
    public class Main : Game
    {
        private int score;
        private string msg;
        private bool IsDragging;
        private bool IsRejected;
        private bool Selected;
        private bool IsShuffling;
        private bool IsPressed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 soc;
        private Vector2 des;
        private Vector2 SocPos;
        private Vector2 DesPos;
        private List<Texture2D> textures;
        private List<Song> music;
        private List<SoundEffect> sound;
        private List<SoundEffectInstance> SoundInstance;
        private List<TableauPile> TPiles;
        private List<FoundationPile> FPiles;
        private List<StockPile> SPile;
        private List<Card> trans;
        private List<Rectangle> OptionsBound;
        private List<Color> OptionsColor;
        private Stack<Storage> undo;
        private Stack<Storage> redo;
        private SpriteFont font;
        private enum GameState { MainMenu,Gameplay};
        private GameState CurrentState;
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1300;
            _graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Random random = new Random();
            int num = 52, index, count = 0;
            List<Card> AllCards = new List<Card>(num);
            this.score = 0;
            this.msg = "Score:";
            this.IsDragging = false;
            this.IsRejected = false;
            this.Selected = false;
            this.IsShuffling = true;
            this.IsPressed = false;
            MediaPlayer.IsRepeating = true;
            this.CurrentState = GameState.MainMenu;
            this.TPiles = new List<TableauPile>(7);
            this.FPiles = new List<FoundationPile>(4);
            this.SPile = new List<StockPile>();
            this.trans = new List<Card>();
            this.textures = new List<Texture2D>(60);
            this.music = new List<Song>(3);
            this.sound = new List<SoundEffect>(5);
            this.SoundInstance = new List<SoundEffectInstance>(5);
            this.OptionsBound = new List<Rectangle>(3);
            this.OptionsColor = new List<Color>(3);
            this.undo = new Stack<Storage>();
            this.redo = new Stack<Storage>();
            this.soc = new Vector2(0, 0);
            this.des = new Vector2(0, 0);
            this.SocPos = new Vector2(0, 0);
            this.DesPos = new Vector2(0, 0);
            int width = (3 * (GraphicsDevice.Viewport.Width/9))/2;
            int height = width/4;
            int XPos = GraphicsDevice.Viewport.Width / 2 - (width / 2);
            int YPos = GraphicsDevice.Viewport.Height / 2 - ((7 * height) / 4);
            this.OptionsBound.Add(new Rectangle(XPos, YPos + (0 * (3 * height / 2)), width, height));
            this.OptionsBound.Add(new Rectangle(XPos, YPos + (1 * (3 * height / 2)), width, height));
            this.OptionsBound.Add(new Rectangle(XPos, YPos + (2 * (3 * height / 2)), width, height));
            this.OptionsColor.Add(Color.White);this.OptionsColor.Add(Color.White); this.OptionsColor.Add(Color.White);
            for (int i = 0; i < 4; i++)
            {
                this.FPiles.Add(new FoundationPile(13,8));
            }
            for (int i = 0; i < num; i++)
            {
                if (i <= 12)
                    AllCards.Add(new Card(i + 1, "red", 'h'));
                else if (i <= 25)
                    AllCards.Add(new Card((i - 13) + 1, "black", 's'));
                else if (i <= 38)
                    AllCards.Add(new Card((i - 26) + 1, "red", 'd'));
                else
                    AllCards.Add(new Card((i - 39) + 1, "black", 'c'));
            }
            while (AllCards.Count > 0)
            {
                index = random.Next(num - count);
                if (count == 0)
                {
                    if (this.TPiles.Count == 0)
                        this.TPiles.Add(new TableauPile(1,1));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[0].AddCards(ref this.trans);
                }
                else if (count <= 2)
                {
                    if (this.TPiles.Count == 1)
                        this.TPiles.Add(new TableauPile(2,2));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[1].AddCards(ref this.trans);
                }
                else if (count <= 5)
                {
                    if (this.TPiles.Count == 2)
                        this.TPiles.Add(new TableauPile(3,3));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[2].AddCards(ref this.trans);
                }
                else if (count <= 9)
                {
                    if (this.TPiles.Count == 3)
                        this.TPiles.Add(new TableauPile(4,4));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[3].AddCards(ref this.trans);
                }
                else if (count <= 14)
                {
                    if (this.TPiles.Count == 4)
                        this.TPiles.Add(new TableauPile(5, 5));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[4].AddCards(ref this.trans);
                }
                else if (count <= 20)
                {
                    if (this.TPiles.Count == 5)
                        this.TPiles.Add(new TableauPile(6, 6));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[5].AddCards(ref this.trans);
                }
                else if (count <= 27)
                {
                    if (this.TPiles.Count == 6)
                        this.TPiles.Add(new TableauPile(7, 7));
                    this.trans.Add(AllCards[index]);
                    this.TPiles[6].AddCards(ref this.trans);
                }
                else
                {
                    if (this.SPile.Count == 0)
                        this.SPile.Add(new StockPile(24,0));
                    this.trans.Add(AllCards[index]);
                    this.SPile[0].AddCards(ref this.trans);
                }
                AllCards.RemoveAt(index);
                count++;
            }
            base.Initialize();
        }

        private void LoadContentCard()
        {
            Vector2 index;
            Vector2 CardArea, CardH_W;
            CardArea.X = (int)(GraphicsDevice.Viewport.Width / 9); CardArea.Y = (int)(GraphicsDevice.Viewport.Height / 30);
            CardH_W.X = (int)((3 * CardArea.X / 4)); CardH_W.Y = (int)((5 * CardH_W.X) / 4);
            Global.Initialize(CardArea, CardH_W);
            for (int i = 0; i < this.TPiles.Count; i++) 
            {
                this.TPiles[i].GetTopCard().SetFaceUp(true);
                for(int j = 0; j < this.TPiles[i].GetSize();j++)
                {
                    index.X = i + 1;index.Y = j;
                    this.TPiles[i][j].LoadContent(ref this.textures, index);
                }
            }
            for (int i = 0; i < SPile.Count; i++) 
            {
                for(int j=0;j< this.SPile[i].GetSize();j++)
                {
                    index.X = 0;index.Y = 0;
                    this.SPile[i][j].LoadContent(ref this.textures, index);
                }
            }
            index.X = 8;
            for(int i = 0; i < this.FPiles.Count;i++)
            {
                index.Y = i;
                this.FPiles[i].LoadContent(ref this.textures, index);
            }
            this.undo.Push(new Storage(this.score, this.TPiles, this.FPiles, this.SPile));
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.font = Content.Load<SpriteFont>("cyborg_punk");

            this.music.Add(Content.Load<Song>("music"));
            this.music.Add(Content.Load<Song>("shuffle"));

            this.sound.Add(Content.Load<SoundEffect>("click"));
            this.sound.Add(Content.Load<SoundEffect>("card_place"));
            this.sound.Add(Content.Load<SoundEffect>("error"));

            this.SoundInstance.Add(this.sound[0].CreateInstance());
            this.SoundInstance.Add(this.sound[1].CreateInstance());
            this.SoundInstance.Add(this.sound[2].CreateInstance());

            this.textures.Add(Content.Load<Texture2D>("ace_heart"));
            this.textures.Add(Content.Load<Texture2D>("two_heart"));
            this.textures.Add(Content.Load<Texture2D>("three_heart"));
            this.textures.Add(Content.Load<Texture2D>("four_heart"));
            this.textures.Add(Content.Load<Texture2D>("five_heart"));
            this.textures.Add(Content.Load<Texture2D>("six_heart"));
            this.textures.Add(Content.Load<Texture2D>("seven_heart"));
            this.textures.Add(Content.Load<Texture2D>("eight_heart"));
            this.textures.Add(Content.Load<Texture2D>("nine_heart"));
            this.textures.Add(Content.Load<Texture2D>("ten_heart"));
            this.textures.Add(Content.Load<Texture2D>("jack_heart"));
            this.textures.Add(Content.Load<Texture2D>("queen_heart"));
            this.textures.Add(Content.Load<Texture2D>("king_heart"));
            // 12
            this.textures.Add(Content.Load<Texture2D>("ace_spade"));
            this.textures.Add(Content.Load<Texture2D>("two_spade"));
            this.textures.Add(Content.Load<Texture2D>("three_spade"));
            this.textures.Add(Content.Load<Texture2D>("four_spade"));
            this.textures.Add(Content.Load<Texture2D>("five_spade"));
            this.textures.Add(Content.Load<Texture2D>("six_spade"));
            this.textures.Add(Content.Load<Texture2D>("seven_spade"));
            this.textures.Add(Content.Load<Texture2D>("eight_spade"));
            this.textures.Add(Content.Load<Texture2D>("nine_spade"));
            this.textures.Add(Content.Load<Texture2D>("ten_spade"));
            this.textures.Add(Content.Load<Texture2D>("jack_spade"));
            this.textures.Add(Content.Load<Texture2D>("queen_spade"));
            this.textures.Add(Content.Load<Texture2D>("king_spade"));
            //25
            this.textures.Add(Content.Load<Texture2D>("ace_diamond"));
            this.textures.Add(Content.Load<Texture2D>("two_diamond"));
            this.textures.Add(Content.Load<Texture2D>("three_diamond"));
            this.textures.Add(Content.Load<Texture2D>("four_diamond"));
            this.textures.Add(Content.Load<Texture2D>("five_diamond"));
            this.textures.Add(Content.Load<Texture2D>("six_diamond"));
            this.textures.Add(Content.Load<Texture2D>("seven_diamond"));
            this.textures.Add(Content.Load<Texture2D>("eight_diamond"));
            this.textures.Add(Content.Load<Texture2D>("nine_diamond"));
            this.textures.Add(Content.Load<Texture2D>("ten_diamond"));
            this.textures.Add(Content.Load<Texture2D>("jack_diamond"));
            this.textures.Add(Content.Load<Texture2D>("queen_diamond"));
            this.textures.Add(Content.Load<Texture2D>("king_diamond"));
            //38
            this.textures.Add(Content.Load<Texture2D>("ace_club"));
            this.textures.Add(Content.Load<Texture2D>("two_club"));
            this.textures.Add(Content.Load<Texture2D>("three_club"));
            this.textures.Add(Content.Load<Texture2D>("four_club"));
            this.textures.Add(Content.Load<Texture2D>("five_club"));
            this.textures.Add(Content.Load<Texture2D>("six_club"));
            this.textures.Add(Content.Load<Texture2D>("seven_club"));
            this.textures.Add(Content.Load<Texture2D>("eight_club"));
            this.textures.Add(Content.Load<Texture2D>("nine_club"));
            this.textures.Add(Content.Load<Texture2D>("ten_club"));
            this.textures.Add(Content.Load<Texture2D>("jack_club"));
            this.textures.Add(Content.Load<Texture2D>("queen_club"));
            this.textures.Add(Content.Load<Texture2D>("king_club"));
            //51
            this.textures.Add(Content.Load<Texture2D>("back"));
            this.textures.Add(Content.Load<Texture2D>("foundation_pile"));
            this.textures.Add(Content.Load<Texture2D>("bg"));
            this.textures.Add(Content.Load<Texture2D>("mainmenu_bg"));
            this.textures.Add(Content.Load<Texture2D>("easy"));
            this.textures.Add(Content.Load<Texture2D>("medium"));
            this.textures.Add(Content.Load<Texture2D>("hard"));

            LoadContentCard();
            MediaPlayer.Play(this.music[0]);
            // TODO: use this.Content to load your game content here
        }
        private Vector2 ConvertIntoRealPosition(Vector2 index)
        {
            Vector2 Pos;
            Pos.X = (index.X * Global.CardArea.X) + (Global.CardArea.X / 8);
            if (index.X != 0)
                Pos.Y = (index.Y * Global.CardArea.Y) + Global.CardArea.X;
            else
                Pos.Y = (index.Y * Global.CardArea.Y) + (3 * Global.CardArea.X);
            return Pos;
        }
        private Vector2 ConvertIntoIndex(Vector2 MousePos)
        {
            Vector2 index;
            int pi = (int)(MousePos.X / Global.CardArea.X);
            int minus = (int)((Global.CardArea.X / 8) + (pi * (2 * (Global.CardArea.X / 8))));
            int ix = (int)(MousePos.X - minus);
            int iy = (int)(MousePos.Y - Global.CardArea.X);
            if(iy < 0)
            if (ix < 0 || iy < 0)
                return index = new Vector2(-1, -1);
            ix = (int)(ix / Global.CardH_W.X);
            if (ix != pi)
                return index = new Vector2(-1, -1);
            if(ix == 0)
            {
                int LastRangeS = (int)(Global.CardArea.X + Global.CardH_W.Y);
                if (MousePos.Y < LastRangeS)
                {
                    index.X = ix;
                    index.Y = -1;
                    return index;
                }
                iy = (int)(iy - (2 * Global.CardArea.X));
                int LiS = this.SPile[0].GetShowSize() - 1;
                if (iy < 0 || LiS < 0)
                    return index = new Vector2(-1, -1);
                LiS = Global.level - 1;
                iy = (int)(iy / Global.CardArea.Y);
                LastRangeS = (int)((3 * Global.CardArea.X) + (LiS * Global.CardArea.Y) + Global.CardH_W.Y);
                if (MousePos.Y > LastRangeS)
                    return index = new Vector2(-1, -1);
                if(iy<=LiS)
                {
                    index.Y = iy; index.X = ix;
                    return index;
                }
                index.Y = LiS;
                index.X = ix;
                return index;
            }
            if(ix == 8)
            {
                iy = (int)(iy / Global.CardH_W.Y);
                if (iy > 3)
                    return index = new Vector2(-1, -1);
                index.X = ix;
                index.Y = iy;
                return index;
            }
            int LiT = this.TPiles[ix-1].GetSize() - 1;
            int LastRangeT = (int)(LiT * Global.CardArea.Y + Global.CardArea.X + Global.CardH_W.Y);
            if (MousePos.Y > LastRangeT)
                return index = new Vector2(-1, -1);
            iy = (int)(iy / Global.CardArea.Y);
            if (iy <= LiT)
            {
                index.Y = iy;index.X = ix;
                return index;
            }
            index.Y = LiT;
            index.X = ix;
            return index;

        }
        private int OptionIndexWithPosition(Vector2 MousePos)
        {
            for (int i = 0; i < this.OptionsBound.Count; i++)
            {
                if (this.OptionsBound[i].Contains(MousePos))
                    return i;
            }
            return -1;
        }
        private void ResetColor()
        {
            this.OptionsColor[0] = this.OptionsColor[1] = this.OptionsColor[2] = Color.White;
        }
        private void SaveState()
        {
            while(redo.Count != 0)
            {
                redo.Pop();
            }
            this.undo.Push(new Storage(this.score, this.TPiles, this.FPiles, this.SPile));
        }
        private void Copy(Storage s)
        {
            this.score = s.score;
            for(int i=0;i<this.TPiles.Count;i++)
            {
                this.TPiles[i] = new TableauPile(0,i+1);
                this.TPiles[i].Copy(s.TPiles[i]);
            }
            for(int i=0;i<this.FPiles.Count;i++)
            {
                this.FPiles[i] = new FoundationPile(0, 8);
                this.FPiles[i].Copy(s.FPiles[i]);
            }
            this.SPile[0] = new StockPile(0, 0);
            this.SPile[0].Copy(s.SPile[0]);
                
        }
        private void Undo()
        {
            if (undo.Count != 1)
                redo.Push(undo.Pop());
            Copy(undo.Peek());
        }
        private void Redo()
        {
            if(redo.Count!=0)
            {
                Copy(redo.Peek());
                this.undo.Push(redo.Pop());
            }
        }
        private bool IsWin()
        {
            if (this.SPile[0].GetSize()== 0 && this.SPile[0].GetShowSize()==0)
            {
                for(int i = 0; i < this.TPiles.Count;i++)
                {
                    for(int j = 0; j < this.TPiles[i].GetSize();j++)
                    {
                        if (this.TPiles[i][j].GetFaceUp() == false)
                            return false;
                    }
                }
                for(int i=0;i<this.TPiles.Count;i++)
                {
                    int ki = this.TPiles[i].GetKingIndex();
                    if (ki != -1)
                    {
                        for(int j=0;j<this.FPiles.Count;j++)
                        {
                            if (this.FPiles[j].GetSym() == this.TPiles[i][ki].GetSym())
                            {
                                this.trans.Add(this.TPiles[i][ki]);
                                this.FPiles[j].AddCards(ref this.trans);
                            }
                        }
                    }
                    this.TPiles[i].Clear();
                }
                return true;
            }
            return false;
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();
            Vector2 MousePos = new Vector2(mouse.X, mouse.Y);
            switch(this.CurrentState)
            {
                case GameState.MainMenu:
                    
                    int oi = OptionIndexWithPosition(MousePos);
                    ResetColor();
                    if(mouse.LeftButton == ButtonState.Pressed && oi != -1)
                    {
                        this.CurrentState = GameState.Gameplay;
                        gameTime.TotalGameTime = System.TimeSpan.Zero;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(this.music[1]);
                        Global.level = oi + 1;
                    }
                    if(oi != -1)
                        this.OptionsColor[oi] = Color.RosyBrown;
                    break;
                case GameState.Gameplay:
                    if (!this.IsShuffling)
                    {
                        MediaPlayer.IsRepeating = false;
                        if (mouse.LeftButton == ButtonState.Released)
                            this.Selected = false;
                        if (Keyboard.GetState().IsKeyUp(Keys.U) && Keyboard.GetState().IsKeyUp(Keys.R))
                            this.IsPressed = false;
                        if (this.IsRejected)
                        {
                            int steps = 10;
                            if (DesPos.X < SocPos.X)
                                DesPos.X+=steps;
                            else if (DesPos.X > SocPos.X)
                                DesPos.X-=steps;
                            if (DesPos.Y < SocPos.Y)
                                DesPos.Y += steps;
                            else if (DesPos.Y > SocPos.Y)
                                DesPos.Y -= steps;
                            if(Math.Abs(DesPos.X - SocPos.X) < steps)
                            {
                                while(DesPos.X < SocPos.X)
                                {
                                    DesPos.X++;
                                }
                                while(DesPos.X > SocPos.X)
                                {
                                    DesPos.X--;
                                }

                            }    
                            if(Math.Abs(DesPos.Y - SocPos.Y) < steps)
                            {
                                while (DesPos.Y < SocPos.Y)
                                {
                                    DesPos.Y++;
                                }
                                while (DesPos.Y > SocPos.Y)
                                {
                                    DesPos.Y--;
                                }
                            }
                            for (int i = 0; i < this.trans.Count; i++)
                            {
                                this.trans[i].UpdatePosition(DesPos, i);
                            }
                            if (DesPos.X == SocPos.X && DesPos.Y == SocPos.Y)
                            {
                                this.IsRejected = false;
                                if (soc.X != 0)
                                {
                                    this.TPiles[(int)soc.X - 1].AddCards(ref this.trans);
                                    for (int j = (int)soc.Y; j < this.TPiles[(int)soc.X - 1].GetSize(); j++)
                                    {
                                        this.TPiles[(int)soc.X - 1][j].SetPosition();
                                    }
                                }
                                else
                                    this.SPile[0].AddCards(ref this.trans);
                            }
                        }
                        else if (this.IsDragging)
                        {
                            if (mouse.LeftButton == ButtonState.Released)
                                this.Selected = true;
                            if (mouse.LeftButton == ButtonState.Pressed && this.Selected)
                            {
                                this.IsDragging = false;
                                this.des = ConvertIntoIndex(MousePos);
                                if (this.des.X != 0 && this.des.X != -1)
                                {
                                    if (this.des.X != 8)
                                    {
                                        this.TPiles[(int)this.des.X - 1].AddCards(ref this.trans);
                                        if (this.trans.Count != 0)
                                        {
                                            this.IsRejected = true;
                                            this.SocPos = ConvertIntoRealPosition(this.soc);
                                            this.DesPos = MousePos;
                                            this.SoundInstance[2].Play();
                                        }
                                        else
                                        {
                                            this.SoundInstance[1].Play();
                                            if (soc.X != 0 && soc.X != 8 && this.TPiles[(int)this.soc.X - 1].GetSize() != 0)
                                                this.TPiles[(int)this.soc.X - 1].GetTopCard().SetFaceUp(true);
                                            if (this.soc.X != this.des.X)
                                            {
                                                this.score++;
                                                SaveState();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.FPiles[(int)this.des.Y].AddCards(ref this.trans);
                                        if (this.trans.Count != 0)
                                        {
                                            this.IsRejected = true;
                                            this.SocPos = ConvertIntoRealPosition(this.soc);
                                            this.DesPos = MousePos;
                                            this.SoundInstance[2].Play();
                                        }
                                        else
                                        {
                                            this.SoundInstance[1].Play();
                                            if (this.soc.X != 0  && this.soc.X != 8 && this.TPiles[(int)this.soc.X - 1].GetSize() != 0)
                                                this.TPiles[(int)this.soc.X - 1][this.TPiles[(int)this.soc.X - 1].GetSize() - 1].SetFaceUp(true);
                                            if (this.soc.X != this.des.X)
                                            {
                                                this.score++;
                                                SaveState();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    this.SoundInstance[2].Play();
                                    this.IsRejected = true;
                                    this.SocPos = ConvertIntoRealPosition(this.soc);
                                    this.DesPos = MousePos;
                                }
                            }
                            if (this.Selected)
                            {
                                for (int i = 0; i < this.trans.Count; i++)
                                {
                                    this.trans[i].UpdatePosition(MousePos, i);
                                }
                            }
                        }
                        else if (!this.Selected && mouse.LeftButton == ButtonState.Pressed)
                        {
                            this.soc = ConvertIntoIndex(MousePos);
                            if (this.soc.X != -1)
                            {
                                if (soc.Y == -1)
                                {
                                    this.SPile[0].OnClick();
                                    SaveState();
                                    this.Selected = true;
                                    this.trans.Clear();
                                    this.SoundInstance[0].Play();
                                }
                                else if (this.soc.X == 0)
                                {
                                    this.trans = this.SPile[(int)this.soc.X].RemoveCards((int)this.soc.Y);
                                }
                                else if(this.soc.X == 8)
                                {
                                    this.trans = this.FPiles[(int)this.soc.Y].RemoveCards((int)this.FPiles[(int)this.soc.Y].GetTop());
                                }
                                else
                                {
                                    this.trans = this.TPiles[(int)this.soc.X - 1].RemoveCards((int)this.soc.Y);
                                }
                                if (this.trans.Count != 0)
                                {
                                    this.IsDragging = true;
                                    this.SoundInstance[0].Play();
                                }
                            }
                        }
                        else if (!this.IsPressed && Keyboard.GetState().IsKeyDown(Keys.U))
                        {
                            Undo();
                            this.IsPressed = true;
                            this.SoundInstance[1].Play();
                        }
                        else if(!this.IsPressed && Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            Redo();
                            this.IsPressed = true;
                            this.SoundInstance[1].Play();
                        }
                    }
                    break;
            }
            
          
        }
        private void SetAllCards()
        {
            for(int i=0;i<this.TPiles.Count;i++)
            {
                this.TPiles[i].SetCards();
            }
            this.SPile[0].SetCards();
            for(int i=0;i<this.FPiles.Count;i++)
            {
                this.FPiles[i].SetCard();
            }
        }
        private void DrawCards()
        {
            SetAllCards();
            for(int i = 0;i<this.TPiles.Count;i++)
            {
                for(int j = 0; j < this.TPiles[i].GetSize();j++)
                {
                    this.TPiles[i][j].Draw(ref _spriteBatch);
                }
            }
            for(int i=0;i<this.SPile.Count;i++)
            {
                for(int j=0;j<this.SPile[i].GetSize();j++)
                {
                    this.SPile[i][j].Draw(ref _spriteBatch);
                }
                int index;
                int size;
                if (this.SPile[i].GetShowSize() < Global.level)
                {
                    size = this.SPile[i].GetShowSize();
                    index = 0;
                }
                else
                {
                    size = Global.level;
                    index = this.SPile[i].GetShowSize() - Global.level;
                }
                for(int j = 0; j < size;j++)
                {
                   this.SPile[i].GetShowCardAt(index + j).Draw(ref _spriteBatch);
                }
            }
            for(int i=0;i<this.FPiles.Count;i++)
            {
                this.FPiles[i].Draw(ref _spriteBatch);
                for(int j = 0; j < this.FPiles[i].GetSize();j++)
                {
                    this.FPiles[i][j].Draw(ref _spriteBatch);
                }
            }
            for(int i=0;i<this.trans.Count;i++)
            {
                this.trans[i].Draw(ref _spriteBatch);
            }
        }
        private void DrawMainMenu()
        {
            _spriteBatch.Draw(this.textures[56], this.OptionsBound[0], this.OptionsColor[0]);
            _spriteBatch.Draw(this.textures[57], this.OptionsBound[1], this.OptionsColor[1]);
            _spriteBatch.Draw(this.textures[58], this.OptionsBound[2], this.OptionsColor[2]);
        }
        private void DrawScore()
        {
            this.msg = "Score: " + this.score;
            int FontSize = 20;
            Vector2 pos = new Vector2(GraphicsDevice.Viewport.Width / 2 - ((this.msg.Length/2) * FontSize), 0);
            _spriteBatch.DrawString(this.font, this.msg, pos, Color.White);
        }
        private void DrawShuffling()
        {
            Random randX = new Random();
            Random randY = new Random();
            Vector2 pos = new Vector2();
            for(int i=0;i<this.TPiles.Count;i++)
            {
                for(int j = 0; j < this.TPiles[i].GetSize();j++)
                {
                    pos.X = randX.Next((int)(GraphicsDevice.Viewport.Width - Global.CardH_W.X));
                    pos.Y = randY.Next((int)(GraphicsDevice.Viewport.Height - Global.CardH_W.Y));
                    this.TPiles[i][j].UpdatePosition(pos);
                    this.TPiles[i][j].Draw(ref _spriteBatch);
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this._spriteBatch.Begin();
            int index = 0;
            switch(this.CurrentState)
            {
                case GameState.MainMenu:
                    index = 55;
                    break;
                case GameState.Gameplay:
                    index = 54;
                    break;
            }
            float ScaleX = (float)GraphicsDevice.Viewport.Width / (float)this.textures[index].Width, ScaleY = (float)GraphicsDevice.Viewport.Height / (float)this.textures[index].Height;
            int ScaledX = (int)(this.textures[index].Width * ScaleX), ScaledY = (int)(this.textures[index].Height * ScaleY);
            this._spriteBatch.Draw(this.textures[index], new Rectangle(0, 0, ScaledX, ScaledY), Color.White);
            switch (this.CurrentState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.Gameplay:
                    if (gameTime.TotalGameTime < System.TimeSpan.FromSeconds(3))
                        DrawShuffling();
                    else
                    {
                        this.IsShuffling = false;
                        DrawScore();
                        DrawCards();
                    }
                    break;
            }
            this._spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}