/// Saturn router
module Site.Router

open Saturn
open Site.Templates.Pages
open Giraffe
open Site.Controller

/// Adds headers to the responses
let private addHeaders =
    pipeline {
        set_header "Strict-Transport-Security" "max-age=31536000; includeSubDomains; preload"

        set_header
            "Content-Security-Policy"
            "default-src 'none'; manifest-src 'self'; connect-src 'self' https://logs.opusclassical.net; script-src 'self' https://logs.opusclassical.net; style-src 'self'; img-src 'self' https://static.zunh.dev"

        set_header "Referrer-Policy" "no-referrer"
        set_header "Permissions-Policy" "geolocation=(), microphone=()"
        plug putSecureBrowserHeaders
    // plug (publicResponseCaching 600 None)
    }

/// API router
let private apiRouter =
    router {
        not_found_handler (setStatusCode 404 >=> text "Endpoint not found")
        forward "/search" searchController
    }

/// Web pages router
let private pagesRouter =
    router {
        not_found_handler (htmlView NotFound.view)
        forward "" periodsController
        forward "/about" aboutController
        forward "/composer" composerController
    }

/// Top level router
let topRouter =
    router {
        pipe_through addHeaders
        not_found_handler (htmlView NotFound.view)
        forward "/api" apiRouter
        forward "" pagesRouter
    }
