import { computed, Injectable, signal } from '@angular/core';
import { IBreadcrumb } from '@/shared/interfaces/breadcrumb';

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbStore {
  private readonly _homeBreadcrumb: IBreadcrumb = {
    label: 'layout.breadcrumbs.home',
    icon: 'house',
    routerLink: ['/'],
  };

  private readonly _items = signal<IBreadcrumb[]>([]);

  public readonly items = computed(() => [
    this._homeBreadcrumb,
    ...this._items(),
  ]);

  public set(breadcrumbs: IBreadcrumb[]): void {
    this._items.set(breadcrumbs);
  }

  public reset(): void {
    this._items.set([]);
  }
}
