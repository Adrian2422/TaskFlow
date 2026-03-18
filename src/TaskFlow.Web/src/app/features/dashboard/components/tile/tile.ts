import { Component, input } from '@angular/core';
import { ITileData } from '@/features/dashboard/interfaces/tile-data';
import { ZardCardComponent } from '@/shared/ui/components/card';
import { ZardIconComponent } from '@/shared/ui/components/icon';

@Component({
  selector: 'app-tile',
  imports: [ZardCardComponent, ZardIconComponent],
  templateUrl: './tile.html',
  styleUrl: './tile.scss',
  host: {
    class: 'flex flex-1',
  },
})
export class Tile {
  public readonly data = input.required<ITileData>();
}
