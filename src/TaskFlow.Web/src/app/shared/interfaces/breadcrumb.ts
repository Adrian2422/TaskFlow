import { ZardIcon } from '../ui/components/icon';

export interface IBreadcrumb {
  label: string;
  icon?: ZardIcon;
  routerLink?: string[];
}
