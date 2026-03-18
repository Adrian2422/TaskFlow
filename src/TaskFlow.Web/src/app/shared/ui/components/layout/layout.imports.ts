import { ContentComponent } from '@/shared/ui/components/layout/content.component';
import { FooterComponent } from '@/shared/ui/components/layout/footer.component';
import { HeaderComponent } from '@/shared/ui/components/layout/header.component';
import { LayoutComponent } from '@/shared/ui/components/layout/layout.component';
import {
  SidebarComponent,
  SidebarGroupComponent,
  SidebarGroupLabelComponent,
} from '@/shared/ui/components/layout/sidebar.component';

export const LayoutImports = [
  LayoutComponent,
  HeaderComponent,
  FooterComponent,
  ContentComponent,
  SidebarComponent,
  SidebarGroupComponent,
  SidebarGroupLabelComponent,
] as const;
