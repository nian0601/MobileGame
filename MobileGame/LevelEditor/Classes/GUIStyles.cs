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

    public class Tile0 : ButtonStyle
    {
        public Tile0(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile0");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile0Hoover");
        }
    }

    public class Tile1 : ButtonStyle
    {
        public Tile1(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile1");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile1Hoover");
        }
    }

    public class Tile2 : ButtonStyle
    {
        public Tile2(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile2");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile2Hoover");
        }
    }

    public class Tile3 : ButtonStyle
    {
        public Tile3(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile3");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile3Hoover");
        }
    }

    public class Tile4 : ButtonStyle
    {
        public Tile4(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile4");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile4Hoover");
        }
    }

    public class Tile5 : ButtonStyle
    {
        public Tile5(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile5");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile5Hoover");
        }
    }

    public class Tile6 : ButtonStyle
    {
        public Tile6(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile6");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile6Hoover");
        }
    }

    public class Tile7 : ButtonStyle
    {
        public Tile7(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile7");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile7Hoover");
        }
    }

    public class Tile8 : ButtonStyle
    {
        public Tile8(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile8");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile8Hoover");
        }
    }

    public class Tile9 : ButtonStyle
    {
        public Tile9(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile9");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile9Hoover");
        }
    }

    public class Tile10 : ButtonStyle
    {
        public Tile10(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile10");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile10Hoover");
        }
    }

    public class Tile11 : ButtonStyle
    {
        public Tile11(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile11");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile11Hoover");
        }
    }

    public class Tile12 : ButtonStyle
    {
        public Tile12(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile12");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile12Hoover");
        }
    }

    public class Tile13 : ButtonStyle
    {
        public Tile13(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile13");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile13Hoover");
        }
    }

    public class Tile14 : ButtonStyle
    {
        public Tile14(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile14");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile14Hoover");
        }
    }

    public class Tile15 : ButtonStyle
    {
        public Tile15(ContentManager content): base(content)
        {
            normalTexture = content.Load<Texture2D>("Editor/TileButtons/Normal/Tile15");
            TextureToDraw = normalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/TileButtons/Hoover/Tile15Hoover");
        }
    }

}
