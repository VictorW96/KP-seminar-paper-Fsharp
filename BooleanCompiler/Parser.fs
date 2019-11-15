module Parser

open AST
open FParsec
open System

// grammar
type Factor = 
    | Variable of Variable
    | Factor of Factor //not
    | Expression of Expression

and Term = Factor * Factor option //or
and Expression = Term * Term option //and
and Variable = string


let nextToken(input: List<String>) =
    match input with
        | head :: tail -> (head, tail)
        | [] -> ("",[])

let isVar(input:string) =
    match String.length input with 
        | 0 -> false
        | _ -> true
    
let parseVariable(input:string) : Node =
    match input |> isVar with 
        | false -> printf "parsing failed"; None
        | true -> Var(input)

let rec parseExpression (input:List<string>) : Node =
    parseTerm input 

and parseTerm (input:List<string>)  : Node =
    parseFactor input
    

and parseFactor (input:List<string>)  : Node =
    let headtail = input |> nextToken
    match headtail |> fst with 
        | "" -> None
        | "!" -> Not(parseFactor(headtail |> snd))
        | "(" -> parseExpression(headtail |> snd)
        | _ -> parseVariable(headtail |> fst)

let parse (input:List<string>) (vars:Map<string,bool>) : bool =
    parseExpression input |> eval vars