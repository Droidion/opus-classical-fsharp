/// Business logic for Streamer.
module Site.Domain.Streamer

/// Streaming service, e.g. Spotify, Tidal, or Apple Music.
type Streamer = {
    name: string
    icon: string // Logo filename, e.g. foo.webp
    link: string
    prefix: string // First part of the links URLs, e.g. spotify://
}