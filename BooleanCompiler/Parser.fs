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


type Result<'a> =
    | Success of 'a
    | Failure of string 


type Parser<'T> = Parser of (string -> Result<'T * string>)

let pchar charToMatch = 
    // define a nested inner function
    let innerFn str =
        if String.IsNullOrEmpty(str) then
            Failure "No more input"
        else
            let first = str.[0] 
            if first = charToMatch then
                let remaining = str.[1..]
                Success (charToMatch,remaining)
            else
                let msg = sprintf "Expecting '%c'. Got '%c'" charToMatch first
                Failure msg
    // return the inner function
    Parser innerFn 

let run parser input = 
    // unwrap parser to get inner function
    let (Parser innerFn) = parser 
    // call inner function with input
    innerFn input


let andThen parser1 parser2 =
    let innerFn input =
        // run parser1 with the input
        let result1 = run parser1 input
        
        // test the result for Failure/Success
        match result1 with
        | Failure err -> 
            // return error from parser1
            Failure err  

        | Success (value1,remaining1) -> 
            // run parser2 with the remaining input
            let result2 =  run parser2 remaining1
            
            // test the result for Failure/Success
            match result2 with 
            | Failure err ->
                // return error from parser2
                Failure err 
            
            | Success (value2,remaining2) -> 
                // combine both values as a pair
                let newValue = (value1,value2)
                // return remaining input after parser2
                Success (newValue,remaining2)

    // return the inner function
    Parser innerFn 

let ( .>>. ) = andThen 

let orElse parser1 parser2 =
    let innerFn input =
        // run parser1 with the input
        let result1 = run parser1 input

        // test the result for Failure/Success
        match result1 with
        | Success result -> 
            // if success, return the original result
            result1

        | Failure err -> 
            // if failed, run parser2 with the input
            let result2 = run parser2 input

            // return parser2's result
            result2 

    // return the inner function
    Parser innerFn 

let ( <|> ) = orElse

/// Choose any of a list of parsers
let choice listOfParsers = 
    List.reduce ( <|> ) listOfParsers 

let anyOf listOfChars = 
    listOfChars
    |> List.map pchar // convert into parsers
    |> choice

let mapP f parser =
    let innerFn input =
        // run parser with the input
        let result = run parser input

        // test the result for Failure/Success
        match result with
        | Success (value,remaining) ->
            // if success, return the value transformed by f
            let newValue = f value
            Success (newValue, remaining)

        | Failure err ->
            // if failed, return the error
            Failure err
    // return the inner function
    Parser innerFn

let ( <!> ) = mapP

let ( |>> ) x f = mapP f x

let returnP x =
    let innerFn input =
        // ignore the input and return x
        Success (x,input )
    // return the inner function
    Parser innerFn

let applyP fP xP =
    // create a Parser containing a pair (f,x)
    (fP .>>. xP)
    // map the pair by applying f to x
    |> mapP (fun (f,x) -> f x)

let ( <*> ) = applyP

// lift a two parameter function to Parser World
let lift2 f xP yP =
    returnP f <*> xP <*> yP

let addP =
    lift2 (+)

let startsWith (str:string) (prefix:string) =
    str.StartsWith(prefix)

let startsWithP =
    lift2 startsWith

let rec sequence parserList =
    // define the "cons" function, which is a two parameter function
    let cons head tail = head::tail

    // lift it to Parser World
    let consP = lift2 cons

    // process the list of parsers recursively
    match parserList with
    | [] ->
        returnP []
    | head::tail ->
        consP head (sequence tail)

let parsers = [ pchar 'A'; pchar 'B'; pchar 'C' ]
let combined = sequence parsers
        
let result = run combined "ABCD"

/// Helper to create a string from a list of chars
let charListToStr charList =
     String(List.toArray charList)

// match a specific string
let pstring str =
    str
    // convert to list of char
    |> List.ofSeq
    // map each char to a pchar
    |> List.map pchar
    // convert to Parser<char list>
    |> sequence
    // convert Parser<char list> to Parser<string>
    |> mapP charListToStr

let rec parseZeroOrMore parser input =
    // run parser with the input
    let firstResult = run parser input
    // test the result for Failure/Success
    match firstResult with
    | Failure err ->
        // if parse fails, return empty list
        ([],input)
    | Success (firstValue,inputAfterFirstParse) ->
        // if parse succeeds, call recursively
        // to get the subsequent values
        let (subsequentValues,remainingInput) =
            parseZeroOrMore parser inputAfterFirstParse
        let values = firstValue::subsequentValues
        (values,remainingInput)

let many parser =

    let rec innerFn input =
        // parse the input -- wrap in Success as it always succeeds
        Success (parseZeroOrMore parser input)

    Parser innerFn