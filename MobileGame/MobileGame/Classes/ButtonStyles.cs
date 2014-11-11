using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GUI_System.GUIObjects;

namespace MobileGame.Screens
{
    public class PlayButtonStyle : ButtonStyle
    {
        public PlayButtonStyle(ContentManager content) : base(content)
        {
            NormalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButtonHoover");
        }
    }

    public class LevelSelectStyle : ButtonStyle
    {
        public LevelSelectStyle(ContentManager content) : base(content)
        {
            NormalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButtonHoover");
        }
    }

    public class MenuButtonStyle : ButtonStyle
    {
        public MenuButtonStyle(ContentManager content) : base(content)
        {
            NormalTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButtonHoover");
        }
    }

    public class RestartButtonStyle : ButtonStyle
    {
        public RestartButtonStyle(ContentManager content) : base(content)
        {
            NormalTexture = content.Load<Texture2D>("GUI Textures/PausScreen/RestartButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/PausScreen/RestartButtonHoover");
        }
    }

    public class CrossButtonStyle : ButtonStyle
    {
        public CrossButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("GUI Textures/PausScreen/ExitButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/PausScreen/ExitButtonHoover");
        }
    }
}
