import { createReducer, on } from '@ngrx/store';
import * as MapActions from './map.actions';
import { IMap } from './map.model';

export const initialState: IMap = {
    id: '',
    name: '',
    imageUrl: '',
    imageSmallUrl: ''
};

export const mapReducer = createReducer(
    initialState,
    on(MapActions.setMap, (state, { map }) => {
        console.log(map);
        return map;
     })
);