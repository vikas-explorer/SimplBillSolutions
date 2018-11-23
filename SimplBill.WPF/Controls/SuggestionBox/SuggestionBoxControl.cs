using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplBill.WPF.Controls
{
    /// <summary>
    /// This class acts as a view modal for any attatched suggestion box textbox
    /// </summary>
    internal class SuggestionBoxControl : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Instance of popup class for hosting listbox within
        /// </summary>
        private Popup suggestionPopup;
        /// <summary>
        /// Instance of listbox to host the suggestion list
        /// </summary>
        private ListBox suggestionListBox;

        /// <summary>
        /// Variable to host the is enabled property
        /// </summary>
        private bool isEnabled;

        /// <summary>
        /// Variable to store the attached textbox
        /// </summary>
        private TextBox sourceTextBox;

        /// <summary>
        /// Variable to store the suggestion provider for custom suggestion 
        /// </summary>
        private ISuggestionProvider suggestionProvider;

        /// <summary>
        /// Variable to store the data source for the listbox
        /// </summary>
        private ObservableCollection<object> suggestionSource;

        /// <summary>
        /// used to suppress the next lostfocus event
        /// </summary>
        private bool suppressTextChange;

        /// <summary>
        /// Flag to check if initialized
        /// </summary>
        private bool isInitialized;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        /// <param name="textBox">Instance of textbox to attach with</param>
        /// <exception cref="ArgumentException">When the argument 'textBox' is null.</exception>
        public SuggestionBoxControl(TextBox textBox)
        {
            //validate the textbox
            if (textBox == null)
                //throw appropriate exception if null
                throw new ArgumentException("'textBox' can not be null.");
            //store the textbox instance
            this.sourceTextBox = textBox;

            //generate popup
            InitializePopup();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets if the suggestion are enabled
        /// </summary>
        public bool IsEnabled
        {
            //return the status
            get { return isEnabled; }
            set
            {
                //store the new status
                isEnabled = value;
                //see if true
                if (value)
                    //if true then attach the appropriate events
                    AttachEvents();
                else
                    //if not then detach the attached events
                    DetachEvents();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the popup control and the inner listbox
        /// </summary>
        private void InitializePopup()
        {
            //create a listbox and assign the properties
            suggestionListBox = new ListBox() { HorizontalContentAlignment = HorizontalAlignment.Stretch };

            //create a popup inatance and assign properties
            suggestionPopup = new Popup()
            {
                StaysOpen = false,
                Child = suggestionListBox,
                Placement = PlacementMode.Bottom,
                PopupAnimation = SystemParameters.ComboBoxPopupAnimation
            };

            //attach the preview mousedown event
            suggestionListBox.PreviewMouseDown += listBox_PreviewMouseDown;
            //attach selection changed event to monitor changes
            suggestionListBox.SelectionChanged += currentListBox_SelectionChanged;
        }
        /// <summary>
        /// Attachs the events to the textbox
        /// </summary>
        private void AttachEvents()
        {
            //if ok then attach the keyboard focus events
            sourceTextBox.AddHandler(FrameworkElement.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(textBox_GotKeyboardFocus), true);
            sourceTextBox.AddHandler(FrameworkElement.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(textBox_LostKeyboardFocus), true);

        }

        /// <summary>
        /// Detachs the events to the textbox
        /// </summary>
        private void DetachEvents()
        {
            //if ok then detach the keyboard focus events
            sourceTextBox.RemoveHandler(FrameworkElement.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(textBox_GotKeyboardFocus));
            sourceTextBox.RemoveHandler(FrameworkElement.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(textBox_LostKeyboardFocus));
        }

        /// <summary>
        /// Handles the KeyDown event on textbox
        /// </summary>
        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //check the key
            switch (e.Key)
            {
                //if key is down key
                case Key.Down:
                    //navigate down in the list
                    NavigateDown();
                    break;

                //see if key is Up Arrow
                case Key.Up:
                    //navigate up in the list
                    NavigateUp();
                    break;

                //see if key is escape
                case Key.Escape:
                    //request to close the popup
                    suggestionPopup.IsOpen=false;
                    break;
            }
        }

        /// <summary>
        /// Navigates down in the suggestion popup list
        /// </summary>
        private void NavigateDown()
        {
            //see if we have items
            if (suggestionListBox.Items.Count > 0)
                //if yes then see if there is no selected item
                if (suggestionListBox.SelectedIndex < 0)
                {
                    //select the first item
                    suggestionListBox.SelectedIndex = 0;
                }
                else
                {
                    //otherwise see if we have enough items
                    if (suggestionListBox.SelectedIndex + 1 < suggestionListBox.Items.Count)
                    {
                        //move to next item
                        suggestionListBox.SelectedIndex++;
                    }
                }
        }

        /// <summary>
        /// Navigates up in the suggestion popup list
        /// </summary>
        private void NavigateUp()
        {
            //see if we have items
            if (suggestionListBox.Items.Count > 0)
                //if yes then see if there some item selected other then the first
                if (suggestionListBox.SelectedIndex > 0)
                {
                    //move to previous item
                    suggestionListBox.SelectedIndex--;
                }
        }

        /// <summary>
        /// Fired upon change in list selected item property
        /// </summary>
        void currentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set the current value to textbox
            SetValueByItem(suggestionListBox.SelectedItem);
        }

        /// <summary>
        /// Sets a value to the textbox
        /// </summary>
        /// <param name="item">Object to set to textbox</param>
        private void SetValueByItem(object item)
        {
            //if item is not valid
            if (item == null)
                //exit method
                return;
            //mark a flag as we are changing values
            suppressTextChange = true;
            //see if we have suggestion provider
            if (suggestionProvider == null)
                //if not then set the generic ToString() value
                sourceTextBox.Text = item.ToString();
            else
                //otherwise ask provider to generate a value
                sourceTextBox.Text = suggestionProvider.GetDisplayValue(item);

            //update the caret position
            sourceTextBox.CaretIndex = sourceTextBox.Text.Length;
        }

        /// <summary>
        /// Displays a popup to screen
        /// </summary>
        private void ShowPopup()
        {
            //if it is open or we have nothing something in textbox
            if (suggestionPopup.IsOpen || sourceTextBox.Text.Trim().Length < 1)
                //exit method
                return;

            //open it
            suggestionPopup.IsOpen = true;
        }

        /// <summary>
        /// Handles text changed event of textbox
        /// </summary>
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //see if we have a data source
            if (suggestionSource == null || suppressTextChange)
            {
                //unmark the text chnage flag 
                suppressTextChange = false;
                //if not the exit method
                return;
            }

            //verify if we have something in textbox
            if (sourceTextBox.Text.Trim().Length > 0)
            {
                //request to show popup
                ShowPopup();

                //see if we have provider
                if (suggestionProvider != null)
                    //invoke query changed
                    suggestionProvider.QueryChanged(suggestionSource, sourceTextBox.Text.Trim());

                //check if we have some item in list
                if (suggestionListBox.Items.Count < 1)
                    //if not then close the popup
                    suggestionPopup.IsOpen=false;
            }
            else
            {
                //otherwise close popup
                 suggestionPopup.IsOpen=false;
            }
        }

        /// <summary>
        /// Returns an instance of popup
        /// </summary>        
        private void PreparePopup()
        {
            //if already initialized then exit method
            if (isInitialized)
                return;

            //set the popup placement target
            suggestionPopup.PlacementTarget = sourceTextBox;
            //assign the width to textbox width
            suggestionListBox.Width = sourceTextBox.ActualWidth;

            //see if we have a suggestion provider
            if (suggestionProvider == null)
            {
                //try to retrieve it from the text box
                suggestionProvider = sourceTextBox.GetValue(SuggestionBoxBehavior.ProviderProperty) as ISuggestionProvider;

                //see if we get one
                if (suggestionProvider == null)
                {
                    //if not then create a basic string suggestion provider
                    suggestionProvider = new StringSuggestionProvider();
                }

                //get the number of results property and assign to provider
                suggestionProvider.NumberOfResults = (int)sourceTextBox.GetValue(SuggestionBoxBehavior.NumberOfResultsProperty);
            }

            //fetch the item template and apply to listbox
            suggestionListBox.ItemTemplate = sourceTextBox.GetValue(SuggestionBoxBehavior.ItemTemplateProperty) as DataTemplate;

            //see if we have datasource
            if (suggestionSource == null)
            {
                //if not then retrieve the masterlist of textbox
                IEnumerable assignedDataSource = (IEnumerable)sourceTextBox.GetValue(SuggestionBoxBehavior.MasterListProperty);

                //use provider to generate the suggestion view source
                suggestionSource = suggestionProvider.ProvideDataView(assignedDataSource);
            }

            //assign the sugegstion view to listbox
            suggestionListBox.SetValue(ListBox.ItemsSourceProperty, suggestionSource);

            //mark the flag
            isInitialized = true;
        }

        /// <summary>
        /// Handles preview mouse down event of listbox
        /// </summary>
        void listBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //retrieve the source
            DependencyObject depObj = (DependencyObject)e.OriginalSource;

            //loop till we reach the listbox item
            while ((depObj != null) && !(depObj is ListBoxItem))
            {
                //get the parent object
                depObj = VisualTreeHelper.GetParent(depObj);
            }

            //if we got nothing then exit method
            if (depObj == null) return;


            //otherwise try to get item from contanier
            var item = suggestionListBox.ItemContainerGenerator.ItemFromContainer(depObj);

            //if we got nothing then exit method
            if (item == null) return;

            //set the value to textbox
            SetValueByItem(item);

            //mark as handled
            e.Handled = true;

            //close popup
            suggestionPopup.IsOpen = false;
        }

        /// <summary>
        /// Handles the keyboard focus event
        /// </summary>
        private void textBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            //prepare popup with values.
            PreparePopup();

            //attach the events
            sourceTextBox.AddHandler(FrameworkElement.PreviewKeyDownEvent, new KeyEventHandler(textBox_PreviewKeyDown), true);
            sourceTextBox.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(textBox_TextChanged), true);
        }

        /// <summary>
        /// Handles the keyboard lost focus event
        /// </summary>
        private void textBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            //detach the events
            sourceTextBox.RemoveHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(textBox_TextChanged));
            sourceTextBox.RemoveHandler(FrameworkElement.PreviewKeyDownEvent, new KeyEventHandler(textBox_PreviewKeyDown));

        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs the cleanup
        /// </summary>
        public void Dispose()
        {
            //detach the listbox events
            suggestionListBox.PreviewMouseDown -= listBox_PreviewMouseDown;
            suggestionListBox.SelectionChanged -= currentListBox_SelectionChanged;

            //assure to detach the events from textbox too
            DetachEvents();

            //nullify the variables
            sourceTextBox = null;
            suggestionProvider = null;
            suggestionSource = null;
            suggestionPopup.Child = null;
            suggestionPopup = null;
            suggestionListBox = null;

            //request gc to suppress finalize on this object 
            //as we already cleaned it up
            GC.SuppressFinalize(this);
        }

        #endregion
    }




}
