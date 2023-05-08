using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PythonIDE
{
    class PythonItemSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate ChildTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is PythonItem viewModel && viewModel.Items.Any())
            {
                return ChildTemplate;
            }
            return DefaultTemplate;
        }
    }
}
