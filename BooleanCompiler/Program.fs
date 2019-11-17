// Learn more about F# at http://fsharp.org

open System
open BooleanParser
open FParsec
open AST

[<EntryPoint>]
let main argv =
    test parseExpression "!!A&(!B|C)"
    0
