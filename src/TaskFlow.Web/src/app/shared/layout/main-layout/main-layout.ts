import { Component, signal } from '@angular/core';
import { LayoutImports } from '@/shared/ui/components/layout';
import { ZardAvatarComponent } from '@/shared/ui/components/avatar';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import { ZardDividerComponent } from '@/shared/ui/components/divider';
import { ZardIcon, ZardIconComponent } from '@/shared/ui/components/icon';
import { ZardMenuImports } from '@/shared/ui/components/menu';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { ZardTooltipImports } from '@/shared/ui/components/tooltip';

interface MenuItem {
  icon: ZardIcon;
  label: string;
  submenu?: { label: string }[];
}

@Component({
  selector: 'app-main-layout',
  imports: [
    TranslatePipe,
    WithPrefixPipe,
    ZardIconComponent,
    ZardButtonComponent,
    LayoutImports,
    ZardButtonComponent,
    ZardMenuImports,
    ZardTooltipImports,
    ZardDividerComponent,
    ZardAvatarComponent,
    ZardIconComponent,
  ],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {
  public readonly sidebarCollapsed = signal(false);

  mainMenuItems: MenuItem[] = [
    { icon: 'house', label: 'Home' },
    { icon: 'inbox', label: 'Inbox' },
  ];

  workspaceMenuItems: MenuItem[] = [
    {
      icon: 'folder',
      label: 'Projects',
      submenu: [
        { label: 'Design System' },
        { label: 'Mobile App' },
        { label: 'Website' },
      ],
    },
    { icon: 'calendar', label: 'Calendar' },
    { icon: 'search', label: 'Search' },
  ];

  public toggleSidebar(): void {
    this.sidebarCollapsed.update((collapsed) => !collapsed);
  }

  public onCollapsedChange(collapsed: boolean): void {
    this.sidebarCollapsed.set(collapsed);
  }
}
