﻿namespace Void.Core

[<RequireQualifiedAccess>]
type KeyPress =
    | One
    | Two
    | Three
    | Four
    | Five
    | Six
    | Seven
    | Eight
    | Nine
    | Zero
    | A
    | B
    | C
    | D
    | E
    | F
    | G
    | H
    | I
    | J
    | K
    | L
    | M
    | N
    | O
    | P
    | Q
    | R
    | S
    | T
    | U
    | V
    | W
    | X
    | Y
    | Z
    | ShiftA
    | ShiftB
    | ShiftC
    | ShiftD
    | ShiftE
    | ShiftF
    | ShiftG
    | ShiftH
    | ShiftI
    | ShiftJ
    | ShiftK
    | ShiftL
    | ShiftM
    | ShiftN
    | ShiftO
    | ShiftP
    | ShiftQ
    | ShiftR
    | ShiftS
    | ShiftT
    | ShiftU
    | ShiftV
    | ShiftW
    | ShiftX
    | ShiftY
    | ShiftZ
    | ControlA
    | ControlB
    | ControlC
    | ControlD
    | ControlE
    | ControlF
    | ControlG
    | ControlH
    | ControlI
    | ControlJ
    | ControlK
    | ControlL
    | ControlM
    | ControlN
    | ControlO
    | ControlP
    | ControlQ
    | ControlR
    | ControlS
    | ControlT
    | ControlU
    | ControlV
    | ControlW
    | ControlX
    | ControlY
    | ControlZ
    | AltA
    | AltB
    | AltC
    | AltD
    | AltE
    | AltF
    | AltG
    | AltH
    | AltI
    | AltJ
    | AltK
    | AltL
    | AltM
    | AltN
    | AltO
    | AltP
    | AltQ
    | AltR
    | AltS
    | AltT
    | AltU
    | AltV
    | AltW
    | AltX
    | AltY
    | AltZ
    | Backtick
    | Tilde
    | Bang
    | AtSymbol
    | Hash
    | DollarSign
    | PercentSign
    | Caret
    | Ampersand
    | Asterisk
    | OpenParenthesis
    | CloseParenthesis
    | Dash
    | Underscore
    | EqualSign
    | OpenSquareBracket
    | CloseSquareBracket
    | OpenCurlyBracket
    | CloseCurlyBracket
    | Backslash
    | Pipe
    | Tab
    | Semicolon
    | Colon
    | ControlSemicolon
    | SingleQuote
    | DoubleQuote
    | Comma
    | Period
    | OpenAngleBracket
    | CloseAngleBracket
    | Slash
    | QuestionMark
    | ShiftTab
    | ControlTab
    | AltTab
    | F1
    | F2
    | F3
    | F4
    | F5
    | F6
    | F7
    | F8
    | F9
    | F10
    | F11
    | F12
    | Escape
    | Insert
    | Delete
    | Backspace
    | Home
    | End
    | PageUp
    | PageDown
    | Enter

[<RequireQualifiedAccess>]
type HotKey =
    | ControlA
    | ControlB
    | ControlC
    | ControlD
    | ControlE
    | ControlF
    | ControlG
    | ControlH
    | ControlI
    | ControlJ
    | ControlK
    | ControlL
    | ControlM
    | ControlN
    | ControlO
    | ControlP
    | ControlQ
    | ControlR
    | ControlS
    | ControlT
    | ControlU
    | ControlV
    | ControlW
    | ControlX
    | ControlY
    | ControlZ
    | AltA
    | AltB
    | AltC
    | AltD
    | AltE
    | AltF
    | AltG
    | AltH
    | AltI
    | AltJ
    | AltK
    | AltL
    | AltM
    | AltN
    | AltO
    | AltP
    | AltQ
    | AltR
    | AltS
    | AltT
    | AltU
    | AltV
    | AltW
    | AltX
    | AltY
    | AltZ
    | Tab
    | ControlSemicolon
    | ShiftTab
    | ControlTab
    | AltTab
    | F1
    | F2
    | F3
    | F4
    | F5
    | F6
    | F7
    | F8
    | F9
    | F10
    | F11
    | F12
    | Escape
    | Insert
    | Delete
    | Backspace
    | Home
    | End
    | ArrowUp
    | ArrowDown
    | PageUp
    | PageDown
    | Enter

[<RequireQualifiedAccess>]
type TextOrHotKey =
    | Text of string
    | HotKey of HotKey
