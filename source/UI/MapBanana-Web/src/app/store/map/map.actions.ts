import { createAction, props } from '@ngrx/store';
import { IMap } from './map.model';

export const setMap = createAction('[Set Map] Set map', props<{ map: IMap }>());