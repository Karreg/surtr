namespace Surtr.MainModule.Views
{
    using System.Windows.Input;

    using Surtr.MainModule.ViewModels;

    /// <summary>
    /// Interaction logic for Library View
    /// </summary>
    public partial class LibraryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryView"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The view model.
        /// </param>
        public LibraryView(LibraryViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }

        /// <summary>
        /// The library grid_ mouse enter.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LibraryGridMouseEnter(object sender, MouseEventArgs e)
        {
            this.LibraryGrid.Focus();
        }

        /// <summary>
        /// The synchronization grid_ mouse enter.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SynchronizationGridMouseEnter(object sender, MouseEventArgs e)
        {
            this.SynchronizationGrid.Focus();
        }
    }
}
