module AST


type Node = 
    | Or of Node * Node 
    | And of Node * Node 
    | Not of Node 
    | Var of string


let rec eval(vars:Map<string,bool>) (ast:Node) : bool =
    match ast with
        | Or(lhs,rhs) -> eval vars lhs || eval vars rhs
        | And(lhs, rhs) -> eval vars lhs && eval vars rhs
        | Not(N) -> not(eval vars N)  
        | Var(s) -> Map.find s vars 

// Testing --------------------------------------------------
let exampleAST = And(Var("A"),Or(Var("B"),Var("C")))
let vars = Map.ofList [ ("A", false); ("B", false); ("C", true) ]
let result = eval vars exampleAST