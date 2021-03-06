﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MobileGame.LevelEditor.Tools;
using MobileGame.LightingSystem;

namespace MobileGame.LevelEditor
{
    class ToolManager
    {
        internal static int mouseX, mouseY;

        #region Selection-Variables
        public static bool HasActiveSelection;
        public static Point SelectionTopLeftIndex;
        public static Point SelectionBottomRightIndex;
        #endregion

        public static bool ShowPasteTarget;

        private static Selector Selector;
        private static TileCreator TileCreator;
        private static CopyPaster CopyPaster;
        private static ColliderMaker ColliderMaker;
        private static JumpThroughMaker JumpThroughMaker;
        private static LightsPlacer LightPlacer;

        private static int prevEditMode;

        public static void Initialize()
        {
            HasActiveSelection = false;
            SelectionTopLeftIndex = new Point(0, 0);
            SelectionBottomRightIndex = new Point(0, 0);

            ShowPasteTarget = false;

            Selector = new Selector();
            TileCreator = new TileCreator();
            CopyPaster = new CopyPaster();
            ColliderMaker = new ColliderMaker();
            JumpThroughMaker = new JumpThroughMaker();
            LightPlacer = new LightsPlacer();

            prevEditMode = 0;
        }

        public static void Update(LightRenderer aLightRenderer)
        {
            Point Offset = new Point((int)EditorMapManager.Offset.X, (int)EditorMapManager.Offset.Y);
            mouseX = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).X;
            mouseY = ConvertPixelsToIndex(KeyMouseReader.GetMousePos()).Y;

            Console.WriteLine(KeyMouseReader.GetMousePos() - Offset);

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
                if (EditorScreen.EditMode == 0)
                {
                    CopyPaster.Update(mouseX, mouseY);

                    if (!CopyPaster.Active)
                        TileCreator.Update(mouseX, mouseY);
                }
                #endregion

                #region Collision-Flagging
                else if (EditorScreen.EditMode == 1)
                {
                    ColliderMaker.Update(mouseX, mouseY);
                }
                #endregion

                #region JumpThroughAble-Flagging
                else if (EditorScreen.EditMode == 2)
                {
                    JumpThroughMaker.Update(mouseX, mouseY);
                }
                #endregion

                #region Lights
                else if (EditorScreen.EditMode == 4)
                {
                    if (prevEditMode != 4)
                        LightPlacer.Init(aLightRenderer);

                    LightPlacer.Update(aLightRenderer);
                }
                #endregion

            }

            if (KeyMouseReader.isKeyDown(Keys.LeftControl) && KeyMouseReader.KeyClick(Keys.D))
                ClearSelection();

            prevEditMode = EditorScreen.EditMode;
        }

        public static void Draw(SpriteBatch SpriteBatch)
        {
            if (CopyPaster.DisplayPasteTarget)
                CopyPaster.ShowPasteTarget(mouseX, mouseY, 0, SpriteBatch);

            Selector.Draw(SpriteBatch);

            if (EditorScreen.EditMode == 4)
            {
                LightPlacer.Draw(SpriteBatch);
            }
        }

        public static void ClearSelection()
        {
            Selector.Reset();

            HasActiveSelection = false;
            SelectionTopLeftIndex = new Point(0, 0);
            SelectionBottomRightIndex = new Point(0, 0);
        }

        public static Point ConvertPixelsToIndex(Point pos)
        {
            int x = (int)(pos.X - EditorMapManager.Offset.X) / EditorMapManager.TileSize;
            int y = (int)(pos.Y - EditorMapManager.Offset.Y) / EditorMapManager.TileSize;

            return new Point(x, y);
        }
    }
}
