using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Naylah.Xamarin.iOS.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts the UIColor to a Xamarin Color object.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="defaultColor">The default color.</param>
        /// <returns>UIColor.</returns>
        public static UIColor ToUIColorOrDefault(this Color color, UIColor defaultColor)
        {
            if (color == Color.Default)
                return defaultColor;

            return color.ToUIColor();
        }
    }
}