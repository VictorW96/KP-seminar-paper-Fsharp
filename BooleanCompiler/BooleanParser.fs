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


let expect s :Parser<string, unit> =
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


let ws = spaces

let parseIdentifier:Parser<string, unit> = ws >>. many1SatisfyL isAsciiLetter "identifier"

let parseVariable = parseIdentifier |>> fun x -> Var(x)

let parseExclamationMarks:Parser<int, unit> =  many (expect "!") |>> fun x -> List.length x

let parseAtom = parseVariable 

let parseNot = parseExclamationMarks .>>. parseAtom |>> fun (x,y) -> makeNot x y 