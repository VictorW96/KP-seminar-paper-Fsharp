module BasicFunctions

let prefix prefixStr baseStr =
    prefixStr + "," + baseStr

let exclaim s =
    s + "!"

let names = ["Victor"; "Petra"; "Test"]

let result = names
             |> List.map (prefix "Hello")
             |> List.map exclaim
