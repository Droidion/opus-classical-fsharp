module OpusClassicalWeb.Tests.HelpersTests

open NUnit.Framework
open FsUnit
open OpusClassicalWeb.Helpers

// Test data
let validYears = [ 1985; 2024; 1000; 9999 ]
let invalidYears = [ -123; 12345; 0 ]

[<TestFixture>]
module ``Helpers tests`` =

    [<Test>]
    let ``isValidYear returns true for valid years`` () =
        for year in validYears do
            isValidYear (Some year) |> should be True

    [<Test>]
    let ``isValidYear returns false for invalid years`` () =
        isValidYear None |> should be False

        for year in invalidYears do
            isValidYear (Some year) |> should be False

    [<Test>]
    let ``sliceYear returns last two digits`` () =
        sliceYear (Some 1985) |> should equal "85"
        sliceYear (Some 2024) |> should equal "24"
        sliceYear None |> should equal ""

    [<Test>]
    let ``centuryEqual correctly compares centuries`` () =
        // Same century cases
        let sameCenturyCases = [ (1801, 1899); (1900, 1950); (2001, 2099) ]

        for (year1, year2) in sameCenturyCases do
            centuryEqual (Some year1) (Some year2) |> should be True

        // Different century cases
        let differentCenturyCases = [ (1899, 1901); (1999, 2000) ]

        for (year1, year2) in differentCenturyCases do
            centuryEqual (Some year1) (Some year2) |> should be False

        // Invalid cases
        centuryEqual None None |> should be False
        centuryEqual (Some 1900) None |> should be False

    [<Test>]
    let ``formatYearsRangeString formats ranges correctly`` () =
        // Test cases structure: startYear, finishYear, expected result
        let testCases =
            [
              // Same century
              (Some 1720, Some 1795, "1720–95")
              (Some 1900, Some 1901, "1900–01")

              // Different centuries
              (Some 1795, Some 1803, "1795–1803")
              (Some 1699, Some 1701, "1699–1701")

              // Only start year
              (Some 1720, None, "1720–")

              // Only finish year
              (None, Some 1805, "1805")

              // No years
              (None, None, "") ]

        for (start, finish, expected) in testCases do
            formatYearsRangeString start finish |> should equal expected

    [<Test>]
    let ``formatWorkLength formats duration correctly`` () =
        let testCases =
            [ (65, "1h 5m") // Hours and minutes
              (60, "1h") // Only hours
              (45, "45m") // Only minutes
              (120, "2h") // Multiple hours, no minutes
              (150, "2h 30m") // Multiple hours and minutes
              (0, "") // Zero
              (-10, "") ] // Negative

        for (minutes, expected) in testCases do
            formatWorkLength minutes |> should equal expected

    [<Test>]
    let ``formatCatalogueName formats catalogue correctly`` () =
        let testCases =
            [ (Some "BWV", Some 12, Some "p", "BWV 12p")
              (Some "BWV", Some 12, None, "BWV 12")
              (Some "Op.", Some 27, Some "b", "Op. 27b")
              (None, Some 12, None, "")
              (Some "BWV", None, None, "")
              (None, None, None, "") ]

        for (name, number, postfix, expected) in testCases do
            formatCatalogueName name number postfix |> should equal expected

    [<Test>]
    let ``formatWorkName formats work name correctly`` () =
        let testCases =
            [ ("Symphony", Some 9, Some "Great", "Symphony No. 9 Great")
              ("Symphony", Some 9, None, "Symphony No. 9")
              ("Symphony", None, Some "Great", "Symphony Great")
              ("Symphony", None, None, "Symphony")
              ("", None, None, "")
              ("", Some 1, Some "Test", "") ]

        for (title, number, nickname, expected) in testCases do
            formatWorkName title number nickname |> should equal expected
