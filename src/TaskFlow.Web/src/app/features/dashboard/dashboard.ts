import {
  Component,
  inject,
  input,
  OnDestroy,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';
import { MainLayout } from '@/shared/layout/main-layout/main-layout';
import { MainHeader } from '@/shared/layout/main-header/main-header';
import { BreadcrumbStore } from '@/shared/stores/breadcrumbs/breadcrumb-store';
import { IBreadcrumb } from '@/shared/interfaces/breadcrumb';
import { Tile } from '@/features/dashboard/components/tile/tile';
import { ITileData } from '@/features/dashboard/interfaces/tile-data';
import { RecentBoards } from '@/features/dashboard/components/recent-boards/recent-boards';
import { OldestTasks } from '@/features/dashboard/components/oldest-tasks/oldest-tasks';

@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    TranslatePipe,
    WithPrefixPipe,
    MainLayout,
    MainHeader,
    Tile,
    RecentBoards,
    OldestTasks,
  ],
  providers: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements IComponentTranslate, OnInit, OnDestroy {
  protected readonly breadcrumbs = inject(BreadcrumbStore);

  public readonly prefix = input('feature.dashboard');
  protected readonly tiles = signal<ITileData[]>([]);

  private readonly _breadcrumbs: IBreadcrumb[] = [
    {
      label: 'layout.breadcrumbs.dashboard',
    },
  ];

  public ngOnInit(): void {
    this.breadcrumbs.set(this._breadcrumbs);
    this.tiles.set([
      {
        title: 'Active boards',
        metric: '4',
        icon: 'clipboard',
      },
      {
        title: 'Total tasks',
        metric: '52',
        icon: 'file',
      },
      {
        title: 'Completed tasks',
        metric: '6%',
        icon: 'activity',
      },
    ]);
  }

  public ngOnDestroy(): void {
    this.breadcrumbs.reset();
  }
}
