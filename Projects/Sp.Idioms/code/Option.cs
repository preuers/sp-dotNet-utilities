using System;

namespace Sp.Idioms
{
  /// <summary>
  /// <para>Provides logical optionality for reference-types.</para>
  /// <para>Some .net languages do not provide a built-in concept to represent logical optionality of values.
  /// But since this is a rather common need <see cref="Option{T}"/> is intended to provide consistent representation of
  /// logical optionality in a convenient way as a library type instead.</para>
  /// <para>Using such a consistent representation of logical optionality improves the readability of code and is especially
  /// useful for the expressiveness of interfaces (in- and output parameters of interface-methods), since interface-clients
  /// and -implementation are typically not done by the same person. Especially directly using <code>null</code> to represent
  /// logical optionality is a widespread idiom. But this approach has the downside, that the parameter type does not convey
  /// the information of optionality, so a client has to rely on proper naming of parameters, documentation of their meaning,
  /// or a self-expanatory context. All of them are problematic and easily become a source of error and missunderstandings,
  /// especially since they are hard to get consistently used through out an application. Using the type to express logical 
  /// optionality is also a good practice, since modern development environments provide type information in a very efficient 
  /// way in terms of development speed (e.g. type information is conveniently shown in code-completion features).</para>
  /// <para>For reference types the logical optionality can internally technically be represented by using <code>null</code>
  /// for the logical <see cref="Option{T}.None"/>. This allows an implementation of <see cref="Option{T}"/> as a struct with
  /// no runtime overhead at all in comparision to the widespread idiom of abusing the techical <code>null</code> to directly
  /// represent logical optionality. The downside of this approach is, that it does not at the same time allow to be used for
  /// value types. That's why a separate implementation <see cref="ValueOption{T}"/> is provided for value types. It is an
  /// intentional design decision that this separtion is in general less problematic than an unnecessary runtime overhead
  /// for reference types would be.</para>
  /// </summary>
  public struct Option<TValue> where TValue : class
  {
    public static readonly Option<TValue> None = new Option<TValue>(null);
    private readonly TValue _value;

    private Option(TValue value)
    {
      _value = value;
    }

    public static Option<TValue> Some(TValue value)
    {
      #region Preconditions

      if (value == null)
        throw new ArgumentNullException("value");

      #endregion

      return new Option<TValue>(value);
    }

    public static Option<TValue> FromSelectOrNull<TExpressionContext>(TExpressionContext context, Func<TExpressionContext, TValue> selector)
      where TExpressionContext : class
    {
      return context == null ? None : FromValueOrNull(selector(context));
    }

    public static Option<TValue> FromSelectOrNullWithCondition<TExpressionContext>(TExpressionContext context, Func<TExpressionContext, TValue> selector, Predicate<TExpressionContext> condition)
      where TExpressionContext : class
    {
      if (context == null)
        return None;
      return condition(context) ? FromValueOrNull(selector(context)) : None;
    }

    public static Option<TValue> FromValueOrNull(TValue valueOrNull)
    {
      return valueOrNull == null ? None : Some(valueOrNull);
    }

    public static Option<TValue> FromValueOrNullWithCondition(TValue valueOrNull, Predicate<TValue> condition)
    {
      if (valueOrNull == null)
        return None;
      return condition(valueOrNull) ? Some(valueOrNull) : None;
    }

    public bool HasValue
    {
      get { return _value != null; }
    }

    public TValue Value
    {
      get
      {
        if (_value == null)
          throw new InvalidOperationException("option does not have a value");
        return _value;
      }
    }

    public TValueDynamic ValueAs<TValueDynamic>() where TValueDynamic : class
    {
      return (TValueDynamic) (object) Value;
    }

    public bool TryGetValue(out TValue value)
    {
      if (HasValue)
      {
        value = Value;
        return true;
      }

      value = null;
      return false;
    }

    public bool TryGetValueAs<TValueDynamic>(out TValueDynamic value) where TValueDynamic : class
    {
      TValue result;
      if (TryGetValue(out result))
      {
        value = (TValueDynamic)(object)result;
        return true;
      }
      value = null;
      return false;
    }

    public TValue ValueOrNull
    {
      get { return _value; }
    }

    public TValue ValueOr(TValue defaultValue)
    {
      return HasValue ? Value : defaultValue;
    }

    public TSelect SelectOrDefault<TSelect>(Func<TValue, TSelect> selector)
    {
      return SelectOr(selector, default(TSelect));
    }

    public TSelect SelectOr<TSelect>(Func<TValue, TSelect> selector, TSelect defaultSelectValue)
    {
      return HasValue ? selector(_value) : defaultSelectValue;
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

    public Option<TSpecificResult> ToOption<TSpecificResult>() where TSpecificResult : class
    {
      return HasValue ? Option<TSpecificResult>.Some((TSpecificResult) (object) _value) : Option<TSpecificResult>.None;
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

    public static bool operator ==(Option<TValue> value1, Option<TValue> value2)
    {
      return value1._value == value2._value;
    }

    public static bool operator !=(Option<TValue> value1, Option<TValue> value2)
    {
      return value1._value != value2._value;
    }
  }
}
