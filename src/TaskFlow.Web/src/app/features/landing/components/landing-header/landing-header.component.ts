import { Component } from '@angular/core';
import {ZardIconComponent} from '@/shared/ui/components/icon';
import {environment} from '../../../../../environments/environment';
import {ZardButtonComponent} from '@/shared/ui/components/button';

@Component({
  selector: 'app-landing-header',
  imports: [
    ZardIconComponent,
    ZardButtonComponent
  ],
  templateUrl: './landing-header.component.html'
})
export class LandingHeader {
  public readonly appName = environment.appName;
}
