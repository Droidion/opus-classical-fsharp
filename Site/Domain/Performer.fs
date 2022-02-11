/// Business logic for performer.
module Site.Domain.Performer

/// Performer of some musical work.
type Performer = {
    firstName: string option
    lastName: string
    priority: int option
    instrument: string option
}