﻿namespace Void.Model

type DataOperations = 
    | Append
    | Insert
    | Change
    | Delete
    | Left
    | Right
    | Up
    | Down
    | Escape // TODO Is there a better word for this?

type Modes =
    | Insert
    | Normal
    | Command

// TODO This is naive, obviously
type FileBuffer = {
    Filepath : string option
    Contents: string
}

type Coordinate = {
    Row : uint32
    Column : uint32
}

type ScreenBuffer = {
    Buffer : FileBuffer
    ScreenPosition : Coordinate
    BufferPosition : Coordinate
}

type EditorState = {
    CurrentBuffer : ScreenBuffer
    BufferList : FileBuffer list
}

module Editor = 
    let private startOfFile() =
        { Row = 0u; Column = 0u }

    let private emptyFileBuffer() =
        { Filepath = None; Contents = "" }

    let private emptyScreenBuffer() =
        {
            Buffer = emptyFileBuffer()
            ScreenPosition = startOfFile()
            BufferPosition = startOfFile()
        }

    let init (commands : CommandLine.Arguments) =
        {
            CurrentBuffer = emptyScreenBuffer()
            BufferList = []
        }

    let bootstrapped = true