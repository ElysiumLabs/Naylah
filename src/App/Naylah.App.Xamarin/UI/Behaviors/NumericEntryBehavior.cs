using Naylah.App.UI.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Naylah.App.UI.Behaviors
{
    public class NumericEntryBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty NumericFormatProperty =
           BindableProperty.Create(nameof(NumericFormat), typeof(string), typeof(NumericEntryBehavior), "N");

        public string NumericFormat
        {
            get { return (string)GetValue(NumericFormatProperty); }
            set { SetValue(NumericFormatProperty, value); }
        }

        public static readonly BindableProperty NumericTypeProperty =
           BindableProperty.Create(nameof(NumericType), typeof(NumericEntryBehaviorType), typeof(NumericEntryBehavior), default(NumericEntryBehaviorType));

        public NumericEntryBehaviorType NumericType
        {
            get { return (NumericEntryBehaviorType)GetValue(NumericTypeProperty); }
            set { SetValue(NumericTypeProperty, value); }
        }

        public static readonly BindableProperty NumericValueProperty =
           BindableProperty.Create(nameof(NumericValue), typeof(double), typeof(NumericEntryBehavior), (double)0);

        public double NumericValue
        {
            get { return (double)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        public Func<double, bool> NumericValidation { get; set; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (
                ((NumericValueProperty.PropertyName) == propertyName)
                ||
                ((NumericFormatProperty.PropertyName) == propertyName)
                ||
                ((NumericTypeProperty.PropertyName) == propertyName)
                )
            {
                SetTexts(GetNumericNumberFormated());
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (entry == null)
            {
                return;
            }

            try
            {
                entry.TextChanged -= Entry_TextChanged;

                if (string.IsNullOrEmpty(entry.Text))
                {
                    NumericValue = 0;
                }

                if (GetNumericNumberFormated() != entry.Text)
                {
                    SetTexts(GetNumericNumberFormated());
                }
            }
            catch (Exception)
            {
                SetTexts(GetNumericNumberFormated());
            }
            finally
            {
                entry.TextChanged += Entry_TextChanged;
            }
        }

        private string GetNumericNumberFormated()
        {
            if (NumericType == NumericEntryBehaviorType.Double)
            {
                return NumericValue.ToString(NumericFormat);
            }
            else
            {
                return ((int)NumericValue).ToString(NumericFormat);
            }
        }

        public void SetTexts(string text)
        {
            try
            {
                if (entry == null) { return; }
                entry.Text = text;
                //entry.SelectionStart = AssociatedObjectAsTextBox.Text.Length;
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
        }

        private void AddNumberStack(int num)
        {
            var nv = GetCleanNumberStack();

            if (Double.Parse(nv) == 0)
            {
                if (nv.Length > 0)
                {
                    nv = nv.Remove(nv.Length - 1);
                }
            }

            nv += num.ToString();

            SetStackNumberToNumeric(nv);
        }

        private void SetStackNumberToNumeric(string nv)
        {
            double numericValue = 0;

            if (NumericType == NumericEntryBehaviorType.Double)
            {
                nv = nv.Insert(nv.Length - CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                numericValue = Double.Parse(nv);
            }
            else
            {
                numericValue = Int64.Parse(nv);
            }

            bool numericValid = NumericValidation?.Invoke(numericValue) ?? true;

            if (numericValid)
            {
                NumericValue = numericValue;
            }

            SetTexts(GetNumericNumberFormated());
        }

        public string GetCleanNumberStack()
        {
            string r = string.Empty;

            var nv = GetNumericNumberFormated();

            if (NumericType == NumericEntryBehaviorType.Integer)
            {
                nv = nv.Split(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0])[0];
            }

            for (int i = 0; i < nv.Length; i++)
            {
                if (Char.IsDigit(nv[i]))
                    r += nv[i];
            }

            return r;
        }

        private void RemoveNumberStack()
        {
            try
            {
                var nv = GetCleanNumberStack();

                if (nv.Length > 0)
                {
                    nv = nv.Remove(nv.Length - 1);
                }

                if (string.IsNullOrEmpty(nv)) { nv = "0"; }

                SetStackNumberToNumeric(nv);
            }
            catch (Exception)
            {
            }
        }

        private int? GetDigitBykey(VirtualKey virtualKey)
        {
            try
            {
                switch (virtualKey)
                {
                    case VirtualKey.N0:
                        return 0;

                    case VirtualKey.N1:
                        return 1;

                    case VirtualKey.N2:
                        return 2;

                    case VirtualKey.N3:
                        return 3;

                    case VirtualKey.N4:
                        return 4;

                    case VirtualKey.N5:
                        return 5;

                    case VirtualKey.N6:
                        return 6;

                    case VirtualKey.N7:
                        return 7;

                    case VirtualKey.N8:
                        return 8;

                    case VirtualKey.N9:
                        return 9;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void KeyPressed(VirtualKey virtualKey)
        {
            if (entry == null)
            {
                return;
            }

            var num = GetDigitBykey(virtualKey);

            if (num != null)
            {
                AddNumberStack(num.Value);
            }
            else
            {
                if (virtualKey == VirtualKey.Back)
                {
                    RemoveNumberStack();
                }
            }
        }

        private Entry entry;

        protected override void OnAttachedTo(Entry bindable)
        {
            entry = bindable;
            entry.TextChanged += Entry_TextChanged;

            BindingContext = entry.BindingContext;
            entry.BindingContextChanged += associatedObject_BindingContextChanged;

            SetTexts(GetNumericNumberFormated());

            base.OnAttachedTo(bindable);
        }

        private void associatedObject_BindingContextChanged(object sender, EventArgs e)
        {
            if (entry != null)
                this.BindingContext = entry.BindingContext;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            entry.TextChanged -= Entry_TextChanged;
            entry = null;

            base.OnDetachingFrom(bindable);
        }

        public enum NumericEntryBehaviorType
        {
            Integer,
            Double,
        }

        public enum VirtualKey
        {
            None,
            N0,
            N1,
            N2,
            N3,
            N4,
            N5,
            N6,
            N7,
            N8,
            N9,
            Tab,
            Return,
            Back
        }
    }

    public static class NumericEntryBehaviorExtensions
    {
        public static NumericEntryBehavior GetNumericEntryBehavior(this NyEntry entry)
        {
            return entry.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;
        }

        public static NumericEntryBehavior AddNumericEntryBehavior(this NyEntry entry, NumericEntryBehavior numericBehavior = null)
        {
            var b = entry.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).FirstOrDefault() as NumericEntryBehavior;

            if (b == null)
            {
                b = numericBehavior ?? new NumericEntryBehavior();
                entry.Behaviors.Add(b);
            }

            return b;
        }

        public static bool HasNumericEntryBehavior(this NyEntry entry)
        {
            return entry.Behaviors.Where(x => x.GetType() == typeof(NumericEntryBehavior)).Any();
        }
    }
}