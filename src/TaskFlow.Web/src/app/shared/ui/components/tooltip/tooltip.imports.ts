import { OverlayModule } from '@angular/cdk/overlay';

import {
  ZardTooltipComponent,
  ZardTooltipDirective,
} from '@/shared/ui/components/tooltip/tooltip';

export const ZardTooltipImports = [
  ZardTooltipComponent,
  ZardTooltipDirective,
  OverlayModule,
] as const;
