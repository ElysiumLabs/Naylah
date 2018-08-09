using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using System;

namespace Naylah.App.Xamarin.Android.Controls
{
    public class CustomEditText : EditText
    {
        public bool UtilizeCustomInputConnection { get; set; } = false;

        public CustomEditText(Context context) : base(context)
        {
        }

        public CustomEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomEditText(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CustomEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override IInputConnection OnCreateInputConnection(EditorInfo outAttrs)
        {
            if (UtilizeCustomInputConnection)
            {
                this.SetSingleLine(true);

                var inputConnection = new CustomInputConnection(this, false);
                outAttrs.InputType = this.InputType;
                outAttrs.ImeOptions = outAttrs.ImeOptions | (ImeFlags)ImeAction.Done;

                try
                {
                    outAttrs.InitialSelStart = this.Text.Length;
                }
                catch (Exception)
                {
                }

                return inputConnection;
            }
            else
            {
                return base.OnCreateInputConnection(outAttrs);
            }
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            bool v = base.OnKeyDown(keyCode, e);
            return v;
        }
    }

    public class CustomInputConnection : BaseInputConnection
    {
        public CustomInputConnection(View targetView, bool fullEditor) : base(targetView, fullEditor)
        {
        }

        protected CustomInputConnection(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool SendKeyEvent(KeyEvent e)
        {
            bool v = base.SendKeyEvent(e);
            return v;
        }

        public override bool DeleteSurroundingText(int beforeLength, int afterLength)
        {
            base.SendKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.Back));
            return false;
            //return base.DeleteSurroundingText(beforeLength, afterLength);
        }
    };
}