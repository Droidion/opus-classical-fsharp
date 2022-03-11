module Site.Tests

open Shouldly
open Xunit
open Site.Templates.Helpers

[<Fact>]
let formatYearsRangeStrict_FormatsAsExpected () =
    (formatYearsRangeStrict 1920 None)
        .ShouldBe "1920–"

    (formatYearsRangeStrict 1930 (Some 1950))
        .ShouldBe "1930–50"

    (formatYearsRangeStrict 1930 (Some 2001))
        .ShouldBe "1930–2001"

[<Fact>]
let formatYearsRangeStrict_HandlesProblemsGracefully () =
    (formatYearsRangeStrict 20 (Some 1234))
        .ShouldBe ""

    (formatYearsRangeStrict 12345 None).ShouldBe ""
    (formatYearsRangeStrict -123 None).ShouldBe ""

    (formatYearsRangeStrict 1930 (Some 12345))
        .ShouldBe "1930–"

    (formatYearsRangeStrict 1930 (Some 12))
        .ShouldBe "1930–"

    (formatYearsRangeStrict 1234 (Some -123))
        .ShouldBe "1234–"

[<Fact>]
let formatYearsRangeLoose_FormatsAsExpected () =
    (formatYearsRangeLoose (Some 1920) None)
        .ShouldBe "1920"

    (formatYearsRangeLoose (Some 1930) (Some 1950))
        .ShouldBe "1930–50"

    (formatYearsRangeLoose (Some 1930) (Some 2001))
        .ShouldBe "1930–2001"

    (formatYearsRangeLoose None (Some 2001))
        .ShouldBe "2001"

[<Fact>]
let formatYearsRangeLoose_HandlesProblemsGracefully () =
    (formatYearsRangeLoose (Some 20) (Some 1234))
        .ShouldBe "1234"

    (formatYearsRangeLoose (Some 12345) None)
        .ShouldBe ""

    (formatYearsRangeLoose (Some 123) None)
        .ShouldBe ""

    (formatYearsRangeLoose (Some 1930) (Some 12345))
        .ShouldBe "1930"

    (formatYearsRangeLoose (Some 1930) (Some 12))
        .ShouldBe "1930"

    (formatYearsRangeLoose (Some 1234) (Some -123))
        .ShouldBe "1234"

[<Fact>]
let formatCatalogueName_FormatsAsExpected () =
    (formatCatalogueName (Some "Foo", Some 10, Some "bar")).ShouldBe "Foo 10bar"
    (formatCatalogueName (Some "Foo", Some 10, None)).ShouldBe "Foo 10"
    (formatCatalogueName (None, None, None)).ShouldBe ""

[<Fact>]
let formatWorkLength_FormatsAsExpected () =
    (formatWorkLength (Some 1)).ShouldBe "1m"
    (formatWorkLength (Some 30)).ShouldBe "30m"
    (formatWorkLength (Some 60)).ShouldBe "1h"
    (formatWorkLength (Some 95)).ShouldBe "1h 35m"
    (formatWorkLength (Some 240)).ShouldBe "4h"
    (formatWorkLength (Some 245)).ShouldBe "4h 5m"

[<Fact>]
let formatWorkLength_HandlesProblemsGracefully () =
    (formatWorkLength (Some 0)).ShouldBe ""
    (formatWorkLength (Some -1)).ShouldBe ""
    (formatWorkLength (Some -60)).ShouldBe ""
    (formatWorkLength None).ShouldBe ""
