using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naylah.Xamarin.Controls.Buttons
{
    public class ImageButton : CachedImage
    {
        public static readonly BindableProperty CommandProperty =
          BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageButton), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    this.AnchorX = 0.48;
                    this.AnchorY = 0.48;
                    await this.ScaleTo(0.8, 50, Easing.Linear);
                    await Task.Delay(100);
                    await this.ScaleTo(1, 50, Easing.Linear);
                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                    }
                });
            }
        }

        public ImageButton()
        {
            Initialize();
        }

        public void Initialize()
        {
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TransitionCommand
            });
        }
    }
}