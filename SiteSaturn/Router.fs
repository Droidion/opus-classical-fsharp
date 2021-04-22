module Site.Router

open Saturn
open SiteSaturn.Controllers
open SiteSaturn.Templates.Pages
open Giraffe

let addHeaders =
    pipeline {
        set_header "Strict-Transport-Security" "max-age=31536000; includeSubDomains"
        plug putSecureBrowserHeaders
        // plug (publicResponseCaching 600 None)
    }

let main =
    router {
        pipe_through addHeaders
        not_found_handler (htmlView NotFound.view)
        
        forward "" index
        forward "/about" about
        forward "/composer" composer
    }
