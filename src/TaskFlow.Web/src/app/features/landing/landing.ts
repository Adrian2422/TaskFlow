import { Component, input } from '@angular/core';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { LandingHeader } from '@/features/landing/components/landing-header/landing-header.component';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';

@Component({
  selector: 'app-landing',
  imports: [ZardButtonComponent, LandingHeader, TranslatePipe, WithPrefixPipe],
  templateUrl: './landing.html',
  styleUrl: './landing.scss',
})
export class Landing implements IComponentTranslate {
  public readonly prefix = input('feature.landing');
}
