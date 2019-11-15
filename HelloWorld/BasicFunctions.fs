module BasicFunctions

open System

let rec faculty n = 
    if n < 1 then
        1
    else
        n * faculty(n-1)

Console.WriteLine(faculty 3)