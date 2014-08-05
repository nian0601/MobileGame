using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using LevelEditor.Tools;

namespace LevelEditor.Managers
{
    class ToolManager
    {
        internal static Tile[, ,] TileArray;
        internal static int mouseX, mouseY;

        #region Selection-Variables
        public static bool HasActiveSelection;
        public static Point SelectionTopLeft;
        public static Point SelectionBottomRight;
        #endregion

        public static bool ShowPasteTarget;

        private static Selector Selector;
        private static TileCreator TileCreator;
        private static CopyPaster CopyPaster;
        private static ColliderMaker ColliderMaker;
        private static JumpThroughMaker JumpThroughMaker;

        public static void Initialize(Tile[, ,] tileArray)
        {
            TileArray = tileArray;
            HasActiveSelection = false;
            SelectionTopLeft = new Point(0, 0);
            SelectionBottomRight = new Point(0, 0);

            ShowPasteTarget = false;

            Selector = new Tools.Selector(tileArray);
            TileCreator = new Tools.TileCreator(tileArray);
            CopyPaster = new Tools.CopyPaster(tileArray);
            ColliderMaker = new Tools.ColliderMaker(tileArray);
            JumpThroughMaker = new JumpThroughMaker(tileArray);
        }

        public static void Update()
        {
            mouseX = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).X;
            mouseY = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).Y;

            mouseX += MapManager.xOffset;
            mouseY += MapManager.yOffset;

            if (KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.LeftClick())
            {
                MapManager.SetCursorTexture(TileArray[0, mouseX, mouseY].TileValue);
            }

            #region Selection
            if (KeyMouseReader.isKeyDown(Keys.LeftShift))
            {
                Selector.Update();
            }
            #endregion

            else
            {
                #region TileCreation
                if (Game1.EditMode == 0)
                {
                    CopyPaster.Update(mouseX, mouseY);

                    if (!CopyPaster.Active)
                        TileCreator.Update(mouseX, mouseY);
                }
                #endregion

                #region Collision-Flagging
                else if (Game1.EditMode == 1)
                {
                    ColliderMaker.Update(mouseX, mouseY);
                }
                #endregion

                #region JumpThroughAble-Flagging
                else if (Game1.EditMode == 2)
                {
                    JumpThroughMaker.Update(mouseX, mouseY);
                }
                #endregion
            }

            if (KeyMouseReader.isKeyDown(Keys.LeftControl) && KeyMouseReader.KeyClick(Keys.D))
                ClearSelection();
        }

        public static void Draw(SpriteBatch SpriteBatch)
        {
            if(CopyPaster.DisplayPasteTarget)
                CopyPaster.ShowPasteTarget(mouseX, mouseY, 0, MapManager.Spritebatch);
        }

        public static void ClearSelection()
        {
            //Platform-layer
            for (int x = 0; x < TileArray.GetUpperBound(1); x++)
            {
                for (int y = 0; y < TileArray.GetUpperBound(2); y++)
                {
                    TileArray[0, x, y].Selected = false;
                }
            }

            //Special-layer
            for (int x = 0; x < TileArray.GetUpperBound(1); x++)
            {
                for (int y = 0; y < TileArray.GetUpperBound(2); y++)
                {
                    TileArray[1, x, y].Selected = false;
                }
            }

            HasActiveSelection = false;
            SelectionTopLeft = new Point(0, 0);
            SelectionBottomRight = new Point(0, 0);
        }

        public static Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)(pos.X - MapManager.Offset.X) / MapManager.TileSize;
            int y = (int)(pos.Y - MapManager.Offset.Y) / MapManager.TileSize;

            return new Point(x, y);
        }
    }
}
