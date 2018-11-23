using System.Collections;
using System.Collections.ObjectModel;

namespace SimplBill.WPF.Controls
{
    /// <summary>
    /// Suggestion provider class for custom implementation for Suggestion ModalView
    /// </summary>
    public interface ISuggestionProvider
    {
        /// <summary>
        /// Generates an observable collection from the given enumerable datasource
        /// </summary>
        /// <param name="data">Instance of a enumerable class</param>
        /// <returns>Instance of ObservableCollection</returns>
        ObservableCollection<object> ProvideDataView(IEnumerable data);

        /// <summary>
        /// Generates a display value for the given object
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Display value as string</returns>
        string GetDisplayValue(object obj);

        /// <summary>
        /// Invoked upon change in the input query
        /// It also updates the data source for matching suggestions
        /// </summary>
        /// <param name="view">Instance of the given observable collection previously generated via ProvideDataView method</param>
        /// <param name="query">String specifying the current query</param>
        void QueryChanged(ObservableCollection<object> view, string query);

        /// <summary>
        /// Gets or sets the total Number of results to be displayed at once
        /// </summary>
        int NumberOfResults { get; set; }
    }
}
