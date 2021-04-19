module Site.Router

open Saturn
open SiteSaturn.Controllers

let main =
    router {
        forward "" index
        forward "/about" about
        forward "/composer" composer
    }
