import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
        },
    },
    build: {
        rollupOptions: {
            onwarn(warning, warn) {
                // Suppress PURE annotation warning in signalr
                if (
                    warning.code === 'SOURCEMAP_ERROR' ||
                    (warning.message &&
                        warning.message.includes('PURE') &&
                        warning.message.includes('@microsoft/signalr'))
                ) {
                    return;
                }

                // Let Vite handle all other warnings
                warn(warning);
            },
        },
    },
});
