/// Saturn router
module Site.Router

open Saturn
open SiteSaturn.Controllers
open SiteSaturn.Templates.Pages
open Giraffe

/// Adds headers to the responses
let private addHeaders =
    pipeline {
        set_header "Strict-Transport-Security" "max-age=31536000; includeSubDomains"
        plug putSecureBrowserHeaders
        // plug (publicResponseCaching 600 None)
    }

/// API router
let private apiRouter =
    router {
        not_found_handler (setStatusCode 404 >=> text "Endpoint not found")
        forward "/search" searchApiController
    }

/// Web pages router
let private pagesRouter =
    router {
        not_found_handler (htmlView NotFound.view)
        forward "" indexPageController
        forward "/about" aboutPageController
        forward "/composer" composerPageController
    }

/// Top level router
let topRouter =
    router {
        pipe_through addHeaders
        not_found_handler (htmlView NotFound.view)
        forward "/api" apiRouter
        forward "" pagesRouter
    }
