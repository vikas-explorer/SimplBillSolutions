using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SimplBill.WPF.Controls
{
    /// <summary>
    /// This class is used to control the textbox behavior
    /// </summary>
    public class SuggestionBoxBehavior : DependencyObject
    {
        #region Private Memebers
        /// <summary>
        /// Variable to store the attached control instances
        /// </summary>
        private static Dictionary<TextBox, SuggestionBoxControl> suggestionsControls;

        #endregion

        #region Attached Dependency Property
        /// <summary>
        /// Using a DependencyProperty as the backing store for IsSuggestionBoxEnable.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsSuggestionBoxEnabledProperty;

        /// <summary>
        /// Using a DependencyProperty as the backing store for NumberOfResults.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty NumberOfResultsProperty;

        /// <summary>
        /// Using a DependencyProperty as the backing store for MasterList.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MasterListProperty;
        /// <summary>
        /// Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty;
        /// <summary>
        /// Using a DependencyProperty as the backing store for Provider.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ProviderProperty;

        public static readonly DependencyProperty SelectedItemProperty;
        #endregion

        #region Static Constructor
        static SuggestionBoxBehavior()
        {
            suggestionsControls = new Dictionary<TextBox, SuggestionBoxControl>();
            IsSuggestionBoxEnabledProperty = DependencyProperty.RegisterAttached("IsSuggestionBoxEnabled", typeof(bool), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(false, IsSuggestionBoxEnabledChanged));
            NumberOfResultsProperty = DependencyProperty.RegisterAttached("NumberOfResults", typeof(int), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(10));
            MasterListProperty = DependencyProperty.RegisterAttached("MasterList", typeof(object), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(null));
            ItemTemplateProperty = DependencyProperty.RegisterAttached("ItemTemplate", typeof(DataTemplate), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(null));
            ProviderProperty = DependencyProperty.RegisterAttached("Provider", typeof(ISuggestionProvider), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(null));
            SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(SuggestionBoxBehavior), new UIPropertyMetadata(null));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Invoked when IsSuggestionBoxEnabled property changes
        /// </summary>
        static void IsSuggestionBoxEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //cast the object to textbox
            TextBox textBox = d as TextBox;

            //see if it is a textbox
            if (textBox != null)
            {
                //see if we have assigned a control to it
                if (suggestionsControls.ContainsKey(textBox))
                {
                    //set the property
                    suggestionsControls[textBox].IsEnabled = (bool)e.NewValue;
                }
                else
                {
                    //otherwise create a new control; associate to textbox and assign the property and store for future ref
                    suggestionsControls.Add(textBox, new SuggestionBoxControl(textBox) { IsEnabled = (bool)e.NewValue });
                    //attach the unloaded event for cleanup
                    textBox.Unloaded += textBox_Unloaded;
                }
            }
        }

        /// <summary>
        /// Handles the unloaded event of textbox
        /// </summary>
        private static void textBox_Unloaded(object sender, RoutedEventArgs e)
        {
            //cast to textbox
            TextBox textBox = sender as TextBox;

            //see if we have one
            if (textBox != null)
            {
                //detach the unloaded event
                textBox.Unloaded -= textBox_Unloaded;

                //see if we have a control associated with it
                if (suggestionsControls.ContainsKey(textBox))
                {
                    //dispose the attached control
                    suggestionsControls[textBox].Dispose();

                    //remove from the list
                    suggestionsControls.Remove(textBox);
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the value of IsSuggestionBoxEnable Property
        /// </summary>
        public static bool GetIsSuggestionBoxEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSuggestionBoxEnabledProperty);
        }

        /// <summary>
        /// Sets the value of IsSuggestionBoxEnable Property
        /// </summary>
        public static void SetIsSuggestionBoxEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSuggestionBoxEnabledProperty, value);
        }

        /// <summary>
        /// Gets the value of NumberOfResults Property
        /// </summary>
        public static int GetNumberOfResults(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberOfResultsProperty);
        }

        /// <summary>
        /// Sets the value of NumberOfResults Property
        /// </summary>
        public static void SetNumberOfResults(DependencyObject obj, int value)
        {
            obj.SetValue(NumberOfResultsProperty, value);
        }

        /// <summary>
        /// Gets the value of MasterList Property
        /// </summary>
        public static object GetMasterList(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(MasterListProperty);
        }

        /// <summary>
        /// Sets the value of MasterList Property
        /// </summary>
        public static void SetMasterList(DependencyObject obj, object value)
        {
            obj.SetValue(MasterListProperty, value);
        }

        /// <summary>
        /// Gets the value of Provider Property
        /// </summary>
        public static ISuggestionProvider GetProvider(DependencyObject obj)
        {
            return (ISuggestionProvider)obj.GetValue(ProviderProperty);
        }

        /// <summary>
        /// Sets the value of Provider Property
        /// </summary>
        public static void SetProvider(DependencyObject obj, ISuggestionProvider value)
        {
            obj.SetValue(ProviderProperty, value);
        }

        /// <summary>
        /// Gets the value of ItemTemplate Property
        /// </summary>
        public static DataTemplate GetItemTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(ItemTemplateProperty);
        }

        /// <summary>
        /// Sets the value of ItemTemplate Property
        /// </summary>
        public static void SetItemTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(ItemTemplateProperty, value);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }
        #endregion
    }
}
