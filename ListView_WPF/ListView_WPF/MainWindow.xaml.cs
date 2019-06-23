using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListView_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isGrouping;
        public bool isSorted;
        public MainWindow()
        {
            InitializeComponent();
            lvViewQuestions.ItemsSource = DataProvider.Instance.GetLstQuestion();
            isGrouping = false;
            isSorted = false;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lvViewQuestions.ItemsSource);
            collectionView.Filter = userFilter;
            #region LINQ
            //var results = DataProvider.Instance.GetLstQuestion().Where(x => x.result == Question.Result.B).Select(x => x.question);
            //List<Question> questions = DataProvider.Instance.GetLstQuestion();
            //var results = from question in questions where question.result == Question.Result.B select question.question;
            //foreach (string result in results)
            //{
            //    MessageBox.Show(result);
            //}
            #endregion
        }

        private bool userFilter(object item)
        {
            if (string.IsNullOrEmpty(txbFilter.Text))
                return true;
            else
                return ((item as Question).question.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lvViewQuestions.ItemsSource);
            if (!isGrouping && collectionView.GroupDescriptions.Count == 0)
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription("result"));
            else
            {
                if (isGrouping && collectionView.GroupDescriptions.Count != 0)
                    collectionView.GroupDescriptions.Clear();
            }
            isGrouping = !isGrouping;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader header = sender as GridViewColumnHeader;
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(lvViewQuestions.ItemsSource);
            if (!isSorted)
            {
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(header.Name, ListSortDirection.Ascending));
            }
            else
            {
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(header.Name, ListSortDirection.Descending));
            }

            isSorted = !isSorted;
        }

        private void BtnFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvViewQuestions.ItemsSource).Refresh();
        }
    }
}
