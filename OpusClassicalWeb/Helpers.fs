module OpusClassicalWeb.Helpers

let MAX_VALID_YEAR = 9999

/// IsValidYear checks if given number is a 4 digits number, like 1234 (not -123, 123, or 12345).
let isValidYear (year: int option) : bool =
    match year with
    | Some n -> n > 1 && n <= MAX_VALID_YEAR
    | None -> false

/// SliceYear returns slice of the full year, like 85 from 1985.
let sliceYear (year: int option) : string =
    match year with
    | Some n -> string n |> fun s -> s.[2..3]
    | None -> ""

let private getCentury (year: int option) : string =
    match year with
    | Some n -> string n |> fun s -> s.[0..1]
    | None -> ""

/// CenturyEqual checks if two given years are of the same century, like 1320 and 1399.
let centuryEqual (year1: int option) (year2: int option) : bool =
    if not (isValidYear year1) || not (isValidYear year2) then
        false
    else
        getCentury year1 = getCentury year2

/// FormatYearsRangeString formats the range of two years into the string, e.g. "1720–95", or "1720–1805", or "1720–".
/// Start year and dash are always present.
/// It's supposed to be used for lifespans, meaning we always have birth, but may not have death.
let formatYearsRangeString (startYear: int option) (finishYear: int option) : string =
    match startYear, finishYear with
    | None, None -> ""
    | Some start, None -> sprintf "%d–" start
    | None, Some finish -> string finish
    | Some start, Some finish ->
        if centuryEqual (Some start) (Some finish) then
            sprintf "%d–%s" start (sliceYear (Some finish))
        else
            sprintf "%d–%d" start finish

/// FormatWorkLength formats minutes into a string with hours and minutes, like "2h 35m"
let formatWorkLength (lengthInMinutes: int) : string =
    let hours = lengthInMinutes / 60
    let minutes = lengthInMinutes % 60

    match hours, minutes with
    | h, m when h <= 0 && m <= 0 -> ""
    | h, m when h < 0 || m < 0 -> ""
    | 0, m -> sprintf "%dm" m
    | h, 0 -> sprintf "%dh" h
    | h, m -> sprintf "%dh %dm" h m

/// FormatCatalogueName formats catalogue name of the musical work, like "BWV 12p".
let formatCatalogueName
    (catalogueName: string option)
    (catalogueNumber: int option)
    (cataloguePostfix: string option)
    : string =
    match catalogueName, catalogueNumber with
    | Some name, Some number ->
        let postfix = defaultArg cataloguePostfix ""
        sprintf "%s %d%s" name number postfix
    | _ -> ""

/// FormatWorkName formats music work full name, like "Symphony No. 9 Great".
let formatWorkName (workTitle: string) (workNo: int option) (workNickname: string option) : string =
    if workTitle = "" then
        ""
    else
        let withNumber =
            match workNo with
            | Some n -> sprintf "%s No. %d" workTitle n
            | None -> workTitle

        match workNickname with
        | Some nickname -> sprintf "%s %s" withNumber nickname
        | None -> withNumber
