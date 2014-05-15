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
    public class PlayButtonStyle : Style
    {
        public PlayButtonStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/PlayButtonHoover");
        }
    }

    public class LevelSelectStyle : Style
    {
        public LevelSelectStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/MainMenu/LevelSelectionButtonHoover");
        }
    }

    public class MenuButtonStyle : Style
    {
        public MenuButtonStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/LevelSelection/ReturnButtonHoover");
        }
    }

    public class RestartButtonStyle : Style
    {
        public RestartButtonStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/PausScreen/RestartButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/PausScreen/RestartButtonHoover");
        }
    }

    public class CrossButtonStyle : Style
    {
        public CrossButtonStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("GUI Textures/PausScreen/ExitButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("GUI Textures/PausScreen/ExitButtonHoover");
        }
    }
}
