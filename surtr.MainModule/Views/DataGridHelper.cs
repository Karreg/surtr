namespace surtr.MainModule.Views
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    public class DataGridHelper
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(DataGridHelper),
            new FrameworkPropertyMetadata((IList)null,
                new PropertyChangedCallback(OnSelectedItemsChanged)));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList) d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid) d;
            ReSetSelectedItems(dataGrid);
            dataGrid.SelectionChanged += delegate
            {
                ReSetSelectedItems(dataGrid);
            };
        }

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
