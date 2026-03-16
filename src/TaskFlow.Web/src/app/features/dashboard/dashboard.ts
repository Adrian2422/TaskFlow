import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';
import { DashboardStore } from '@/features/dashboard/store/dashboard-store';
import { MainLayout } from '@/shared/layout/main-layout/main-layout';
import {
  ZardBreadcrumbComponent,
  ZardBreadcrumbItemComponent,
} from '@/shared/ui/components/breadcrumb';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardDividerComponent } from '@/shared/ui/components/divider';
import { ZardIconComponent } from '@/shared/ui/components/icon';
import { ZardSkeletonComponent } from '@/shared/ui/components/skeleton';

@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    TranslatePipe,
    WithPrefixPipe,
    MainLayout,
    ZardBreadcrumbComponent,
    ZardBreadcrumbItemComponent,
    ZardButtonComponent,
    ZardDividerComponent,
    ZardIconComponent,
    ZardSkeletonComponent,
  ],
  providers: [DashboardStore],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements IComponentTranslate {
  public readonly store = inject(DashboardStore);

  public readonly prefix = input('feature.dashboard');
}
