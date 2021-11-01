import { IAppState } from 'src/app/app.state';
import { IMap } from './map.model';

import { createSelector } from '@ngrx/store';
 
export const selectMap = createSelector(
    (state: IAppState) => state.map,
    (map: IMap) => map
);