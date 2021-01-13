using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

using DynamicData;
using ReactiveUI;

using ModernWpfRepro.Models;

namespace ModernWpfRepro.ViewModels {
  public class MainViewModel : ReactiveObject {
    #region Commands

    public ReactiveCommand<(IFilter filter, FilterType type), Unit> ChangeFilterType { get; }

    public ReactiveCommand<(FilterOneOf filter, string value), Unit> AddOneOfValue { get; }
    public ReactiveCommand<(FilterOneOf filter, string value), Unit> DeleteOneOfValue { get; }

    #endregion

    public Dictionary<FilterType, string> FilterTypes { get; set; } = new() {
      [FilterType.Equal] = "is equal to",
      [FilterType.OneOf] = "is one of"
    };

    #region Collections

    private readonly ReadOnlyObservableCollection<IFilter> _filters;
    public ReadOnlyObservableCollection<IFilter> Filters => _filters;

    private readonly SourceList<IFilter> _filterSource = new();

    #endregion

    public MainViewModel () {
      #region Commands

      ChangeFilterType = ReactiveCommand.Create<(IFilter filter, FilterType type)>(payload => {
        IFilter newFilter = payload.type switch {
          FilterType.Equal => new FilterEqual("") { Id = payload.filter.Id },
          FilterType.OneOf => new FilterOneOf() { Id = payload.filter.Id },
          _ => throw new NotSupportedException()
        };

        _filterSource.Replace(payload.filter, newFilter);
      });

      AddOneOfValue = ReactiveCommand.Create<(FilterOneOf filter, string value)>(payload => {
        payload.filter.Values.Add(payload.value);
      });

      DeleteOneOfValue = ReactiveCommand.Create<(FilterOneOf filter, string value)>(payload => {
        payload.filter.Values.Remove(payload.value);
      });

      #endregion

      var sharedFilters = _filterSource.Connect().Publish();

      _filterSource.AddRange(new IFilter[] {
        new FilterEqual("foo"),
        new FilterOneOf("one", "two")
      });

      sharedFilters.Bind(out _filters).Subscribe();
      sharedFilters.Connect();
    }
  }
}
