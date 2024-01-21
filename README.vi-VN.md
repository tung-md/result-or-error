<div align="center">

<img src="assets/logo.png" alt="drawing" width="700px"/></br>

[![ResultOrError - NuGet](https://img.shields.io/nuget/v/resultorerror.svg?label=ResultOrError%20-%20nuget)](https://www.nuget.org/packages/resultorerror) [![NuGet](https://img.shields.io/nuget/dt/resultorerror.svg)](https://www.nuget.org/packages/resultorerror) [![Build Status](../../actions/workflows/build.yml/badge.svg)](../../actions/workflows/build.yml)

[![en](https://img.shields.io/badge/lang-en-green.svg)](README.md)
![vi](https://img.shields.io/badge/lang-vi-red.svg)

---

### Kết hợp đơn giản hợp nhất một kết quả hoặc một lỗi

`dotnet add package ResultOrError`

</div>

- [Bắt đầu 🏃](#bắt-đầu-)
  - [Thay thế việc ném ngoại lệ bằng `ResultOrError<T>`](#thay-thế-việc-ném-ngoại-lệ-bằng-resultorerrort)
  - [Trả lại nhiều lỗi khi cần thiết](#trả-lại-nhiều-lỗi-khi-cần-thiết)
- [Tạo một cá thể `ResultOrError`](#tạo-một-cá-thể-resultorerror)
  - [Sử dụng chuyển đổi ngầm](#sử-dụng-chuyển-đổi-ngầm)
  - [Sử dụng `ResultOrErrorFactory`](#sử-dụng-resultorerrorfactory)
  - [Sử dụng phương pháp mở rộng `ToResultOrError`](#sử-dụng-phương-pháp-mở-rộng-toresultorerror)
- [Thuộc tính](#thuộc-tính)
  - [`IsError`](#iserror)
  - [`Value`](#value)
  - [`Errors`](#errors)
  - [`FirstError`](#firsterror)
  - [`ErrorsOrEmptyList`](#errorsoremptylist)
- [Phương thức](#phương-thức)
  - [`Match`](#match)
    - [`Match`](#match-1)
    - [`MatchAsync`](#matchasync)
    - [`MatchFirst`](#matchfirst)
    - [`MatchFirstAsync`](#matchfirstasync)
  - [`Switch`](#switch)
    - [`Switch`](#switch-1)
    - [`SwitchAsync`](#switchasync)
    - [`SwitchFirst`](#switchfirst)
    - [`SwitchFirstAsync`](#switchfirstasync)
  - [`Then`](#then)
    - [`Then`](#then-1)
    - [`ThenAsync`](#thenasync)
    - [Kết hợp `Then` và `ThenAsync`](#kết-hợp-then-và-thenasync)
  - [`Else`](#else)
    - [`Else`](#else-1)
    - [`ElseAsync`](#elseasync)
- [Kết hợp tính năng (`Then`, `Else`, `Switch`, `Match`)](#kết-hợp-tính-năng-then-else-switch-match)
- [Các loại lỗi](#các-loại-lỗi)
  - [Các loại lỗi tích hợp](#các-loại-lỗi-tích-hợp)
  - [Tùy chỉnh loại lỗi](#tùy-chỉnh-loại-lỗi)
- [Các loại kết quả tích hợp (`Result.Success`, ..)](#các-loại-kết-quả-tích-hợp-resultsuccess-)
- [Tổ chức lỗi](#tổ-chức-lỗi)
- [Mediator + FluentValidation + `ResultOrError` 🤝](#mediator--fluentvalidation--resultorerror-)
- [Đóng góp 🤲](#đóng-góp-)
- [Giấy phép 🪪](#giấy-phép-)

# Bắt đầu 🏃

## Thay thế việc ném ngoại lệ bằng `ResultOrError<T>`

Cái này 👇

```cs
public float Divide(int a, int b)
{
    if (b == 0)
    {
        throw new Exception("Cannot divide by zero");
    }

    return a / b;
}

try
{
    var result = Divide(4, 2);
    Console.WriteLine(result * 2); // 4
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    return;
}
```

Biến thành thế này 👇

```cs
public ResultOrError<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return Error.Unexpected(description: "Cannot divide by zero");
    }

    return a / b;
}

var result = Divide(4, 2);

if (result.IsError)
{
    Console.WriteLine(result.FirstError.Description);
    return;
}

Console.WriteLine(result.Value * 2); // 4
```

Hoặc sử dụng [Then](#then--thenasync)/[Else](#else--elseasync) và [Switch](#switch--switchasync)/[Match](#match--matchasync), bạn có thể thực hiện việc này 👇

```cs

Divide(4, 2)
    .Then(val => val * 2)
    .SwitchFirst(
        onValue: Console.WriteLine, // 4
        onFirstError: error => Console.WriteLine(error.Description));
```

## Trả lại nhiều lỗi khi cần thiết

Bên trong, đối tượng `ResultOrError` có một danh sách `Error`s, vì vậy nếu có nhiều lỗi, bạn không cần phải thoả hiệp và chỉ có lỗi đầu tiên.

```cs
public class User(string _name)
{
    public static ResultOrError<User> Create(string name)
    {
        List<Error> errors = [];

        if (name.Length < 2)
        {
            errors.Add(Error.Validation(description: "Name is too short"));
        }

        if (name.Length > 100)
        {
            errors.Add(Error.Validation(description: "Name is too long"));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(description: "Name cannot be empty or whitespace only"));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new User(name);
    }
}
```

# Tạo một cá thể `ResultOrError`

## Sử dụng chuyển đổi ngầm

Có các bộ chuyển đổi ngầm định từ `TResult`, `Error`, `List<Error>` sang `ResultOrError<TResult>`

```cs
ResultOrError<int> result = 5;
ResultOrError<int> result = Error.Unexpected();
ResultOrError<int> result = [Error.Validation(), Error.Validation()];
```

```cs
public ResultOrError<int> IntToResultOrError()
{
    return 5;
}
```

```cs
public ResultOrError<int> SingleErrorToResultOrError()
{
    return Error.Unexpected();
}
```

```cs
public ResultOrError<int> MultipleErrorsToResultOrError()
{
    return [
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ];
}
```

## Sử dụng `ResultOrErrorFactory`

```cs
ResultOrError<int> result = ResultOrErrorFactory.From(5);
ResultOrError<int> result = ResultOrErrorFactory.From<int>(Error.Unexpected());
ResultOrError<int> result = ResultOrErrorFactory.From<int>([Error.Validation(), Error.Validation()]);
```

```cs
public ResultOrError<int> GetValue()
{
    return ResultOrErrorFactory.From(5);
}
```

```cs
public ResultOrError<int> SingleErrorToResultOrError()
{
    return ResultOrErrorFactory.From<int>(Error.Unexpected());
}
```

```cs
public ResultOrError<int> MultipleErrorsToResultOrError()
{
    return ResultOrErrorFactory.From([
        Error.Validation(description: "Invalid Name"),
        Error.Validation(description: "Invalid Last Name")
    ]);
}
```

## Sử dụng phương pháp mở rộng `ToResultOrError`

```cs
ResultOrError<int> result = 5.ToResultOrError();
ResultOrError<int> result = Error.Unexpected().ToResultOrError<int>();
ResultOrError<int> result = new[] { Error.Validation(), Error.Validation() }.ToResultOrError<int>();
```

# Thuộc tính

## `IsError`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    // the result contains one or more errors
}
```

## `Value`

```cs
ResultOrError<int> result = User.Create();

if (!result.IsError) // the result contains a value
{
    Console.WriteLine(result.Value);
}
```

## `Errors`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    result.Errors // contains the list of errors that occurred
        .ForEach(error => Console.WriteLine(error.Description));
}
```

## `FirstError`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    var firstError = result.FirstError; // only the first error that occurred
    Console.WriteLine(firstError == result.Errors[0]); // true
}
```

## `ErrorsOrEmptyList`

```cs
ResultOrError<int> result = User.Create();

if (result.IsError)
{
    result.ErrorsOrEmptyList // List<Error> { /* one or more errors */  }
    return;
}

result.ErrorsOrEmptyList // List<Error> { }
```

# Phương thức

## `Match`

Phương thức `Match` nhận được hai hàm `onValue` và `onError`, `onValue` sẽ được gọi nếu kết quả thành công và `onError` được gọi nếu kết quả là lỗi.

### `Match`

```cs
string foo = result.Match(
    value => value,
    errors => $"{errors.Count} errors occurred.");
```

### `MatchAsync`

```cs
string foo = await result.MatchAsync(
    value => Task.FromResult(value),
    errors => Task.FromResult($"{errors.Count} errors occurred."));
```

### `MatchFirst`

Phương thức `MatchFirst` nhận được hai hàm `onValue` và `onError`, `onValue` sẽ được gọi nếu kết quả thành công và `onError` được gọi nếu kết quả là lỗi.

Không giống như `Match`, nếu trạng thái có lỗi, hàm `onError` của `MatchFirst` chỉ nhận được lỗi đầu tiên xảy ra chứ không phải toàn bộ danh sách lỗi.

```cs
string foo = result.MatchFirst(
    value => value,
    firstError => firstError.Description);
```

### `MatchFirstAsync`

```cs
string foo = await result.MatchFirstAsync(
    value => Task.FromResult(value),
    firstError => Task.FromResult(firstError.Description));
```

## `Switch`

Phương thức `Switch` nhận được hai hành động `onValue` và `onError`, `onValue` sẽ được gọi nếu kết quả thành công và `onError` được gọi nếu kết quả là lỗi.

### `Switch`

```cs
result.Switch(
    value => Console.WriteLine(value),
    errors => Console.WriteLine($"{errors.Count} errors occurred."));
```

### `SwitchAsync`

```cs
await result.SwitchAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    errors => { Console.WriteLine($"{errors.Count} errors occurred."); return Task.CompletedTask; });
```

### `SwitchFirst`

Phương thức `SwitchFirst` nhận được hai hành động `onValue` và `onError`, `onValue` sẽ được gọi nếu kết quả thành công và `onError` được gọi nếu kết quả là lỗi.

Không giống như `Switch`, nếu trạng thái có lỗi, hàm `onError` của `SwitchFirst` chỉ nhận được lỗi đầu tiên xảy ra chứ không phải toàn bộ danh sách lỗi.

```cs
result.SwitchFirst(
    value => Console.WriteLine(value),
    firstError => Console.WriteLine(firstError.Description));
```

###  `SwitchFirstAsync`

```cs
await result.SwitchFirstAsync(
    value => { Console.WriteLine(value); return Task.CompletedTask; },
    firstError => { Console.WriteLine(firstError.Description); return Task.CompletedTask; });
```

## `Then`

### `Then`

`Then` nhận một hành động hoặc một hàm và chỉ gọi nó nếu kết quả không phải là lỗi.

```cs
ResultOrError<int> foo = result
    .Then(val => val * 2);
```

Nhiều phương thức `Then` có thể được xâu chuỗi lại với nhau.

```cs
ResultOrError<string> foo = result
    .Then(val => val * 2)
    .Then(val => $"The result is {val}");
```

Nếu bất kỳ phương thức nào trả về lỗi, chuỗi sẽ bị hỏng và lỗi sẽ được trả về.

```cs
ResultOrError<int> Foo() => Error.Unexpected();

ResultOrError<string> foo = result
    .Then(val => val * 2)
    .Then(_ => GetAnError())
    .Then(val => $"The result is {val}") // this function will not be invoked
    .Then(val => $"The result is {val}"); // this function will not be invoked
```

### `ThenAsync`

`ThenAsync` nhận một hành động hoặc hàm không đồng bộ và chỉ gọi nó nếu kết quả không phải là lỗi.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .ThenAsync(val => Task.FromResult($"The result is {val}"));
```

### Kết hợp `Then` và `ThenAsync`

Bạn có thể kết hợp các phương thức `Then` và `ThenAsync` lại với nhau.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .Then(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}");
```

## `Else`

`Else` nhận một giá trị hoặc một hàm. Nếu kết quả là lỗi, `Else` sẽ trả về giá trị hoặc gọi hàm. Ngược lại, nó sẽ trả về giá trị của kết quả.

### `Else`

```cs
ResultOrError<string> foo = result
    .Else("fallback value");
```

```cs
ResultOrError<string> foo = result
    .Else(errors => $"{errors.Count} errors occurred.");
```

### `ElseAsync`

```cs
ResultOrError<string> foo = await result
    .ElseAsync(Task.FromResult("fallback value"));
```

```cs
ResultOrError<string> foo = await result
    .ElseAsync(errors => Task.FromResult($"{errors.Count} errors occurred."));
```

# Kết hợp tính năng (`Then`, `Else`, `Switch`, `Match`)

Bạn có thể kết hợp các phương thức `Then`, `Else`, `Switch` và `Match` lại với nhau.

```cs
ResultOrError<string> foo = await result
    .ThenAsync(val => Task.Delay(val))
    .Then(val => Console.WriteLine($"Finsihed waiting {val} seconds."))
    .ThenAsync(val => Task.FromResult(val * 2))
    .Then(val => $"The result is {val}")
    .Else(errors => Error.Unexpected())
    .MatchFirst(
        value => value,
        firstError => $"An error occurred: {firstError.Description}");
```

# Các loại lỗi

Mỗi phiên bản `Error` có một thuộc tính `Type`, là giá trị enum đại diện cho loại lỗi.

## Các loại lỗi tích hợp

Các loại lỗi sau được xây dựng sẵn:

```cs
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
}
```

Mỗi loại lỗi có một phương thức tĩnh tạo ra lỗi thuộc loại đó. Ví dụ:

```cs
var error = Error.NotFound();
```

theo tùy chọn, bạn có thể chuyển mã, mô tả và siêu dữ liệu cho lỗi:

```cs
var error = Error.Unexpected(
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen",
    metadata: new Dictionary<string, object>
    {
        { "user", user },
    });
```
Enum `ErrorType` là một cách tốt để phân loại lỗi.

## Tùy chỉnh loại lỗi 

Bạn có thể tạo các loại lỗi của riêng mình nếu bạn muốn phân loại các lỗi của mình theo cách khác.


Một loại lỗi tùy chỉnh có thể được tạo bằng phương pháp tĩnh `Custom`

```cs
public static class MyErrorTypes
{
    const int ShouldNeverHappen = 12;
}

var error = Error.Custom(
    type: MyErrorTypes.ShouldNeverHappen,
    code: "User.ShouldNeverHappen",
    description: "A user error that should never happen");
```

Bạn có thể sử dụng phương thức `Error.NumericType` để truy xuất kiểu số của lỗi.

```cs
var errorMessage = Error.NumericType switch
{
    MyErrorType.ShouldNeverHappen => "Consider replacing dev team",
    _ => "An unknown error occurred.",
};
```

# Các loại kết quả tích hợp (`Result.Success`, ..)

Một số loại kết quả được xây dựng sẵn:

```cs
ResultOrError<Success> result = Result.Success;
ResultOrError<Created> result = Result.Created;
ResultOrError<Updated> result = Result.Updated;
ResultOrError<Deleted> result = Result.Deleted;
```

Có thể được sử dụng như sau

```cs
ResultOrError<Deleted> DeleteUser(Guid id)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user is null)
    {
        return Error.NotFound(description: "User not found.");
    }

    await _userRepository.DeleteAsync(user);
    return Result.Deleted;
}
```

# Tổ chức lỗi

Một cách tiếp cận hay là tạo một lớp tĩnh với các lỗi dự kiến. Ví dụ:

```cs
public static partial class DivisionErrors
{
    public static Error CannotDivideByZero = Error.Unexpected(
        code: "Division.CannotDivideByZero",
        description: "Cannot divide by zero.");
}
```

Có thể được sử dụng như sau 👇

```cs
public ResultOrError<float> Divide(int a, int b)
{
    if (b == 0)
    {
        return DivisionErrors.CannotDivideByZero;
    }

    return a / b;
}
```

# [Mediator](https://github.com/jbogard/MediatR) + [FluentValidation](https://github.com/FluentValidation/FluentValidation) + `ResultOrError` 🤝

Một cách tiếp cận phổ biến khi sử dụng `MediatR` là sử dụng `FluentValidation` để xác thực yêu cầu trước khi nó đến trình xử lý.

Thông thường, việc xác thực được thực hiện bằng cách sử dụng `Behavior` đưa ra ngoại lệ nếu yêu cầu không hợp lệ.

Bằng cách sử dụng `ResultOrError`, chúng ta có thể tạo một `Behavior` trả về một lỗi thay vì đưa ra một ngoại lệ.

Điều này hoạt động tốt khi dự án sử dụng `ResultOrError`, với lớp gọi `Mediator`, tương tự như các thành phần khác trong dự án, chỉ cần nhận được một `ResultOrError` và có thể xử lý nó cho phù hợp.

Dưới đây là ví dụ về `Behavior` xác thực yêu cầu và trả về lỗi nếu yêu cầu đó không hợp lệ 👇

```cs
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResultOrError
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        return (dynamic)errors;
    }
}
```

# Đóng góp 🤲

Nếu bạn có bất kỳ câu hỏi, nhận xét hoặc đề xuất nào, vui lòng mở một vấn đề hoặc tạo yêu cầu kéo 🙂

# Giấy phép 🪪

Dự án này được cấp phép theo các điều khoản của giấy phép [MIT](https://github.com/tung-md/result-or-error/blob/main/LICENSE).
