# API Docs

[https://opusclassical.net/api/](https://opusclassical.net/api/)

## Authorization

Currently none.

## Endpoints

### `GET search`

Searches for composers by last name, sorts by relevance, returns top 5 results.

#### Query parameters

- `q: string` search query

#### Returns

```typescript
Array<{
    id: number
    lastName: string
    rating: number
}>
```

#### Response statuses

- `200 Ok`. 0–5 composers found
- `400 Bad request`. Query parameter is not `q: string`.

#### Request example

`curl "https://opusclassical.net/api/search?q=kor%20rim"`

#### Response example

```json
[
  {
    "id": 44,
    "lastName": "Rimsky-Korsakov",
    "rating": 0.3333333432674408
  },
  {
    "id": 68,
    "lastName": "Korngold",
    "rating": 0.2142857164144516
  }
]
```
