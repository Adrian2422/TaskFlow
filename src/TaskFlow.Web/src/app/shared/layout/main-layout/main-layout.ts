import { Component, inject, input, signal } from '@angular/core';
import { LayoutImports } from '@/shared/ui/components/layout';
import { ZardAvatarComponent } from '@/shared/ui/components/avatar';
import { ZardButtonComponent } from '@/shared/ui/components/button';
import {
  ZARD_ICONS,
  ZardIcon,
  ZardIconComponent,
} from '@/shared/ui/components/icon';
import { ZardMenuImports } from '@/shared/ui/components/menu';
import { TranslatePipe } from '@ngx-translate/core';
import { WithPrefixPipe } from '@/shared/pipes/with-prefix-pipe';
import { ZardTooltipImports } from '@/shared/ui/components/tooltip';
import { IComponentTranslate } from '@/shared/interfaces/component-translate';
import { AuthService } from '@/shared/services/auth.service';

type UserRole = 'user' | 'admin';

interface MenuItem {
  icon: ZardIcon;
  label: string;
  submenu?: { label: string }[];
  command?: () => void;
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
    ZardAvatarComponent,
    ZardIconComponent,
  ],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout implements IComponentTranslate {
  private readonly authService = inject(AuthService);

  public readonly sidebarCollapsed = signal(false);

  // Placeholder for future auth integration; for now the host can pass `userRole="admin"`
  // to unlock admin-only navigation entries.
  public readonly userRole = input<UserRole>('user');

  public readonly prefix = input('layout');

  get mainMenuItems(): MenuItem[] {
    return [
      { icon: 'layout-dashboard', label: 'sidebar.dashboard' },
      { icon: 'clipboard', label: 'sidebar.my-tasks' },
      { icon: 'bell', label: 'sidebar.notifications' },
    ];
  }

  get workspaceMenuItems(): MenuItem[] {
    const isAdmin = this.userRole() === 'admin';

    const boardsItem: MenuItem = isAdmin
      ? {
          icon: 'folder-open',
          label: 'sidebar.boards',
          submenu: [
            { label: 'sidebar.all-boards' },
            { label: 'sidebar.create-board' },
          ],
        }
      : { icon: 'folder-open', label: 'sidebar.boards' };

    return [
      boardsItem,
      { icon: 'inbox', label: 'sidebar.backlog' },
      { icon: 'archive', label: 'sidebar.archive' },
      ...(isAdmin
        ? [{ icon: ZARD_ICONS['file-text'], label: 'sidebar.reports' }]
        : []),
    ];
  }

  get userMenuItems(): MenuItem[] {
    return [
      { icon: ZARD_ICONS.user, label: 'user-menu.profile' },
      { icon: ZARD_ICONS.settings, label: 'user-menu.settings' },
      {
        icon: 'log-out',
        label: 'user-menu.log-out',
        command: () => this.authService.logout(),
      },
    ];
  }

  public toggleSidebar(): void {
    this.sidebarCollapsed.update((collapsed) => !collapsed);
  }

  public onCollapsedChange(collapsed: boolean): void {
    this.sidebarCollapsed.set(collapsed);
  }
}
