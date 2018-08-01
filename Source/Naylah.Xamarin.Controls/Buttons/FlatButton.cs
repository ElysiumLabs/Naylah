using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Buttons
{
    public class FlatButton : Button
    {
        public static readonly new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(FlatButton), Color.Gray);

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly new BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FlatButton), null);

        public new ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly new BindableProperty CommandParameterProperty =
           BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FlatButton), null);

        public new object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }

        }


      
    }
}
