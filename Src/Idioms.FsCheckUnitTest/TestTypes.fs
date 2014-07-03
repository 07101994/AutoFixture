﻿namespace Ploeh.AutoFixture.Idioms.FsCheckUnitTest

type AClass () = 
    member this.WriteOnlyProperty with set (value) = value |> ignore
    member this.NullReturnValueProperty with get () = null
    member this.ReadOnlyProperty  with get () = obj()
    member this.MethodWithReturnValue () = obj()
    member this.VoidMethod () = ()

[<AbstractClass; Sealed>]
type AStaticClass () =
    static member WriteOnlyProperty with set (value) = value |> ignore
    static member NullReturnValueProperty  with get () = null
    static member ReadOnlyProperty with get () = obj()
    static member MethodWithReturnValue () = obj()
    static member VoidMethod () = ()