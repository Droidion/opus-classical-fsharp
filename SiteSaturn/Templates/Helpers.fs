/// Helpers for HTML templates
module SiteSaturn.Templates.Helpers

open System
let coversUrl = Environment.GetEnvironmentVariable("StaticAssetsUrl")

/// Checks if given string is a 4 digits number, like "1234" (not "-123", "123", or "12345")
let private validDigits (str: string) : bool = str.Length = 4 && str.[0] <> '-'

/// Checks if two given string have the same first two letters, like "1320" and "1399"
let private centuryEqual (year1: int) (year2: int) : bool =
    let str1 = string year1
    let str2 = string year2
    str1.[..1] = str2.[..1]

/// Formats the range of two years into the string, e.g. "1720–95", or "1720–1805", or "1720–"
/// Start year and dash are always present
/// It's supposed to be used for lifespans, meaning we always have birth, but may not have death
let formatYearsRangeStrict (startYear: int) (finishYear: int option) : string =
    match (startYear, finishYear) with
    | start, _ when start |> string |> validDigits |> not -> ""
    | start, None -> $"{start}–"
    | start, Some finish when finish |> string |> validDigits |> not -> $"{start}–"
    | start, Some finish when centuryEqual start finish -> $"{start}–{(string finish).[2..3]}"
    | start, Some finish -> $"{start}–{finish}"
    | _, _ -> ""

/// Formats the range of two years into a string, e.g. "1720–95", or "1720–1805", or "1720"
/// Both years can be present or absent, so it's a more generic, loose form
let formatYearsRangeLoose (startYear: int option) (finishYear: int option) : string =
    match (startYear, finishYear) with
    | Some start, None when string start |> validDigits -> string start
    | None, Some finish -> string finish
    | Some start, Some finish when string start |> validDigits && string finish |> validDigits |> not -> string start
    | Some start, Some finish when string start |> validDigits |> not && string finish |> validDigits -> string finish
    | Some start, Some finish when centuryEqual start finish -> $"{start}–{(string finish).[2..3]}"
    | Some start, Some finish -> $"{start}–{finish}"
    | _, _ -> ""
    
/// Formats minutes into a string with hours and minutes, like "2h 35m"
let formatWorkLength (lengthInMinutes: int option) : string =
    let length =
        match lengthInMinutes with
        | Some len -> len
        | None -> 0

    let hours = length / 60
    let minutes = length % 60

    match (hours, minutes) with
    | 0, 0 -> ""
    | h, m when h < 0 || m < 0 -> ""
    | 0, m -> $"{m}m"
    | h, 0 -> $"{h}h"
    | h, m -> $"{h}h {m}m"