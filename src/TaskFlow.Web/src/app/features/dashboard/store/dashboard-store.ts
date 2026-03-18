import { signalStore, withState } from '@ngrx/signals';
import { BaseState } from '@/shared/stores/common/base-state';

const initialState: BaseState = {
  loadingState: 'idle',
  error: 'null',
};

export const DashboardStore = signalStore(
  withState(initialState)
  // withEntities(),
  // withMethods(),
  // withHooks({
  //   onInit(store) {
  //     store.loadBoards();
  //   },
  // })
);
