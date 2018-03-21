using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Naylah.Xamarin.Android.Extras;
using Naylah.Xamarin.Behaviors;

namespace Naylah.Xamarin.Android.Extras
{
    public class NumericEntryBehaviorListener : BaseKeyListener
    {
        private NumericEntryBehavior NumericBehavior;

        public NumericEntryBehaviorListener(NumericEntryBehavior numericBehavior)
        {
            NumericBehavior = numericBehavior;
        }

        public override InputTypes InputType
        {
            get
            {
                return InputTypes.ClassNumber;
            }
        }

        public override bool OnKeyDown(View view, IEditable content, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {

            switch (keyCode)
            {
                case Keycode.Num0:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N0);
                    break;
                case Keycode.Num1:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N1);
                    break;
                case Keycode.Num2:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N2);
                    break;
                case Keycode.Num3:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N3);
                    break;
                case Keycode.Num4:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N4);
                    break;
                case Keycode.Num5:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N5);
                    break;
                case Keycode.Num6:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N6);
                    break;
                case Keycode.Num7:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N7);
                    break;
                case Keycode.Num8:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N8);
                    break;
                case Keycode.Num9:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N9);
                    break;
                case Keycode.Back:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.Back);
                    break;
                //case Keycode.Clear:
                //    break;
                case Keycode.Del:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.Back);
                    break;
                case Keycode.Numpad0:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N0);
                    break;
                case Keycode.Numpad1:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N1);
                    break;
                case Keycode.Numpad2:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N2);
                    break;
                case Keycode.Numpad3:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N3);
                    break;
                case Keycode.Numpad4:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N4);
                    break;
                case Keycode.Numpad5:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N5);
                    break;
                case Keycode.Numpad6:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N6);
                    break;
                case Keycode.Numpad7:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N7);
                    break;
                case Keycode.Numpad8:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N8);
                    break;
                case Keycode.Numpad9:
                    NumericBehavior.KeyPressed(NumericEntryBehavior.VirtualKey.N9);
                    break;

            }

            var edTxt = view as CustomEditText;

            if (edTxt != null)
            {
                edTxt.SetSelection(edTxt.Length());
            }

            return (keyCode == Keycode.Enter || keyCode == Keycode.Tab);
        }

    }
}