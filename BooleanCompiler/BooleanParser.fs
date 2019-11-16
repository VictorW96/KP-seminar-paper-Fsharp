module BooleanParser

open FParsec
open AST

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


let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg


let expect s =
    spaces >>. pstring s

let makeOr argument =
    match snd argument with 
        | None -> fst argument 
        | _ -> Or(fst argument, snd argument)

let makeAnd argument =
    match snd argument with
        | None -> fst argument
        | _ -> And(fst argument, snd argument)

let rec makeNot num node =
    if num <= 0 then node
    else Not(makeNot (num-1) node)

let ExpectIdentifier = 
    many satisfy
    

let parseVariable =
    let innerFnc input =