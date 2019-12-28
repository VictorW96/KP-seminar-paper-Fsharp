module BooleanCompiler.Tests

open NUnit.Framework
open BooleanParser
open FParsec
open AST

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TestBooleanCompiler () =
   let expected = Or(Not(Var("A")),And(Var("B"),Var("C")))
   match run parseExpression "(!A)|(B&C)" with
        | Success(result, _, _)   -> Assert.That(result, Is.EqualTo(expected))
        | Failure(errorMsg, _, _) -> Assert.Fail(errorMsg)

[<Test>]
let TestEval () =
   let expected = false
   let varMap = Map.empty
                    .Add("A",true)
                    .Add("B",false)
                    .Add("C",false)
   match run parseExpression "(!A)|(B&C)" with
         | Success(result, _, _)   -> Assert.That(eval varMap result, Is.EqualTo(expected))
         | Failure(errorMsg, _, _) -> Assert.Fail(errorMsg)

   
