<script lang="ts">
    type SearchResult = { 
        id: number
        firstName: string
        lastName: string
        slug: string
        rating: number 
    }
    
    // Composers found
    let composers: SearchResult[] = [];
    // Tuples with search queries: the currently being requested from API, and the last inputted
    let queryStack: [ string?, string? ] = [ undefined, undefined ]
    
    /**
     * Performs request to API
     * @param query Search query
     */
    async function getFromApi(query: string): Promise<SearchResult[]> {
        const response = await fetch(`/api/search?q=${query}`)
        return await response.json()
    }
    
    /** Sends queries to API. Implements throttling so that no more than one parallel search request can be executed at each given moment. */
    async function queryApi() {
        if (queryStack[0] !== undefined) {
            composers = await getFromApi(queryStack[0])
            if (queryStack[1] !== undefined) {
                // Send the latest query to the API after the current one is done
                queryStack[0] = queryStack[1]
                queryStack[1] = undefined
                await queryApi()
            } else {
                queryStack[0] = undefined
            }
        } else {
            // Clean up results when there are no more queries
            composers = []
        }
    }

    /**
     * Saves user input into search field and sends requests to API
     * @param event User input
     */
    function handleSearch(event: { target: HTMLInputElement; }): void {
        // Convert empty string to undefined so it's more clear
        const inputEvent = event.target.value || undefined
        if (queryStack[0] === undefined) {
            // If no current searches, start the search
            queryStack[0] = inputEvent
            queryApi()
        } else {
            // If there is active search, save current input for the next search to run
            queryStack[1] = inputEvent
        }
    }
</script>

<div>
    <input class="search__field" type="search" placeholder="Search composers" on:input={handleSearch} />
    <div class="search__results">
        {#each composers as composer}
            <div><a href="/composer/{composer.slug}">{composer.lastName}, {composer.firstName}</a></div>
        {/each}
    </div>
</div>