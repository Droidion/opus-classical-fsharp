/// Falco router.
module Site.Router

open Falco
open Falco.Routing
open Site.Controllers

/// Falco routes.
let endpoints : HttpEndpoint list =
    [
        get "/" periodsController
        get "/about" aboutController
        get "/composer/{slug}" composerController
        get "/composer/{slug}/work/{workId}" workController
        get "/api/search" searchController
    ]