import { createAction, props } from '@ngrx/store';
import { IMap } from './maps.model';

export const getMaps = createAction('', props<{ maps: Map<string, IMap> }>());