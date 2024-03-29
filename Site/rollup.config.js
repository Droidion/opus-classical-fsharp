import typescript from '@rollup/plugin-typescript';
import svelte from 'rollup-plugin-svelte';
import sveltePreprocess from 'svelte-preprocess';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';
import { terser } from 'rollup-plugin-terser';
import gzipPlugin from 'rollup-plugin-gzip';
import brotli from "rollup-plugin-brotli";

export default {
    input: 'Scripts/main.ts',
    output: {
        file: 'wwwroot/bundle.js',
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
        terser(),
        gzipPlugin({
            additionalFiles: ['wwwroot/bundle.css']
        }),
        brotli({
            additional: ['wwwroot/bundle.css']
        })
    ],
};