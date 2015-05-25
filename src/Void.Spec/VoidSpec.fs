﻿namespace Void.Spec

open Void
open Void.Core
open Void.ViewModel
open NUnit.Framework
open FsUnit

type MainViewStub() =
    member val Bus = Bus([]) with get, set
    interface MainView with
        member x.SetBackgroundColor color =
            ()
        member x.SetFontBySize size =
            ()
        member x.SetViewSize dimensions =
            ()
        member x.SetViewTitle title =
            ()

type InputModeChangerStub() =
    let mutable _inputHandler = InputMode<unit>.KeyPresses (fun _ -> ())
    member x.getInputHandler() =
        _inputHandler
    interface InputModeChanger with
        member x.SetInputHandler inputMode =
            _inputHandler <- inputMode

[<TestFixture>]
type ``Void``() = 
    [<Test>]
    member x.``When I have freshly opened Vim with one window, when I enter the quit command, then the editor exists``() =
        let mainView = MainViewStub()
        let inputModeChanger = InputModeChangerStub()
        let messagingSystem = Init.buildVoid mainView inputModeChanger
        Init.launchVoid messagingSystem
        let closed = ref false
        let closeHandler event =
            match event with
            | Event.LastWindowClosed ->
                closed := true
            | _ -> ()
            noMessage
        messagingSystem.EventChannel.addHandler closeHandler

        match inputModeChanger.getInputHandler() with
        | InputMode.KeyPresses handler ->
            handler KeyPress.Semicolon
        | InputMode.TextAndHotKeys _ ->
            Assert.Fail "Right after startup did not have a key presses handler set"

        match inputModeChanger.getInputHandler() with
        | InputMode.KeyPresses _ ->
            Assert.Fail "After typing ; did not have text and hotkeys handler set"
        | InputMode.TextAndHotKeys handler ->
            TextOrHotKey.Text "q" |> handler
            TextOrHotKey.HotKey HotKey.Enter |> handler

        (* Remeber that ! is dereference, not negate, in F# *)
        !closed |> should be True
