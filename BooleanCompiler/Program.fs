// Learn more about F# at http://fsharp.org

open System
open BooleanParser
open FParsec

[<EntryPoint>]
let main argv =
    test pfloat "23"
    0
