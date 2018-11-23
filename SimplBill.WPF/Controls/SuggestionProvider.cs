namespace SimplBill.WPF.Controls
{

    using System;
    using System.Collections;
    /// <summary>
    /// A generic suggestion provider. 
    /// </summary>
    /// <seealso cref="WpfControls.Editors.ISuggestionProvider" />
    public class ProductSuggestionProvider : ISuggestionProvider
    {


        #region Private Fields

        private readonly Func<string, IEnumerable> _method;

        #endregion Private Fields
        

        public IEnumerable GetSuggestions(string filter)
        {
            return new string[] { "Aluminium", "Copper" };
        }
    }
}
