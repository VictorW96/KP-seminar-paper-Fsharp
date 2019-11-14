module Parser


let expectChar expectedChar inputChars =
    match inputChars with
    | c :: remainingChars -> 
        if c = expectedChar then (c, remainingChars)
            else (sprintf "Expected '%c'. got '%c' ", expectedChar c)