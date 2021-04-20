# SaferVariants

## Boring explanation
An alternative to returning null and throwing exceptions as indications of a value being present and errors occurring.

The only purpose of this library is to (to the extent that it is possible) force checking if a value is present/ok before using it.

Currently, the way that that is achieved is by having an interface that can either be pattern matched against to get access to the value or by calling the methods that explicitly handle the case where the value is not present.

Inspired by the programming language [Rust](https://www.rust-lang.org/).

## Code
1. Pattern matching
2. Returning/creating IOption/IResult values
3. Using Map()

## 1. Pattern matching
### Pattern matching an `IOption<T>`
Very straightforward:

```c#
IOption<int> length = GetLength();
if (length is Some<int> some)
{
    Console.WriteLine($"The length was {some.Value}");
}
else
{
    Console.WriteLine("Failed to get length");
}
```

### Pattern matching an `IResult<TValue,TError>`
A little more involved, depending on usage:

#### switch
```c#
IResult<int,string> length = GetLength();
switch (length)
{
    case Ok<int,string> ok:
        Console.WriteLine($"The length was {ok.Value}");
        break;
    case Err<int,string> err:
        Console.WriteLine($"Failed to get length: {err.Error}");
        break;
    default:
    // only happens if someone returns null instead of Ok/Err,
    // which.. yeah.. don't do that...
    // and for that reason, it's good to check for null in switch or if->else if
    // obviously not necessary in an if->else
        throw new NotImplementedException();
}
```

#### if -> else if
```c#
if (length is Ok<int,string> some)
{
    Console.WriteLine($"The length was {some.Value}");
}
else if (length is Err<int,string> err)
{
    Console.WriteLine($"Failed to get length: {err.Error}");
}
else
{
    throw new NotImplementedException();
}
```

#### if -> else (consider using IOption instead if the error is not used)
```c#
// if->else
if (length is Ok<int,string> some)
{
    Console.WriteLine($"The length was {some.Value}");
}
else
{
    Console.WriteLine("Failed to get length");
}
```

## 2. Returning/creating IOption/IResult values
### Returning/creating IOption
```c#
IOption<int> GetLength(string s)
{
    if (s == null)
    {
        return Option.None<int>(); // can't infer type on None, sorry
    }
    return Option.Some(s.Length); // works on Some at least
}
```

### Returning/creating IResult
```c#
IResult<int,string> GetLength(string s)
{
    if (s == null)
    {
        return Result.Err<int,string>($"{nameof(s)} was null"); // can't infer type on Err, sorry!
        // note: returning a string message as an error is not recommended
        //       in the case where you want to handle errors,
        //       it's preferable to at the very least return an enum
    }
    return Result.Ok<int,string>(s.Length); // no type inference here either, please accept my apologies
}
```

## 3. Using Map() and ValueOr()
### On an IOption
```c#
// you'd normally use `var` instead of typing out the whole type
// it's just written out here for clarity
IOption<string> str = GetString();
var length = str.Map(s =>
    // this is only called if `str` is `Some`
    Option.Some(s.Length));

Console.WriteLine($"The length was {length.ValueOr(0)}");
```

### On an IResult
```c#
enum EType
{
    StringTooShort
}

IResult<string,EType> str = GetString();
var length = str.Map(s =>
    // this is only called if `str` is `Ok`
    s.Length > 1
    ? Result.Ok<int,EType>(s.Length)
    : Result.Err<int,EType>(EType.StringTooShort));

Console.WriteLine($"The length was {length.ValueOr(0)}");
if (length is Err<int,EType> err)
{
    Console.WriteLine($"There was an error btw, {err.Error}");
}
```