/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#3B82F6',
          light: '#60A5FA',
          dark: '#2563EB'
        },
        secondary: {
          DEFAULT: '#10B981',
          light: '#34D399',
          dark: '#059669'
        }
      }
    },
  },
  plugins: [],
}
