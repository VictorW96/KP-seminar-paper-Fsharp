// Learn more about F# at http://fsharp.org

open System
open BasicFunctions

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let result1 :int = sampleFunction1 2000
    printfn "The result of squaring the integer 2000 and adding 3 is %d" result1
    0
    