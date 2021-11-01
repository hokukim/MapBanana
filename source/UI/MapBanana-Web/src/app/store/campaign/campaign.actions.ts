import { createAction, props } from '@ngrx/store';
import { ICampaign } from './campaign.model';

export const setCampaign = createAction('[Set Campaign] Set campaign', props<{ campaign: ICampaign }>());
export const setActiveMapId = createAction('Set Active Map ID] Set active map ID', props<{ campaignId: string, mapId: string }>());