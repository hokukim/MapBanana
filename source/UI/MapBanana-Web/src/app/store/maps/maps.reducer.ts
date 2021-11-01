import { createReducer, on } from '@ngrx/store';
import * as MapsActions from './maps.actions';
import { IMap } from './maps.model';

export const initialState: Map<string, IMap> = new Map<string, IMap>();

export const mapsReducer = createReducer(
    initialState,
    on(MapsActions.setMaps, (state, { maps }) => {
        console.log(maps);
        state = maps

        return state;
    })
);