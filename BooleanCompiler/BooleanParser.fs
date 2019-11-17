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
//  --------------------------------------------------------------

// testing the parser p with string str
let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

 // make functions for the AST nodes ----------
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
// ---------------------------------------------------------------

// BoolParser with FParsec Combinators ---------------------------

// whitespaces    
let ws = spaces
// expect parses string s with optional leading whitespaces 
let expect s :Parser<string, unit> =
    ws >>. pstring s


let parseIdentifier:Parser<string, unit> = ws >>. many1SatisfyL isAsciiLetter "identifier"

// parseVariable parses the following grammar:
// Variable := [a-zA-Z_][a-zA-Z_0-9]* and puts the result into a AST Variable 
let parseVariable = (parseIdentifier |>> fun x -> Var(x)) 

// parseExclamationMarks parses the following grammar:
// "!"*
let parseExclamationMarks:Parser<int, unit> =  many (expect "!") |>> fun x -> List.length x

let parseExpression, exprImpl = createParserForwardedToRef()
let parseOr, orImpl = createParserForwardedToRef()
let parseAnd, andImpl = createParserForwardedToRef()

// parseAtom parses the followiong grammar:
// Atom := Variable | "(" ^ Expression ^ ")
let parseAtom = parseVariable <|> (expect "(" >>. parseExpression .>> expect ")") 

// parseNot parses the following grammar:
// Not := "!"* ^ Atom
let parseNot = parseExclamationMarks .>>. parseAtom |>> fun (x,y) -> makeNot x y 

// parseAnd parses the following grammar:
// And := Not ^ ("&" ^ And)?
do andImpl := (parseNot .>>. opt (expect "&" >>. parseAnd)) |>> makeAnd

// parseOr parses the following grammar:
// Or := And ^ ("|" ^ Or)?
do orImpl := (parseAnd .>>. opt (expect "|" >>. parseOr)) |>> makeOr

// parseExpression parses the following grammar:
// Expression := Or Spaces*
do exprImpl := parseOr .>> opt ws
// -----------------------------------------------------------------