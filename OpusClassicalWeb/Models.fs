module OpusClassicalWeb.Models

type Period =
    { Id: int
      Name: string
      YearStart: int
      YearEnd: int option
      Slug: string }

type Composer =
    { Id: int
      FirstName: string
      LastName: string
      YearBorn: int
      YearDied: int option
      PeriodId: int
      Slug: string
      WikipediaLink: string option
      ImslpLink: string option
      Enabled: bool
      Countries: string }

type PeriodWithComposers =
    { Period: Period
      Composers: Composer list }
