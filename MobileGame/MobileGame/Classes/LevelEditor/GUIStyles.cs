using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GUI_System.GUIObjects;

namespace MobileGame.LevelEditor
{
    public class ColorPickStyle : ColorPickerStyle
    {
        public ColorPickStyle(ContentManager content)
            : base(content)
        {
            TextureToPickFrom = content.Load<Texture2D>("Editor/Colorpicker2");
        }
    }

    public class ExitButtonStyle : ButtonStyle
    {
        public ExitButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/Close");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/CloseHoover");
        }
    }

    public class LoadMapButtonStyle : ButtonStyle
    {
        public LoadMapButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/Load");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/LoadHoover");
        }
    }

    public class SaveMapButtonStyle : ButtonStyle
    {
        public SaveMapButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/Save");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/SaveHoover");
        }
    }

    public class LayerUpButtonStyle : ButtonStyle
    {
        public LayerUpButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/LayerUp");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/LayerUpHoover");
        }
    }

    public class LayerDownButtonStyle : ButtonStyle
    {
        public LayerDownButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/LayerDown");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/LayerDownHoover");
        }
    }

    public class SaveButtonStyle : ButtonStyle
    {
        public SaveButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/Save");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/SaveHoover");
        }
    }

    public class CancelButtonStyle : ButtonStyle
    {
        public CancelButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/Cancel");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/CancelHoover");
        }
    }

    public class MapNameInputStyle : InputFieldStyle
    {
        public MapNameInputStyle(ContentManager content)
            : base(content)
        {
            LabelTexture = content.Load<Texture2D>("Editor/SaveMap/InputLabel");

            InputBGTexture = content.Load<Texture2D>("Editor/SaveMap/InputBG");
            InputTextureToDraw = InputBGTexture;

            InputBGHooverTexture = content.Load<Texture2D>("Editor/SaveMap/InputBGHoover");

            Font = content.Load<SpriteFont>("Fonts/DejaVuSans_20");
        }
    }

    public class LoadButtonStyle : ButtonStyle
    {
        public LoadButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/LoadMap/LoadNormal");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/LoadMap/LoadHoover");
        }
    }

    public class DeleteButtonStyle : ButtonStyle
    {
        public DeleteButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/LoadMap/DeleteNormal");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/LoadMap/DeleteHoover");
        }
    }

    public class LoadMapListStyle : ListStyle
    {
        public LoadMapListStyle(ContentManager content)
            : base(content)
        {
            Texture = content.Load<Texture2D>("Editor/LoadMap/ListBackground");
            Width = Texture.Width;
            Height = Texture.Height;

            Position = new Vector2(200, 200);
            ItemOffset = new Vector2(10, 10);
        }
    }

    public class LoadMapListItemStyle : ListItemStyle
    {
        public LoadMapListItemStyle(ContentManager content)
            : base(content)
        {
            Font = content.Load<SpriteFont>("Editor/LoadMap/MapListFont");
            DefaultColor = Color.Black;
        }
    }

    public class NoButtonStyle : ButtonStyle
    {
        public NoButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ConfirmationBox/NoNormal");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ConfirmationBox/NoHoover");
        }
    }

    public class YesButtonStyle : ButtonStyle
    {
        public YesButtonStyle(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ConfirmationBox/YesNormal");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ConfirmationBox/YesHoover");
        }
    }

    public class BGLayerButton : ButtonStyle
    {
        public BGLayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Normal/Background");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Hoover/BackgroundHoover");
        }
    }

    public class MiddleLayerBytton : ButtonStyle
    {
        public MiddleLayerBytton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Normal/Middle");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Hoover/MiddleHoover");

        }
    }

    public class FGLayerButton : ButtonStyle
    {
        public FGLayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Normal/Foreground");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/LayerButtons/Hoover/ForegroundHoover");
        }
    }

    public class TileLayerButton : ButtonStyle
    {
        public TileLayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/TileLayerToggle");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/TileLayerToggleHoover");
        }
    }

    public class CollisionLayerButton : ButtonStyle
    {
        public CollisionLayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/CollisionLayerToggle");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/CollisionLayerToggleHoover");
        }
    }

    public class JumpLayerButton : ButtonStyle
    {
        public JumpLayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/JumpLayerToggle");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/Buttons/NewButtons/JumpLayerToggleHoover");
        }
    }

    #region Styles for all ToolButtons

    public class DynamicTileButton : ButtonStyle
    {
        public DynamicTileButton(int aTileSheetX, int aTileSheetY, int aTileSize, ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/TileSetEditorButtons");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/TileSetEditorButtons");

            SourceRect = new Rectangle(aTileSheetX * aTileSize, aTileSheetY * aTileSize, aTileSize, aTileSize);
        }
    }

    public class Tile0 : ButtonStyle
    {
        public Tile0(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile0");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile0Hoover");
        }
    }

    public class Tile1 : ButtonStyle
    {
        public Tile1(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile1");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile1Hoover");
        }
    }

    public class Tile2 : ButtonStyle
    {
        public Tile2(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile2");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile2Hoover");
        }
    }

    public class Tile3 : ButtonStyle
    {
        public Tile3(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile3");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile3Hoover");
        }
    }

    public class Tile4 : ButtonStyle
    {
        public Tile4(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile4");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile4Hoover");
        }
    }

    public class Tile5 : ButtonStyle
    {
        public Tile5(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile5");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile5Hoover");
        }
    }

    public class Tile6 : ButtonStyle
    {
        public Tile6(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile6");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile6Hoover");
        }
    }

    public class Tile7 : ButtonStyle
    {
        public Tile7(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile7");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile7Hoover");
        }
    }

    public class Tile8 : ButtonStyle
    {
        public Tile8(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile8");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile8Hoover");
        }
    }

    public class Tile9 : ButtonStyle
    {
        public Tile9(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile9");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile9Hoover");
        }
    }

    public class Tile10 : ButtonStyle
    {
        public Tile10(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile10");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile10Hoover");
        }
    }

    public class Tile11 : ButtonStyle
    {
        public Tile11(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile11");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile11Hoover");
        }
    }

    public class Tile12 : ButtonStyle
    {
        public Tile12(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile12");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile12Hoover");
        }
    }

    public class Tile13 : ButtonStyle
    {
        public Tile13(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile13");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile13Hoover");
        }
    }

    public class Tile14 : ButtonStyle
    {
        public Tile14(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile14");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile14Hoover");
        }
    }

    public class Tile15 : ButtonStyle
    {
        public Tile15(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Tile15");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Tile15Hoover");
        }
    }

    public class BGTile0 : ButtonStyle
    {
        public BGTile0(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile0");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile0Hoover");
        }
    }

    public class BGTile1 : ButtonStyle
    {
        public BGTile1(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile1");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile1Hoover");
        }
    }

    public class BGTile2 : ButtonStyle
    {
        public BGTile2(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile2");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile2Hoover");
        }
    }

    public class BGTile3 : ButtonStyle
    {
        public BGTile3(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile3");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile3Hoover");
        }
    }

    public class BGTile4 : ButtonStyle
    {
        public BGTile4(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile4");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile4Hoover");
        }
    }

    public class BGTile5 : ButtonStyle
    {
        public BGTile5(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile5");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile5Hoover");
        }
    }

    public class BGTile6 : ButtonStyle
    {
        public BGTile6(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile6");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile6Hoover");
        }
    }

    public class BGTile7 : ButtonStyle
    {
        public BGTile7(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile7");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile7Hoover");
        }
    }

    public class BGTile8 : ButtonStyle
    {
        public BGTile8(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/BGTile8");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/BGTile8Hoover");
        }
    }

    public class Spike0Button : ButtonStyle
    {
        public Spike0Button(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Spike00");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Spike0Hoover");
        }
    }

    public class Spike1Button : ButtonStyle
    {
        public Spike1Button(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Spike01");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Spike01Hoover");
        }
    }

    public class Spike2Button : ButtonStyle
    {
        public Spike2Button(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Spike02");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/Spike02Hoover");
        }
    }

    public class GoalTileButton : ButtonStyle
    {
        public GoalTileButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/GoalTile");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/GoalTileHoover");
        }
    }

    public class JumpTileButton : ButtonStyle
    {
        public JumpTileButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/JumpTile");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/JumpTileHoover");
        }
    }

    public class PlayerButton : ButtonStyle
    {
        public PlayerButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/PlayerButton");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/PlayerButtonHoover");
        }
    }

    public class EnemyButton : ButtonStyle
    {
        public EnemyButton(ContentManager content)
            : base(content)
        {
            NormalTexture = content.Load<Texture2D>("Editor/ToolButtons/Normal/Enemy");
            TextureToDraw = NormalTexture;

            hooverTexture = content.Load<Texture2D>("Editor/ToolButtons/Hoover/EnemyHoover");
        }
    }
    #endregion
}
