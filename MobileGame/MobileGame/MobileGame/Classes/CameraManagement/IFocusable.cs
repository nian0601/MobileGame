using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame.CameraManagement
{
    interface IFocusable
    {
        /// <summary>
        /// The position of the focusobject
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The width of the focusobject
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the focusobject
        /// </summary>
        int Height { get; }

        /// <summary>
        /// The radius used to influence the cameramovement, but not completely take control of it
        /// </summary>
        int InterestRadius { get; }

        /// <summary>
        /// The radius used to take full control over the camera
        /// </summary>
        int ControlRadius { get; }

        /// <summary>
        /// The texture used to draw interest-debug
        /// </summary>
        Texture2D InterestCircle { get; set; }

        /// <summary>
        /// The texture used to draw control-debug
        /// </summary>
        Texture2D ControlCircle { get; set; }

    }
}
