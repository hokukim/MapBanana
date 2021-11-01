import { IAppState } from 'src/app/app.state';
import { IMap } from './maps.model';

import { createSelector } from '@ngrx/store';
 
export const selectMaps = createSelector(
    (state: IAppState) => state.maps,
    (maps: Map<string, IMap>) => maps      
);

export const selectMap = (mapId: string) => createSelector(
    (state: IAppState) => state.maps,
    (maps: Map<string, IMap>) => maps.get(mapId)
);