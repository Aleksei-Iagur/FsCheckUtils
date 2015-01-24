
## Provides some convenience extension methods

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

## Provides wrappers around FsCheck operators

FsCheck's Prop operators e.g. <code>.&.</code>, are visible to C# as static methods with names that begin with <code>op&#95;</code> and have symbol names in place of the symbols themselves e.g. <code>op&#95;DotAmpDot</code>. It is possible to call these static methods from C# (as a normal method call - not as an infix operator). However, the names are very odd and ReSharper thinks they are errors (even in ReSharper 9 when I tried it recently). For these reasons, FsCheckUtils
provides wrappers around these operators.

* PropExtensions.And (wraps .&.)
* PropExtensions.AndAll (a convenience method that applies .&. to a params array of properties)
* PropExtensions.Or (wraps .|.)
* PropExtensions.OrAll (a convenience method that applies .|. to a params array of properties)
* PropExtensions.Label (wraps |@)
* PropExtensions.Implies (wraps ==>)

## Provides functionality that is in ScalaCheck but not in FsCheck

* GenExtensions.pick
* GenExtensions.someOf

## TODO

* NumChar
* AlphaUpperChar
* AlphaLowerChar
* AlphaChar
* AlphaNumChar

* Identifier
* AlphaStr
* NumStr

* Uuid
   * See http://en.wikipedia.org/wiki/Universally_unique_identifier#Version_4_.28random.29

* zip (arity 2 to 9)
    * Generates Tuple2 from g1/g2, Tuple3 from g1/g2/g3, etc

* retryUntil
* containerOf / Buildable