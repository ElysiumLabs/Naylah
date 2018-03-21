using Naylah.Xamarin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Style
{
    public class StyleKitBase
    {
        public Color PrimaryColor { get; set; }

        public Color PrimaryColorDark { get; set; }

        public Color PrimaryColorLight { get; set; }

        /// <summary>
        /// This color may idicate a relatedAction or information.
        /// </summary>
        public Color SecondaryColor { get; set; }

        public Color SecondaryColorDark { get; set; }

        public Color SecondaryColorLight { get; set; }

        /// <summary>
        /// This normally is the secondary color it self. It's used to actions like action button
        /// and interactive elements.
        /// Text fields, cursors, text selection, progress bar,
        /// selection controls, buttons, sliders, links
        /// </summary>
        public Color AccentColor { get; set; }



        public Color BackgroundColorSystem { get; set; }

        public Color BackgroundColorAppBar { get; set; }

        public Color BackgroundColorPage { get; set; }

        public Color BackgroundColorModal { get; set; }



        public Color PrimaryTextColor { get; set; }

        public Color SecondaryTextColor { get; set; }

        public Color DisabledTextColor { get; set; }

        public Color DividerColor { get; set; }

        public Color TextColorOfPrimaryColor { get; set; }

        public Color TextColorOfAccentColor { get; set; }



        public Color ActiveIconColor { get; set; }

        public Color InactiveIconColor { get; set; }



        public Action Customizations;

        public virtual void Apply()
        {

            //BootStrapper.CurrentApp.Resources[nameof(PrimaryColor)] = PrimaryColor;
            //BootStrapper.CurrentApp.Resources[nameof(PrimaryDarkColor)] = PrimaryDarkColor;
            //BootStrapper.CurrentApp.Resources[nameof(PrimaryLightColor)] = PrimaryLightColor;
            //BootStrapper.CurrentApp.Resources[nameof(SecondaryColor)] = SecondaryColor;
            //BootStrapper.CurrentApp.Resources[nameof(SecondaryDarkColor)] = SecondaryDarkColor;
            //BootStrapper.CurrentApp.Resources[nameof(AccentColor)] = AccentColor;
            //BootStrapper.CurrentApp.Resources[nameof(BackgroundColor)] = BackgroundColor;
            //BootStrapper.CurrentApp.Resources[nameof(BackgroundDarkColor)] = BackgroundDarkColor;
            //BootStrapper.CurrentApp.Resources[nameof(PrimaryTextColor)] = PrimaryTextColor;
            //BootStrapper.CurrentApp.Resources[nameof(SecondaryTextColor)] = SecondaryTextColor;
            //BootStrapper.CurrentApp.Resources[nameof(WindowColor)] = WindowColor;
            //BootStrapper.CurrentApp.Resources[nameof(DividerColor)] = DividerColor;

            Customizations?.Invoke();
        }

    

    }
}
