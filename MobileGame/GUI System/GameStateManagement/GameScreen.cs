using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GUI_System.GameStateManagement
{
    public abstract class GameScreen
    {

        public enum ScreenState
        {
            TransitionOn,
            Active,
            TransitionOff,
            Hidden
        }

        public GameScreen()
        {

        }

        //This property is used to determine if the underlying screen should transition off or not
        //If the new screen is just a simple popup, then there is no need to transition off the underlying screen
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }
        bool isPopup = false;

        //The time it takes for the screen to transition when its activated
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }
        TimeSpan transitionOnTime = TimeSpan.Zero;

        //The time it takes for the screen to transition off when deactivated
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }
        TimeSpan transitionOffTime = TimeSpan.Zero;

        //Used to represent the position of the transition, from 0 (fully active, no transition) to 1 (transition fully off to nothing)
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }
        float transitionPosition = 1;

        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        public ScreenState CurrentScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }
        ScreenState screenState = ScreenState.TransitionOn;


        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }
        bool isExiting = false;


        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus && (screenState == ScreenState.TransitionOn || screenState == ScreenState.Active);
            }
        }
        bool otherScreenHasFocus;


        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        ScreenManager screenManager;

        public virtual void Activate() { }

        public virtual void Unload() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                //If the screen is on its way to die, it should transition off
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //When the transition is finished we can remove the screen
                    ScreenManager.RemoveScreen(this);
                }
            }

            else if (coveredByOtherScreen)
            {
                //If the screen is being coverd by another screen it should transition off
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    //Still transitioning
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    //Transition done
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                //Otherwice the screen should transition on
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    //Still transitioning
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    //Transition done
                    screenState = ScreenState.Active;
                }
            }
        }

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            //Calulates how much we should move
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            //Now we can update the transitions position
            TransitionPosition += transitionDelta * direction;

            //Have we finished the transition?
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            //If we havnt finished the transition that means we still are transitioning
            return true;
        }

        public virtual void HandleInput(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }

        //This tells the screen to go away. However, this method respects the transition-timings and makes the
        //screen transition off. Unlike ScreenManager.RemoveScreen which just instantly removes the screen
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                isExiting = true;
            }
        }
    }
}