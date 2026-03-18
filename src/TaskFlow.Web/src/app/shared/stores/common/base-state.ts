import { LoadingState } from '@/shared/stores/common/loading-state';

export interface BaseState {
  loadingState: LoadingState;
  error: string | null;
}
