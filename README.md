## Usage of the BooleanCompiler



# Comparison of functional F# to GO

## Table of Contents
1. [ Introduction](#intro)
2. [ F# Overview ](#FOver)
	1. [Functional programming basics](#fprog)
	2. [F# concepts](#fconc)
3. [ Go Overview ](#GoOver)
3. [ Parser Example ](#ParsEx)
5. [ Comparison ](#Comp)
6. [ Conclusion ](#Conc)

<a name="intro"></a>
## 1. Introduction


<a name="FOver"></a>
## 2. F# Overview

F# is a multi paradigm programming language. That means it takes multiple parts of different code design and puts it together.
Primarily it was designed to be a functional programming language but you can also do pbject oriented programming or imperative programming 
but the last one should only be rarely used and its considered to be bad design. F# was invented by a Microsoft Research Team and is compatible with
the whole .NET Plattform. So you can interact for example with Visual Basic or C#, which makes F# very powerfull for actual application writing. 
This was the main idea, to build a functional programming language, that is integrated into the Microsoft Ecosystem.
Its language features are very similar to [OCaml](https://de.wikipedia.org/wiki/Objective_CAML) and it was ineed a role model for F#. 

<a name="fprog"></a>
### 1. Functional programming basics

The basic concept of functional programing is that the most important structural unit is a function in contrast for example to object oriented programming, where the 
most important structural unit is a class. The abstraction happens through passing basic functions to higher level functions as arguments. 
One goal of functional programming is also to minimze the use of mutable state. So in a functional programming language you only work with immutable data structures.

<a name="fconc"></a>
### 2. F# concepts

In this section we will focus on the propertys of F#'s language features.

#### Let Bindings and immutability

```fhsarp
let a = 10
a = 20
```

This code isn't valid F# code for two reasons. First if you bind a value to an identifier with the `let` binding you cannot 
alter it anymore. It is immutable to assert one principle of functional programming. Second the `=` is the equality sign and not the assignment operator. This would be `<-`. You can do mutable data structures with `let mutable` but with this extra keyword its obviously discouraged.

#### Type interference

```fhsarp
let a = 10
a = 20
```

Like we have seen above, we do not have to give a type definition for the variable `a`. F# derives it because the value `10` is of type `int` so `a` must be an `int` too. F# also can do this in more complex constructs like functions, consider this function definition.

```fhsarp
let prefix prefixStr baseStr =
    prefixStr + "," + baseStr
```

F# interferes that the arguments `prefixStr` and `baseStr` have to be of type `string` and that the function returns a `string` because of the method body and the operation in which they are used. You can always add the type definition, if you want to. In this way F# combines the security of a strongly typed language and the read- and writability of a loosely typed language, which is similar to Go.

<a name="GoOver"></a>
## 3. Go Overview

<a name="ParsEx"></a>
## 4. Parser Example

<a name="Comp"></a>
## 5. Comparison

<a name="Conc"></a>
## 6. Conclusion
