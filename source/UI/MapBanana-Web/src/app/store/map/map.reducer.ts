import { Action, createReducer, on } from '@ngrx/store';
import * as MapActions from './map.actions';

export const initialState = {
    id: '',
    name: '',
    imageUrl: '',
    imageSmallUrl: ''
};

export const mapReducer = createReducer(
    initialState,
    on(MapActions.getMap, (state, { map }) =>
        ({
            id: map.id,
            name: map.name,
            imageUrl: map.imageUrl,
            imageSmallUrl: map.imageSmallUrl
         }))
  );