using Naylah.App.UX;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Naylah.App.UI.Controls
{
    public class ContentButtonView : ContentView, IButtonController
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ContentButtonView), null, propertyChanged: (bo, o, n) => ((ContentButtonView)bo).OnCommandChanged());

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Button), null,
            propertyChanged: (bindable, oldvalue, newvalue) => ((ContentButtonView)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public event EventHandler Clicked;

        public event EventHandler Pressed;

        public event EventHandler Released;

        public Action<ContentButtonView> PressedAnimation { get; set; }

        public Action<ContentButtonView> ReleasedAnimation { get; set; }

        public Action<ContentButtonView> FeedbackAction { get; set; }

        public ContentButtonView()
        {
            PressedAnimation = UIUtils.DefaultButtonPressedAnimation;
            ReleasedAnimation = UIUtils.DefaultButtonReleasedAnimation;
            FeedbackAction = UXUtils.DefaultButtonHapticFeedback;

            Pressed += ContentButtonView_Pressed;
            Released += ContentButtonView_Released;
        }

        private void ContentButtonView_Released(object sender, EventArgs e)
        {
            ReleasedAnimation?.Invoke(this);
        }

        private void ContentButtonView_Pressed(object sender, EventArgs e)
        {
            PressedAnimation?.Invoke(this);
            FeedbackAction?.Invoke(this);
        }

        private void CommandCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            ICommand cmd = Command;
            if (cmd != null)
                IsEnabled = cmd.CanExecute(CommandParameter);
        }

        private void OnCommandChanged()
        {
            if (Command != null)
            {
                Command.CanExecuteChanged += CommandCanExecuteChanged;
                CommandCanExecuteChanged(this, EventArgs.Empty);
            }
            else
                IsEnabled = true;
        }

        public virtual void SendClicked()
        {
            if (IsEnabled == true)
            {
                Command?.Execute(CommandParameter);
                Clicked?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void SendPressed()
        {
            if (IsEnabled == true)
            {
                Pressed?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void SendReleased()
        {
            if (IsEnabled == true)
            {
                Released?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}