import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideZard } from '@/shared/ui/core/provider/providezard';
import { provideTranslateService } from '@ngx-translate/core';
import { AppLanguages } from '@/shared/enums/app-languages';
import { provideHttpClient } from '@angular/common/http';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideZard(),
    provideHttpClient(),
    provideTranslateService({
      loader: provideTranslateHttpLoader({
        prefix: '/i18n/',
        suffix: '.json',
      }),
      fallbackLang: AppLanguages.ENGLISH,
      lang: AppLanguages.ENGLISH,
    }),
  ],
};
