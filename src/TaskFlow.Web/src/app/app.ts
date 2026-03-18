import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AppLanguages } from '@/shared/enums/app-languages';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  private readonly _translate = inject(TranslateService);

  constructor() {
    this._translate.addLangs([AppLanguages.ENGLISH, AppLanguages.POLISH]);
    this._translate.setFallbackLang(AppLanguages.ENGLISH);
    this._translate.use(AppLanguages.ENGLISH);
  }
}
