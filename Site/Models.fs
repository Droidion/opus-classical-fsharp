module Site.Models

/// Composer, like Bach or Beethoven
type Composer =
    { id: int
      firstName: string
      lastName: string
      yearBorn: int
      yearDied: int option
      countries: string list // Countries composer is associated with.
      slug: string // Unique composer readable text id, to be used in URLs.
      wikipediaLink: string option
      imslpLink: string option
      enabled: bool } // Show this composer on the main page

type ComposerSearchResult =
    { id: int
      firstName: string
      lastName: string
      slug: string
      rating: float }

/// Musical work, like Symphony No. 9 by Beethoven
type Work = {
    id: int
    title: string
    yearStart: int option // Year when composer started the work, if known. Can be used without YearFinish if the work was finished in a single year.
    yearFinish: int option // Year when composer finished the work, if known. Can be used without YearStart if the work was finished in a single year.
    averageMinutes: int option // Approximate length of the work in minutes.
    catalogueName: string option // Name of the catalogue of composer's works, like "BWV" for Bach or "Op." for Beethoven.
    catalogueNumber: int option // Catalogue number of the work, like 123 for Op. 123
    cataloguePostfix: string option // Postfix for the number of the work in the catalogue, like b in Op. 123b
    key: string option // e.g. C# minor
    no: int option // Work number in some sequence, like 9 in Symphony No. 9
    nickname: string option // e.g. Great in Beethoven's Symphony No. 9 Great
}

/// Genre of the work, like Symphony, or String Quartet, or Choral music.
type Genre = {
    name: string
    icon: string // e.g. 🐕
    works: Work list // List of the works belonging to the genre.
}

/// Music Label, like Sony or EMI
type Label = {
    id: int
    name: string
}

/// Performer of some musical work
type Performer = {
    firstName: string option
    lastName: string
    priority: int option
    instrument: string option
}
    
/// Period when composer lived and worked, e.g. Late Baroque or Romanticism. 
type Period = {
    id: int
    name: string
    yearStart: int
    yearEnd: int option
    slug: string // Unique period readable text id, to be used in URLs.
    composers: Composer list
}

/// Streaming service, e.g. Spotify, Tidal, or Apple Music.
type Streamer = {
    name: string
    icon: string // Logo filename, e.g. foo.webp
    link: string
    prefix: string // First part of the links URLs, e.g. spotify://
}

/// Recording of a musical work.
type Recording = {
    id: int
    coverName: string
    yearStart: int option
    yearFinish: int option
    performers: Performer list
    label: string option
    length: int
    streamers: Streamer list
}
