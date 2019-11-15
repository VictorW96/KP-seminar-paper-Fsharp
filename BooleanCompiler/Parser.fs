module Parser

open AST
open FParsec
open System

//  ---------------------------------------------------------
// The expression should have the following EBNF form:
//  EBNF
//  ---------------------------------------------------------
// 	<expression> ::= <term> { <or> <term> }
// 	<term> ::= <factor> { <and> <factor> }
// 	<factor> ::= <var> | <not> <factor> | (<expression>)
// 	<or>  ::= '|'
// 	<and> ::= '&'
// 	<not> ::= '!'
//  <var> ::= '[a-zA-Z0-9]*'
//  ---------------------------------------------------------


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