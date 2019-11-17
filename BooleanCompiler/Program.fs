// Learn more about F# at http://fsharp.org

open System
open BooleanParser
open FParsec
open AST

let splitKeyVal (str : string) =
    match str.Split(':') with
    |[|key; value|] ->  (key, System.Boolean.Parse(value))
    |_ -> invalidArg "str" "str must have the format key:value"

let createMap (str : string) =
    str.Split('|') 
    |> Array.map (splitKeyVal)
    |> Map

[<EntryPoint>]
let main args =
    if args.Length <> 2 then
        failwith "Error: Expected arguments <varBoolMap> of the form \"A:true|B:false|C:true\" and <stringToParse> of the form \"!A&(!!B|C)\""
    let stringvarBoolMap, stringToParse = args.[0], args.[1]
    let varBoolMap = createMap stringvarBoolMap

    match run parseExpression stringToParse with
        | Success(result, _, _)   -> printfn "Success: %A evaluated as %b" result (eval varBoolMap result)
        | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg
    0
