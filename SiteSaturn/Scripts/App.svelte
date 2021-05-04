<script lang="ts">
    type SearchResult = { 
        id: number
        lastName: string
        rating: number 
    }
    
    let names: SearchResult[] = [];
    
    async function handleSearch(event: { target: HTMLInputElement; }): Promise<void> {
        const response = await fetch(`http://localhost:5000/api/search?q=${event.target.value}`)
        const composers: SearchResult[] = await response.json()
        names = composers
    }
</script>

<div>
    <input class="search__field" type="search" placeholder="Search composers" on:input={handleSearch} />
    <div class="search__results">
        {#each names as name}
            <div>{name.lastName}</div>
        {/each}
    </div>
</div>