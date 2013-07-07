using System;

namespace Sp.Idioms
{
  /// <summary>
  /// Provides logical optionality for value-types. See <see cref="Option{T}"/> (logical optionality for reference types)
  /// for the description of the option-idom in general and the explanation why a special implementation for value-types is
  /// necessary/provided. 
  /// </summary>
  public struct ValueOption<TValue> where TValue : struct
  {
    public static readonly ValueOption<TValue> None = new ValueOption<TValue>(null);
    private readonly TValue? _nullableValue;

    private ValueOption(TValue? nullableValue)
    {
      _nullableValue = nullableValue;
    }
   
    public static ValueOption<TValue> Some(TValue value)
    {
      return new ValueOption<TValue>(value);
    }
 
    public bool HasValue
    {
      get { return _nullableValue.HasValue; }
    }

    public TValue Value
    {
      get { return _nullableValue.Value; }
    }

    public TValue ValueOrDefault
    {
      get { return _nullableValue.GetValueOrDefault(); }
    }

    public TValue ValueOr(TValue defaultValue)
    {
      return _nullableValue.GetValueOrDefault(defaultValue);
    }

    public bool TryGetValue(out TValue value)
    {
      value = _nullableValue.GetValueOrDefault();
      return HasValue;
    }

    public TSelect SelectOrDefault<TSelect>(Func<TValue, TSelect> selector)
    {
      return SelectOr(selector, default(TSelect));
    }

    public TSelect SelectOr<TSelect>(Func<TValue, TSelect> selector, TSelect defaultSelectValue)
    {
      return HasValue ? selector(Value) : defaultSelectValue;
    }

    public ValueOption<TSelect> SelectValueTypeOrNone<TSelect>(Func<TValue, TSelect> selector)
      where TSelect : struct
    {
      return HasValue ? ValueOption<TSelect>.Some(selector(Value)) : ValueOption<TSelect>.None;
    }

    public ValueOption<TSelect> SelectValueTypeOrNone<TSelect>(Func<TValue, ValueOption<TSelect>> selector)
      where TSelect : struct
    {
      return HasValue ? selector(Value) : ValueOption<TSelect>.None;
    }

    public Option<TSelect> SelectOrNone<TSelect>(Func<TValue, TSelect> selector)
      where TSelect : class
    {
      return HasValue ? Option<TSelect>.Some(selector(Value)) : Option<TSelect>.None;
    }

    public Option<TSelect> SelectOrNone<TSelect>(Func<TValue, Option<TSelect>> selector)
      where TSelect : class
    {
      return HasValue ? selector(Value) : Option<TSelect>.None;
    }

    public TValue ValueOrThrow(Exception exception)
    {
      if (HasValue)
        return Value;
      throw exception;
    }

    public TValue ValueOrThrowUsing(Action throwAction)
    {
      if (HasValue)
        return Value;
      throwAction();
      throw new ArgumentException("no exception thrown", "throwAction");
    }

    public void IfHasValue(Action<TValue> action)
    {
      if (HasValue)
        action(Value);
    }
  }
}
