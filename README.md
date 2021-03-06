[![Build status](https://ci.appveyor.com/api/projects/status/2qb0m0g484qdqaae/branch/master?svg=true)](https://ci.appveyor.com/project/taylorjg/fscheckutils/branch/master)

## Introduction

FsCheckUtils provides the facilities described in the following sections.

## NuGet Package

FsCheckUtils is available as a NuGet package:

* https://www.nuget.org/packages/FsCheckUtils/

## Convenience extension methods

### Config

<code>FsCheck.Config</code> is an immutable class. The following extension methods create a new
<code>FsCheck.Config</code> object with an updated value.

* Config.WithMaxTest
* Config.WithMaxFail
* Config.WithReplay
* Config.WithNoReplay
* Config.WithName
* Config.WithStartSize
* Config.WithEndSize
* Config.WithEvery
* Config.WithEveryShrink
* Config.WithArbitrary
* Config.WithRunner

In F#, we can do this:

```F#
let config = { Config.Default with MaxTest = 1000; Name = "My Config" }
```

With the above extension methods, we can achieve something pretty similar in C#:

```C#
var config = Config.Default
                    .WithMaxTest(1000)
                    .WithName("My Config");
```

### Configuration

<code>FsCheck.Fluent.Configuration</code> is a mutable version of <code>FsCheck.Config</code>
from the fluent part of the FsCheck API. <code>SpecBuilder.Check</code> takes a <code>FsCheck.Fluent.Configuration</code> parameter.
The following method makes it easy to create an instance of <code>FsCheck.Fluent.Configuration</code> from an instance of <code>FsCheck.Config</code>.

* Config.ToConfiguration

As mentioned above, <code>FsCheck.Fluent.Configuration</code> is mutable.
However, the following extension methods allow multiple properties to be set in a chained
style.

* Configuration.WithMaxTest
* Configuration.WithMaxFail
* Configuration.WithName
* Configuration.WithStartSize
* Configuration.WithEndSize
* Configuration.WithEvery
* Configuration.WithEveryShrink
* Configuration.WithRunner

```C#
var configuration = Config.Default
                            .ToConfiguration()
                            .WithMaxTest(1000)
                            .WithName("My Configuration");
```

## Wrappers around FsCheck operators

FsCheck's Prop operators e.g. <code>.&.</code>, are visible to C# as static methods with names that begin with <code>op&#95;</code> and have symbol names in place of the symbols themselves e.g. <code>op&#95;DotAmpDot</code>. It is possible to call these static methods from C# (as a normal method call - not as an infix operator). However, the names are very odd and ReSharper thinks they are errors (even in ReSharper 9.0 Update 1 - see the screenshot below). For these reasons, FsCheckUtils
provides wrappers around these operators.

![ReSharper error](https://raw.githubusercontent.com/taylorjg/FsCheckUtils/master/Images/ReSharperError.png "ReSharper error")

* PropExtensions.And (wraps .&.)
* PropExtensions.AndAll (a convenience method that applies .&. to a params array of properties)
* PropExtensions.Or (wraps .|.)
* PropExtensions.OrAll (a convenience method that applies .|. to a params array of properties)
* PropExtensions.Label (wraps |@)
* PropExtensions.Implies (wraps ==>)

## Generators and combinators that are in ScalaCheck but not in FsCheck

* GenExtensions.PickValues (Gen.pick)
* GenExtensions.PickGenerators (Gen.pick)
* GenExtensions.SomeOfValues (Gen.someOf)
* GenExtensions.SomeOfGenerators (Gen.someOf)
* GenExtensions.RetryUntil (Gen.retryUntil)
* GenExtensions.NumChar (Gen.numChar)
* GenExtensions.AlphaUpperChar (Gen.alphaUpperChar)
* GenExtensions.AlphaLowerChar (Gen.alphaLowerChar)
* GenExtensions.AlphaChar (Gen.alphaChar)
* GenExtensions.AlphaNumChar (Gen.alphaNumChar)
* GenExtensions.AlphaStr (Gen.alphaStr)
* GenExtensions.NumStr (Gen.numStr)
* GenExtensions.Identifier (Gen.identifier)
* GenExtensions.Guid (Gen.uuid)
* GenExtensions.Zip (Gen.zip)

## Documentation

Documentation, built from XML documentation comments using
[Sandcastle Help File Builder](https://github.com/EWSoftware/SHFB), can be found here:

* http://taylorjg.github.io/FsCheckUtils/index.html

## TODO

* Ensure all public methods are covered by unit tests (only a couple aren't)
* Write some GitHub wiki articles about using FsCheck from C#

## Using FsCheck from C#: some useful resources 

* https://github.com/fsharp/FsCheck/blob/master/examples/FsCheck.CSharpExamples/Program.cs
* http://fortysix-and-two.blogspot.co.uk/2009/05/announcing-fscheck-06-dot-is-new-pipe.html
* http://bugsquash.blogspot.co.uk/2014/02/generating-immutable-instances-in-c.html
* https://github.com/taylorjg/AnIntroductionToFsCheckTesting
* https://github.com/taylorjg/ScalaCheckBookExamplesInFsCheck
* https://github.com/taylorjg/DontShrinkTests
* https://github.com/taylorjg/CustomShrinkerTests