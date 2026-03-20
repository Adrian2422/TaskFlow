import { Component, inject, input } from '@angular/core';
import { ZardIconComponent } from '@/shared/ui/components/icon';
import { environment } from '../../../../../environments/environment';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { TranslatePipe } from '@ngx-translate/core';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-header',
  imports: [
    ZardIconComponent,
    ZardButtonComponent,
    TranslatePipe,
    WithPrefixPipe,
  ],
  templateUrl: './landing-header.component.html',
})
export class LandingHeader implements IComponentTranslate {
  private readonly _router = inject(Router);
  public readonly prefix = input('feature.landing.components.landing-header');
  public readonly appName = environment.appName;

  public signIn(): void {
    void this._router.navigateByUrl('/dashboard');
  }
}
