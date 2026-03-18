import { ZardContextMenuDirective } from '@/shared/ui/components/menu/context-menu.directive';
import { ZardMenuContentDirective } from '@/shared/ui/components/menu/menu-content.directive';
import { ZardMenuItemDirective } from '@/shared/ui/components/menu/menu-item.directive';
import { ZardMenuLabelComponent } from '@/shared/ui/components/menu/menu-label.component';
import { ZardMenuShortcutComponent } from '@/shared/ui/components/menu/menu-shortcut.component';
import { ZardMenuDirective } from '@/shared/ui/components/menu/menu.directive';

export const ZardMenuImports = [
  ZardContextMenuDirective,
  ZardMenuContentDirective,
  ZardMenuItemDirective,
  ZardMenuDirective,
  ZardMenuLabelComponent,
  ZardMenuShortcutComponent,
] as const;
