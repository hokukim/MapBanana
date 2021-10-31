import { createReducer, on } from '@ngrx/store';
import * as MapsActions from './maps.actions';
import { IMap } from './maps.model';

export const initialState: Map<string, IMap> = new Map<string, IMap>();

export const campaignsReducer = createReducer(
    initialState,
    on(MapsActions.getMaps, (state, { maps }) => maps)
);