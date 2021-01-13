using System.Windows;
using System.Windows.Controls;

using ModernWpfRepro.Models;

namespace ModernWpfRepro.DataTemplates {
  public class FilterDataTemplateSelector : DataTemplateSelector {
    public DataTemplate? DefaultDataTemplate { get; set; }
    public DataTemplate? FilterEqualDataTemplate { get; set; }
    public DataTemplate? FilterOneOfDataTemplate { get; set; }

    public override DataTemplate? SelectTemplate (object item, DependencyObject container) {
      if (container is FrameworkElement element) {
        if (item is IFilter filter) {
          return element.FindResource($"{filter.Kind}DataTemplate") as DataTemplate;
        }
      }

      return null;
    }
  }
}
