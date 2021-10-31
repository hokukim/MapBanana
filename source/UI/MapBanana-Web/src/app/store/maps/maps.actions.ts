import { createAction, props } from '@ngrx/store';
import { IMap } from './maps.model';

export const getMaps = createAction('[GET MAPS] Get maps', props<{ maps: Map<string, IMap> }>());
export const setMaps = createAction('[SET MAPS] Set maps', props<{ maps: Map<string, IMap> }>());