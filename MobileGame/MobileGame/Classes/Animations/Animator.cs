using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.Animations
{
    class Animator
    {
        private List<Animation> animationList;
        private Animation activeAnimation;
        private Animation prevAnimation;
        private Rectangle sourceRect;
        private bool animating;

        private int frameWidth;
        private int frameHeight;
        private int currentCol;
        private int currentRow;
        private float currentTime;

        #region Properties

        public Rectangle SourceRectangle { get { return sourceRect; } }
        public Animation ActiveAnimation { get { return activeAnimation; } }
        public Animation PrevAnimation { get { return prevAnimation; } }

        #endregion

        public Animator(int FrameWidth, int FrameHeight)
        {
            animationList = new List<Animation>();
            activeAnimation = null;
            prevAnimation = null;
            animating = false;

            frameWidth = FrameWidth;
            frameHeight = FrameHeight;
            currentCol = 0;
            currentRow = 0;

            sourceRect = new Rectangle(0, 0, frameWidth, frameHeight);
            currentTime = 0;
        }

        public void Update(float ElapsedTime)
        {
            if (animating && activeAnimation != null)
            {
                currentTime += ElapsedTime;

                UpdateSourceRect();

                if (currentTime >= activeAnimation.TimePerFrame)
                {
                    currentCol++;

                    if (currentCol >= activeAnimation.EndCol)
                    {
                        currentCol = activeAnimation.StartCol;
                        currentRow++;

                        
                        if (currentRow > activeAnimation.EndRow)
                        {
                            if(activeAnimation.Looping == true)
                            {
                                currentRow = activeAnimation.StartRow;
                            }
                            else
                            {
                                StopAnimation();
                            }
                            
                        }
                    }

                    if(activeAnimation != null)
                    {
                        currentTime -= activeAnimation.TimePerFrame;
                    }
                    
                }
            }
        }

        public void StartAnimation(string Animation)
        {
            if (activeAnimation == null)
            {
                foreach (Animation Anim in animationList)
                {
                    if (Anim.Name == Animation)
                    {
                        Console.WriteLine("STARTED ANIMATION: " + Animation);
                        activeAnimation = Anim;
                        currentCol = activeAnimation.StartCol;
                        currentRow = activeAnimation.StartRow;
                        animating = true;
                        return;
                    }
                }
            } 
        }

        public void StopAnimation()
        {
            if (activeAnimation != null)
            {
                animating = false;
                currentCol = activeAnimation.StartCol;
                currentRow = activeAnimation.StartRow;
                currentTime = 0;

                UpdateSourceRect();
                Console.WriteLine("STOPPED ANIMATION: " + activeAnimation.Name);
                prevAnimation = activeAnimation;
                activeAnimation = null;
            }
        }

        public void StopAnimation(String AnimationName)
        {
            if (activeAnimation != null)
            {
                if(activeAnimation.Name == AnimationName)
                {
                    animating = false; 
                    currentCol = activeAnimation.StartCol;
                    currentRow = activeAnimation.StartRow;
                    currentTime = 0;

                    UpdateSourceRect();

                    prevAnimation = activeAnimation;
                    activeAnimation = null;
                }
            }
        }

        public void AddAnimation(Animation NewAnimation)
        {
            if (animationList.Count == 0)
            {
                currentCol = NewAnimation.StartCol;
                currentRow = NewAnimation.StartRow;
                currentTime = 0;
            }

            if (!animationList.Contains(NewAnimation))
                animationList.Add(NewAnimation);
        }

        public void ResetPrevAnimation()
        {
            prevAnimation = null;
        }

        private void UpdateSourceRect()
        {
            sourceRect = new Rectangle(currentCol * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
        }
    }
}
