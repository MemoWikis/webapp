#if INTERACTIVE
#r "../packages/R.NET.Community.1.6.4/lib/net40/RDotNet.dll"
#r "../packages/R.NET.Community.FSharp.1.6.4/lib/net40/RDotNet.FSharp.dll"
#r "../packages/R.NET.Community.1.6.4/lib/net40/RDotNet.NativeLibrary.dll"
#r "../packages/RProvider.1.1.14/lib/net40/RProvider.dll"
#r "../packages/RProvider.1.1.14/lib/net40/RProvider.Runtime.dll"
#endif

#load "Regression.fs"

open System
open TrueFalse.FSharp

let TestRegression() = 
    let regression = Regression()
    regression.Exp()


type Test() =
    member this.foo() =
        "ABC "

let t = Test()