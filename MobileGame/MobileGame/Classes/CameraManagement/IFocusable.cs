using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MobileGame.CameraManagement
{
    interface IFocusable
    {
        /// <summary>
        /// The position of the focusobject
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// The radius used to influence the cameramovement, but not completely take control of it
        /// </summary>
        int InterestRadius { get; }

        /// <summary>
        /// The radius used to take full control over the camera
        /// </summary>
        int ControlRadius { get; }

    }
}
