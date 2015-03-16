namespace Surtr.MainModule.Views
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Data Grid helper.
    /// </summary>
    public class DataGridHelper
    {
        /// <summary>
        /// The selected items property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(DataGridHelper),
                new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>The list.</returns>
        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="value">The value.</param>
        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Called when [selected items changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            ReSetSelectedItems(dataGrid);
            dataGrid.SelectionChanged += delegate
            {
                ReSetSelectedItems(dataGrid);
            };
        }

        /// <summary>
        /// Res the set selected items.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        private static void ReSetSelectedItems(DataGrid dataGrid)
        {
            IList selectedItems = GetSelectedItems(dataGrid);
            selectedItems.Clear();
            if (dataGrid.SelectedItems != null)
            {
                foreach (var item in dataGrid.SelectedItems)
                {
                    selectedItems.Add(item);
                }
            }
        }
    }
}
