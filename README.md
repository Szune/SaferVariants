# SaferVariants

⚠ N.B.: This branch is for the WIP 2.0.0, there are still a lot of things to fix and tests to update.

## Boring explanation
An alternative to returning null and throwing exceptions as indications of a value being present and errors occurring.

The only purpose of this library is to (to the extent that it is feasible) force checking if a value is present/ok before using it.

Currently, the way that that is achieved is by having structs where the default value represents the missing/error value and to get access to the inner value you call the methods that either explicitly handle the case where the value is not present or are explicit about what will happen in those cases.

Inspired by functional programming languages.

## Code
N.B.: Instead of only using `var` in the examples, some types are shown to make it easier to follow.

1. Option.TryGetValue(out value), Result.TryGetValue(out value), Result.TryGetError(out error)
2. Returning/creating Option/Result values
3. Using Map(), Bind() and ValueOr()
4. Using IfSome(), IfNone(), IfOk(), IfError()
5. Using HandleError() for more fluent handling of Result

## 1. Option.TryGetValue(out value), Result.TryGetValue(out value), Result.IsErr(out error)
### Option.TryGetValue
```c#
Option<int> length = 10;
if (length.TryGetValue(out var value))
{
    Console.WriteLine($"The length was {value}");
}
else
{
    Console.WriteLine("Failed to get length");
}
```

### Result.TryGetValue
```c#
Result<int, string> length = 10;
if (length.TryGetValue(out var value))
{
    Console.WriteLine($"The length was {value}");
}
else
{
    Console.WriteLine("Failed to get length");
}
```

### Result.TryGetError
```c#
Result<int, string> length = "Failed to parse length";
if (length.TryGetValue(out var value))
{
    Console.WriteLine($"The length was {value}");
}
else if (length.TryGetError(out var error))
{
    Console.WriteLine($"Failed to get length: {error}");
}
```

## 2. Returning/creating Option/Result values
### Returning/creating Option
Using `Option.None` or `Option.Some()`
```c#
Option<int> GetLength(string? s)
{
    if (s == null)
    {
        return Option.None;
    }
    return Option.Some(s.Length);
}
```

Using `Option.NoneIfNull()`
```c#
Option<string> ReplaceWorldWithBird(string? s)
{
    return Option.NoneIfNull(s?.Replace("world", "bird"));
}
```

Implicit conversion from null:
```c#
string? _value;
Option<string> GetValue() {
    return _value; // None if _value is null
}
```

### Returning/creating Result
Using `Result.Error()`  or `Result.Ok()`
```c#
Result<int,string> GetLength(string? s)
{
    if (s == null)
    {
        return Result.Error<int, string>($"{nameof(s)} was null");
    }
    return Result.Ok<int,string>(s.Length); // no type inference here either, please accept my apologies
}
```

Implicit conversion:
```c#
Result<int,string> GetLength(string? s)
{
    if (s == null)
    {
        return $"{nameof(s)} was null";
    }
    return s.Length;
}
```

## 3. Using Map(), Bind() and ValueOr()
### On an Option
```c#
Option<string> str = GetString();
Option<int> length = str.Map(s => s.Length);
Option<int> length2 = str.Bind(s => Option.Some(s.Length));
Option<int> noLength = str.Bind(_ => Option<int>.None);

Console.WriteLine($"The length was {length.ValueOr(0)}");
```

### On a Result
```c#
enum EType
{
    StringTooShort
}

Result<string, EType> str = GetString();
Result<int, EType> length = str.Bind(s =>
    // this is only called if `str` is ok
    s.Length > 1
        ? Result.Ok<int, EType>(s.Length)
        : Result.Error<int, EType>(EType.StringTooShort));
Result<int, EType> length2 = str.Map(s =>
    // this is only called if `str` is ok
    s.Length);

Console.WriteLine($"The length was {length.ValueOr(0)}");
if (length.TryGetError(out var err))
{
    Console.WriteLine($"There was an error btw, {err}");
}
```

## 4. Using IfSome(), IfNone(), IfOk(), IfError()
```c#
Option<int> length = GetLength()
    .IfNone(() => Console.WriteLine("No length"))
    .IfSome(len => Console.WriteLine($"Length: {len}"));

Result<int,int> length2 = GetLength2()
    .IfError(error => Console.WriteLine($"Error: {error}"))
    .IfOk(len => Console.WriteLine($"Length: {len}"));
```

## 5. Using HandleError() for more fluent handling of Result

HandleError() is mostly useful for error handling when method chaining:

```c#
// chaining methods
Option<string> str = 
    DoImportantThing()
        .HandleError(err => Console.WriteLine($"Error: {err}"))
        .Map(it => it.ToString());
        
// the types of the individual steps
Result<ImportantValue, SomeError> result = DoImportantThing();

Option<ImportantValue> errorHandledResult =
    result.HandleError(err => Console.WriteLine($"Error: {err}"));
    
Option<string> str = errorHandledResult.Map(it => it.ToString());
```
