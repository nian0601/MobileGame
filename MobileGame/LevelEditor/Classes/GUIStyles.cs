using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GUI_System.GUIObjects;

namespace LevelEditor
{
    public class ExitButtonStyle : ButtonStyle
    {
        public ExitButtonStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/Buttons/ExitButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/ExitButtonHoover");
        }
    }

    public class LoadMapButtonStyle : ButtonStyle
    {
        public LoadMapButtonStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/Buttons/LoadMapButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/LoadMapButtonHoover");
        }
    }

    public class SaveMapButtonStyle : ButtonStyle
    {
        public SaveMapButtonStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/Buttons/SaveMapButton");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/SaveMapButtonHoover");
        }
    }

    public class SaveButtonStyle : ButtonStyle
    {
        public SaveButtonStyle(ContentManager content) : base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/Buttons/Save");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/SaveHoover");
        }
    }

    public class CancelButtonStyle : ButtonStyle
    {
        public CancelButtonStyle(ContentManager content)
            : base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/Buttons/Cancel");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/CancelHoover");
        }
    }

    public class MapNameInputStyle : InputFieldStyle
    {
        public MapNameInputStyle(ContentManager content) : base(content)
        {
            LabelTexture = content.Load<Texture2D>("Editor/SaveMap/InputLabel");

            InputBGTexture = content.Load<Texture2D>("Editor/SaveMap/InputBG");
            InputTextureToDraw = InputBGTexture;

            InputBGHooverTexture = content.Load<Texture2D>("Editor/SaveMap/InputBGHoover");

            Font = content.Load<SpriteFont>("Fonts/DejaVuSans_20");
        }
    }
}
