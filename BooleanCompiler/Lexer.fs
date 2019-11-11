module Lexer

let tokens = ['&','|','!']
 
let isInList elementToFind listToCheck = 
    List.fold(fun acc x -> acc || x = elementToFind) false listToCheck

let IsBlank c =
    if  c = " " then
        true
    else
        false

let rec lexer input =
    let mutable output = []
    let token = []
    let position = 0

    for current_token in input do 
    
        if IsBlank(current_token) = false then
            
            if isInList token tokens then 
                output = List.append output token
                output = List.append output current_token
            else
                List.append token current_token
    output

