// vite.config.js
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5059', // <-- your ASP.NET Core API base
        changeOrigin: true,
        secure: false, // allow self-signed HTTPS in dev
      },
    },
  },
})