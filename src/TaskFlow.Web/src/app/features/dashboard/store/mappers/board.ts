import { BoardDto } from '@/shared/api/model';
import { Board } from '@/features/dashboard/store/models/board';

export function mapBoardFromDto(dto: BoardDto): Board {
  return {
    id: dto.id ?? crypto.randomUUID(),
    name: dto.name ?? '',
    description: dto.description ?? null,
  };
}
