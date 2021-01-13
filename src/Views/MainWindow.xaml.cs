using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Windows.Controls;

using ReactiveUI;

using ModernWpfRepro.Models;
using ModernWpfRepro.ViewModels;

namespace ModernWpfRepro.Views {
#nullable disable
  public partial class MainWindow : ReactiveWindow<MainViewModel> {
#nullable enable
    public MainWindow () {
      InitializeComponent();
      ViewModel = new MainViewModel();

      this.WhenActivated(dispose => {
        this.OneWayBind(
          ViewModel,
          vm => vm.Filters,
          view => view.FilterListing.ItemsSource
        ).DisposeWith(dispose);
      });
    }

    void FilterTypeChangedImpl (object sender, SelectionChangedEventArgs e) {
      if (
        sender is ComboBox box &&
        box.DataContext is IFilter filter &&
        e.AddedItems.Count > 0 &&
        e.RemovedItems.Count > 0 &&
        e.AddedItems[0] is KeyValuePair<FilterType, string> pair &&
        filter.Kind != $"Filter{pair.Key}"
      ) {
        ViewModel.ChangeFilterType.Execute((filter, pair.Key)).Subscribe();
      }

      e.Handled = true;
    }
  }
}
