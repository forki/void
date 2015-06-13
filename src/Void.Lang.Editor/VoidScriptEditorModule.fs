﻿namespace Void.Lang.Editor

open Void.Core
open Void.Lang.Parser
open Void.Lang.Interpreter

type VoidScriptEditorModule(publish : Command -> unit) =
    let echo _ execEnv =
        publish <| Command.Echo ""

    let edit raw execEnv =
        match raw with
        | "%" -> FileIdentifier.CurrentBuffer
        | "#" -> FileIdentifier.AlternateBuffer
        // TODO better parsing, include #2, etc
        | _ -> FileIdentifier.Path raw
        |> Command.Edit 
        |> publish

    let help _ execEnv =
        publish Command.Help

    let messages _ execEnv =
        publish Command.ShowNotificationHistory

    let quit _ execEnv =
        publish Command.Quit

    let quitAll _ execEnv =
        publish Command.QuitAll

    let redraw _ execEnv =
        publish Command.Redraw

    let view raw execEnv =
        match raw with
        | "%" -> FileIdentifier.CurrentBuffer
        | "#" -> FileIdentifier.AlternateBuffer
        // TODO better parsing, include #2, etc
        | _ -> FileIdentifier.Path raw
        |> Command.View 
        |> publish

    member x.Commands = [
        {
            ShortName = "ec"
            FullName = "echo"
            // TODO of course echo really should take expressions
            WrapArguments = CommandType.NoArgs echo
        }
        {
            ShortName = "e"
            FullName = "edit"
            // TODO Note that in Vim it can take optional ++opt and +cmd args
            WrapArguments = CommandType.Unparsed edit
        }
        {
            ShortName = "h"
            FullName = "help"
            // TODO of course echo really should be able to take arguments
            WrapArguments = CommandType.NoArgs help
        }
        {
            ShortName = "mes"
            FullName = "messages"
            WrapArguments = CommandType.NoArgs messages
        }
        {
            ShortName = "q"
            FullName = "quit"
            WrapArguments = CommandType.NoArgs quit
        }
        {
            ShortName = "qa"
            FullName = "qall"
            WrapArguments = CommandType.NoArgs quitAll
        }
        {
            ShortName = "redr"
            FullName = "redraw"
            WrapArguments = CommandType.NoArgs redraw
        }
        {
            ShortName = "vie"
            FullName = "view"
            // TODO Note that in Vim it can take optional ++opt and +cmd args
            WrapArguments = CommandType.Unparsed view
        }
    ]
