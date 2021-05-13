import App from './App.svelte';

const target = document.getElementById('searchBlock')
const app = target ? new App({ target }) : undefined;

export default app;