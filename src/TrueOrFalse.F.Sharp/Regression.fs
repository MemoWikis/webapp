namespace TrueFalse.FSharp

open System
open RDotNet
open RProvider
open RProvider.graphics
open RProvider.stats

type Regression() = 
    member this.Exp() =

        let rng = Random()
        let rand () = rng.NextDouble()

        let X1s = [ for i in 0 .. 9 -> 10. * rand () ]
        let X2s = [ for i in 0 .. 9 -> 5. * rand () ]

        let Ys = [ for i in 0 .. 9 -> 5. + 3. * X1s.[i] - 2. * X2s.[i] + rand () ]

        let dataset =
            namedParams [
                "Y", box Ys;
                "X1", box X1s;
                "X2", box X2s; ]
            |> R.data_frame

        let result = R.lm(formula = "Y~X1+X2", data = dataset)
        result.AsList