using System;
using System.Collections.ObjectModel;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ModernWpfRepro.Models {
  public enum FilterType {
    Equal,
    OneOf
  }

  public interface IFilter : IEquatable<IFilter> {
    public static FilterType Type { get; set; } = FilterType.Equal;

    public string Kind { get; }
    public string FieldName { get; set; }
    public Guid Id { get; }
  }

  public class FilterBase : ReactiveObject {
    public Guid Id { get; init; } = Guid.NewGuid();

    public bool Equals (IFilter? obj) => obj is IFilter item && Id == item.Id;
    public override bool Equals (object? obj) => Equals(obj as IFilter);
    public override int GetHashCode () => Id.GetHashCode();
  }

  public class FilterEqual : FilterBase, IFilter {
    public static FilterType Type { get; set; } = FilterType.Equal;

    public string Kind => nameof(FilterEqual);

    [Reactive]
    public string FieldName { get; set; } = "";

    [Reactive]
    public string Value { get; set; } = "";

    public FilterEqual (string compare) {
      Value = compare;
    }
  }

  public class FilterOneOf : FilterBase, IFilter {
    public static FilterType Type { get; set; } = FilterType.OneOf;

    public string Kind => nameof(FilterOneOf);

    [Reactive]
    public string FieldName { get; set; } = "";

    [Reactive]
    public ObservableCollection<string> Values { get; set; } = new();

    public FilterOneOf (params string[] values) {
      Values = new(values);
    }
  }
}
