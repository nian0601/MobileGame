using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GUI_System.GUIObjects
{
    abstract public class Style
    {
        public Texture2D normalTexture { get; internal set; }
        public Texture2D hooverTexture { get; internal set; }
        public Texture2D TextureToDraw { get; internal set; }
        public Vector2 Position { get; internal set; }
        public Color Color { get; internal set; }
        public Vector2 Origin { get; internal set; }

        public Style(ContentManager Content)
        {
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 origin)
        {
            Origin = origin;
            spriteBatch.Draw(TextureToDraw, Position, null, Color, 0, Origin, 1f, SpriteEffects.None, 0);
        }

        public virtual Rectangle HitBox()
        {
            return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, TextureToDraw.Width, TextureToDraw.Height);
        }

        public virtual void NormalTexture()
        {
            TextureToDraw = normalTexture;
        }

        public virtual void HooverTexture()
        {
            TextureToDraw = hooverTexture;
        }
    }

    public class PlayButtonStyle : Style
    {
        public PlayButtonStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButtonHoover");
        }
    }

    public class LevelSelectStyle : Style
    {
        public LevelSelectStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButtonHoover");
        }
    }

    public class ReturnButtonSTyle : Style
    {
        public ReturnButtonSTyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButtonHoover");
        }
    }
}