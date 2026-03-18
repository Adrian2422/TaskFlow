import { environment } from './src/environments/environment';

export default {
  taskflow: {
    input: './openapi.json',
    output: {
      mode: 'tags-split',
      baseUrl: environment.apiUrl,
      target: './src/app/shared/api/taskflow.ts',
      schemas: './src/app/shared/api/model',
      client: 'angular',
    },
    hooks: {
      afterAllFilesWrite: 'prettier ./src/app/shared/api --write',
      // mock: true,
    },
  },
};
