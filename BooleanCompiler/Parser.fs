module Parser

open AST
open FParsec
open System

type Factor = 
    | Variable of Variable
    | Factor of Factor
    | Expression of Expression

and Term = Factor * Factor option
and Expression = Term * Term option
and Variable = string

let Or = "&"
let And = "|"
let Not = "!"
