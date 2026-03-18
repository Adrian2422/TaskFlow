import { Component, inject, input, output } from '@angular/core';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import {
  ZardBreadcrumbComponent,
  ZardBreadcrumbItemComponent,
} from '@/shared/ui/components/breadcrumb';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardDividerComponent } from '@/shared/ui/components/divider';
import { ZardIconComponent } from '@/shared/ui/components/icon';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';
import { BreadcrumbStore } from '@/shared/stores/breadcrumbs/breadcrumb-store';

@Component({
  selector: 'app-main-header',
  imports: [
    TranslatePipe,
    WithPrefixPipe,
    ZardBreadcrumbComponent,
    ZardBreadcrumbItemComponent,
    ZardButtonComponent,
    ZardDividerComponent,
    ZardIconComponent,
  ],
  templateUrl: './main-header.html',
  styleUrl: './main-header.scss',
})
export class MainHeader implements IComponentTranslate {
  public readonly breadcrumbStore = inject(BreadcrumbStore);
  public readonly prefix = input('layout.header');
  public readonly toggled = output<void>();
}
