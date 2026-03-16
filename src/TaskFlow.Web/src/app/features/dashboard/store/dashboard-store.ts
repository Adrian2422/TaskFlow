import {
  patchState,
  signalStore,
  withHooks,
  withMethods,
  withState,
} from '@ngrx/signals';
import {
  removeEntity,
  setAllEntities,
  withEntities,
} from '@ngrx/signals/entities';
import { Board } from '@/features/dashboard/store/models/board';
import { inject } from '@angular/core';
import { BoardsService } from '@/shared/api/boards/boards.service';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { map, pipe, switchMap, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { mapBoardFromDto } from '@/features/dashboard/store/mappers/board';

type LoadingState = 'idle' | 'loading' | 'loaded' | 'error';

interface DashboardState {
  loadingState: LoadingState;
  error: string | null;
}

const initialState: DashboardState = {
  loadingState: 'idle',
  error: null,
};

export const DashboardStore = signalStore(
  withState(initialState),
  withEntities<Board>(),
  withMethods((store, boardApi = inject(BoardsService)) => ({
    loadBoards: rxMethod<void>(
      pipe(
        tap(() => patchState(store, { loadingState: 'loading', error: null })),
        switchMap(() =>
          boardApi.getAllBoards().pipe(
            map((boards) => boards.map((board) => mapBoardFromDto(board))),
            tapResponse({
              next: (boards) =>
                patchState(store, setAllEntities(boards), {
                  loadingState: 'loaded',
                }),
              error: (err: Error) =>
                patchState(store, {
                  loadingState: 'error',
                  error: err.message,
                }),
            })
          )
        )
      )
    ),
    removeBoard(id: string): void {
      patchState(store, removeEntity(id));
    },
  })),
  withHooks({
    onInit(store) {
      store.loadBoards();
    },
  })
);
