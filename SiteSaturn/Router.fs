module Site.Router

open Saturn
open SiteSaturn.Controllers
open Giraffe

let addHeaders =
    pipeline {
        set_header "Strict-Transport-Security" "max-age=31536000; includeSubDomains"
        plug (publicResponseCaching 600 None)
    }

let main =
    router {
        pipe_through addHeaders
        forward "" index
        forward "/about" about
        forward "/composer" composer
    }
