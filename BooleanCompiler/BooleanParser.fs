module BooleanParser

open FParsec

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

let result = run pfloat "02"