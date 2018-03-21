using Android.Content.Res;
using Android.Graphics;
using Android.Support.Design.Widget;
using Java.Lang.Reflect;

namespace Naylah.Xamarin.Android.Platform
{
    public static class TextInputLayoutExtensions
    {
        public static void SetExtendColor(this TextInputLayout til, Color color, bool accent = true)
        {
            Field hint = til.Class.GetDeclaredField("mDefaultTextColor");
            hint.Accessible = true;
            hint.Set(til, new ColorStateList(new int[][] { new[] { 0 } }, new int[] { color }));

            Field hintText = til.Class.GetDeclaredField("mFocusedTextColor");
            hintText.Accessible = true;
            hintText.Set(til, new ColorStateList(new int[][] { new[] { 0 } }, new int[] { color }));

            til.EditText.Background.Mutate().SetColorFilter(color, PorterDuff.Mode.SrcAtop);
        }
    }
}