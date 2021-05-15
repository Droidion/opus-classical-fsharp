import typescript from '@rollup/plugin-typescript';
import svelte from 'rollup-plugin-svelte';
import sveltePreprocess from 'svelte-preprocess';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';
import { terser } from 'rollup-plugin-terser';

export default {
    input: 'Scripts/main.ts',
    output: {
        file: 'static/bundle.js',
        format: 'es',
        sourcemap: true,
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
        typescript(),
        terser()
    ],
};