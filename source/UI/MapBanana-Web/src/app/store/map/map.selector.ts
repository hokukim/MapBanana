import { IAppState } from 'src/app/app.store';
import { IMap } from './map.model';

import { createSelector } from '@ngrx/store';
 
export const selectMaps = (state: IAppState) => state.maps;
 
export const selectMap = (id: string) => createSelector(
  selectMaps,
  (maps: Map<string, IMap>) => maps.get(id)
);