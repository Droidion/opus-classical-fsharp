module OpusClassicalWeb.Models

type Period = {
    Id: int
    Name: string
    YearStart: int
    YearEnd: int option
    Slug: string
}