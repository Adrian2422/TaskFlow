import { Component } from '@angular/core';
import {ZardButtonComponent} from '@/shared/ui/components/button';
import {LandingHeader} from '@/features/landing/components/landing-header/landing-header.component';

@Component({
  selector: 'app-landing',
  imports: [
    ZardButtonComponent,
    LandingHeader
  ],
  templateUrl: './landing.html',
  styleUrl: './landing.scss'
})
export class Landing {}
