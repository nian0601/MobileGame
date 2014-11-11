using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GUI_System.GUIObjects
{
    class TextInputObject
    {
        //The text that has been entered into the object
        private string text = string.Empty;
        public string Text
        {
            get { return text; }
            internal set { text = value; }
        }

        //This limits the amount of letters/numbers we can store
        private int charNumLimit = 16;
        public int CharNumLimit
        {
            get { return charNumLimit; }
            set { charNumLimit = value; }
        }

        //Determines if the letters should be upper or lowercase
        private bool caps;
        //A collection of all the keys that were pressed during the LAST frame
        private Keys[] lastPressedKeys = new Keys[1];

        public TextInputObject() { }

        public void Update()
        {
            GetKeys();
        }

        private void GetKeys()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //We check if any of the keys we pressed last update is no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                {
                    OnKeyUp(key);
                }
            }

            //We check if the currently pressed keys were NOT pressed in the last update
            //That is, if we have a new keypress
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                {
                    OnKeyDown(key);
                }
            }

            //And then we save the currently pressed keys so that we can compare them in the next update
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyDown(Keys key)
        {
            //Remove a letter if the string isnt empty, if backspace(back) is pressed
            if (key == Keys.Back)
            {
                if(text.Length > 0)
                    text = text.Remove(text.Length - 1);
            }
            //Enables caps if one of the shiftbutton were pressed
            else if (key == Keys.LeftShift || key == Keys.RightShift)
            {
                caps = true;
            }
            //Simply adds a space if space is pressed
            else if (key == Keys.Space)
            {
                text += " ";
            }
            //If caps is off and the text isnt to long we add the letter in lowercase
            else if (!caps && text.Length < charNumLimit)
            {
                text += key.ToString().ToLower();
            }
            //If all else fails we add the letter in caps
            else if (text.Length < charNumLimit)
            {
                text += key.ToString();
            }
        }

        private void OnKeyUp(Keys key)
        {
            //Turn off caps if one of the shift keys is released
            if (key == Keys.LeftShift || key == Keys.RightShift)
            {
                caps = false;
            }
        }
    }
}
