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

        public static void Initialize()
        {
            HasActiveSelection = false;
            SelectionTopLeft = new Point(0, 0);
            SelectionBottomRight = new Point(0, 0);

            ShowPasteTarget = false;

            Selector = new Tools.Selector();
            TileCreator = new Tools.TileCreator();
            CopyPaster = new Tools.CopyPaster();
            ColliderMaker = new Tools.ColliderMaker();
            JumpThroughMaker = new JumpThroughMaker();
        }

        public static void Update()
        {
            mouseX = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).X;
            mouseY = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).Y;

            mouseX += EditorMapManager.xOffset;
            mouseY += EditorMapManager.yOffset;

            if (KeyMouseReader.isKeyDown(Keys.LeftAlt) && KeyMouseReader.LeftClick())
            {
                EditorMapManager.SetCursorTexture(EditorMapManager.SelectedLayer[mouseX, mouseY]);
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
                CopyPaster.ShowPasteTarget(mouseX, mouseY, 0, SpriteBatch);

            Selector.Draw(SpriteBatch);
        }

        public static void ClearSelection()
        {
            Selector.Reset();

            HasActiveSelection = false;
            SelectionTopLeft = new Point(0, 0);
            SelectionBottomRight = new Point(0, 0);
        }

        public static Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)(pos.X - EditorMapManager.Offset.X) / EditorMapManager.TileSize;
            int y = (int)(pos.Y - EditorMapManager.Offset.Y) / EditorMapManager.TileSize;

            return new Point(x, y);
        }
    }
}
