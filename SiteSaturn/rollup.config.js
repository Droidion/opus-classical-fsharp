import typescript from '@rollup/plugin-typescript';
import svelte from 'rollup-plugin-svelte';
import css from 'rollup-plugin-css-only';
import sveltePreprocess from 'svelte-preprocess';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';

export default {
    input: 'Scripts/main.ts',
    output: {
        file: 'static/js/bundle.js',
        format: 'es'
    },
    plugins: [
        svelte({
            preprocess: sveltePreprocess(),
        }),
        resolve({
            browser: true,
            dedupe: ['svelte']
        }),
        commonjs(),
        //css({ output: 'bundle.css' }),
        typescript()
    ],
};