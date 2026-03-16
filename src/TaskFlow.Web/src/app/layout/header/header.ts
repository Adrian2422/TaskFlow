import { Component } from '@angular/core';
import {ZardIconComponent} from '@/shared/ui/components/icon';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-header',
  imports: [
    ZardIconComponent
  ],
  templateUrl: './header.html',
})
export class Header {
  public readonly appName = environment.appName;
}
