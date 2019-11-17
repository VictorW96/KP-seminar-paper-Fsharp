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

let rec makeNot num node =
    if num <= 0 then node
    else Not(makeNot (num-1) node)

let makeAnd (argument: Node*Node option) =
    match snd argument with
    | None -> fst argument
    | Some node -> And(fst argument, node) 

let makeOr (argument: Node*Node option) =
    match snd argument with
    | None -> fst argument
    | Some node -> Or(fst argument, node) 
    
let ws = spaces

let first (x,y) = x

let second (x,y) = y

let parseIdentifier:Parser<string, unit> = ws >>. many1SatisfyL isAsciiLetter "identifier"

let parseVariable = parseIdentifier |>> fun x -> Var(x)

let parseExclamationMarks:Parser<int, unit> =  many (expect "!") |>> fun x -> List.length x

let rec parseExpression = parseOr .>> opt ws

and parseOr = (parseAnd .>>. opt (expect " |" >>. parseOr)) |>> makeOr

and parseAnd = (parseNot .>>. opt (expect " &" >>. parseAnd)) |>> makeAnd

and parseAtom = parseVariable <|> (expect "(" >>. parseExpression .>> expect ")")

and parseNot = parseExclamationMarks .>>. parseAtom |>> fun (x,y) -> makeNot x y 