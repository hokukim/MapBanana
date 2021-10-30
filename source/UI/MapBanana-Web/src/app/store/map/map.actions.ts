import { Action, createAction, props } from '@ngrx/store';
import { IMap } from './map.model';

export const getMap = createAction(
    '[Get Map] Get camp',
    props<{map: IMap}>()
);