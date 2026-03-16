import { Component } from '@angular/core';
import {Header} from '@/layout/header/header';
import {ZardButtonComponent} from '@/shared/ui/components/button';

@Component({
  selector: 'app-landing',
  imports: [
    Header,
    ZardButtonComponent
  ],
  templateUrl: './landing.html',
  styleUrl: './landing.scss'
})
export class Landing {}
