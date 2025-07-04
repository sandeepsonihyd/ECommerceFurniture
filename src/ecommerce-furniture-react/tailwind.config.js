/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#f5f7fa',
          100: '#ebeef3',
          200: '#d2dbe5',
          300: '#aabfd1',
          400: '#7c9fb8',
          500: '#5a82a2',
          600: '#486988',
          700: '#3d566f',
          800: '#36495d',
          900: '#303e4f',
        },
        secondary: {
          50: '#fdf8f3',
          100: '#fbeee2',
          200: '#f6dac0',
          300: '#efbe94',
          400: '#e79866',
          500: '#e07845',
          600: '#d15e3a',
          700: '#ae4932',
          800: '#8a3c30',
          900: '#70332a',
        }
      },
      fontFamily: {
        'sans': ['Inter', 'system-ui', 'sans-serif'],
      },
      boxShadow: {
        'soft': '0 2px 15px -3px rgba(0, 0, 0, 0.07), 0 10px 20px -2px rgba(0, 0, 0, 0.04)',
      }
    },
  },
  plugins: [],
} 