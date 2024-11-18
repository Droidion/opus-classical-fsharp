/** @type {import('tailwindcss').Config} */
export default {
  content: ['Views.fs'],
  darkMode: ['selector', '[data-theme="dark"]'],
  theme: {
    colors: {
      lazer: '#CAA277',
      codgray: '#1a1a1a',
      mineshaft: '#373737',
      cinnamon: '#7E5503',
      white: '#FFFFFF',
      black: '#000000',
    },
  },
  plugins: [],
}
