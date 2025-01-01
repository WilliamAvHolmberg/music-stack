import { defineConfig } from 'orval';

export default defineConfig({
  api: {
    input: {
      target: 'http://localhost:5001/api/swagger/v1/swagger.json',
      validation: false,
    },
    output: {
      mode: 'split',
      target: './src/shared/api/hooks',
      schemas: './src/shared/api/models',
      client: 'react-query',
      prettier: true,
      override: {
        mutator: {
          path: './src/shared/api/client.ts',
          name: 'customClient',
        },
        query: {
          useQuery: true,
          useInfinite: true,
          useInfiniteQueryParam: 'pageNumber',
          options: {
            staleTime: 10000,
          }
        },
        components: {
          schemas: {
            suffix: 'DTO'
          }
        }
      },
    },
  },
});