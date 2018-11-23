using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimplBill.WPF.Controls
{
    /// <summary>
    /// Class for implementation of ISuggestionProvider for treating any list as string values 
    /// </summary>
    public class StringSuggestionProvider : ISuggestionProvider
    {
        #region Private Members
        /// <summary>
        /// Variable for storing actual data source
        /// </summary>
        private IEnumerable sourceData;
        #endregion

        #region ISuggestionProvider Members

        /// <summary>
        /// Generates an observable collection from the given enumerable datasource
        /// </summary>
        /// <param name="data">Instance of a enumerable class</param>
        /// <returns>Instance of ObservableCollection</returns>
        public ObservableCollection<object> ProvideDataView(IEnumerable data)
        {
            //store to member
            sourceData = data;
            //return a new observable collection
            return new ObservableCollection<object>();
        }

        /// <summary>
        /// Generates a display value for the given object
        /// </summary>
        /// <param name="obj">Object instance</param>
        /// <returns>Display value as string</returns>
        public string GetDisplayValue(object obj)
        {
            //validate the object
            if (obj != null)
                //use the ToString method to pull the display string
                return obj.ToString();
            //otherwise return blank string
            return string.Empty;
        }

        /// <summary>
        /// Invoked upon change in the input query
        /// It also updates the data source for matching suggestions
        /// </summary>
        /// <param name="view">Insatnce of the given observable collection via ProvideDataView method</param>
        /// <param name="query">String specifying the current query</param>
        public void QueryChanged(ObservableCollection<object> view, string query)
        {
            //verify source data
            if (sourceData == null)
                //exit if not valid
                return;

            //clear the previous view
            view.Clear();

            //filter out the values as per the given query
            IEnumerable<object> filter = from object str in sourceData
                                         where str.ToString().StartsWith(query, StringComparison.OrdinalIgnoreCase)
                                         select str;

            //take the defined number of results
            filter.Take(NumberOfResults).All(obj => { view.Add(obj); return true; });
        }

        /// <summary>
        /// Gets or sets the total Number of results to be displayed at once
        /// </summary>
        public int NumberOfResults
        { get; set; }

        #endregion
    }
}
