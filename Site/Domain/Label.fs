module Site.Domain.Label

/// Music Label, like Sony or EMI
type Label = {
    id: int
    name: string
}

/// Select all labels
let labels = "select id, name from labels"