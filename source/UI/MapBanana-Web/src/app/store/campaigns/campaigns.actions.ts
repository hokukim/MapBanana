import { createAction, props } from '@ngrx/store';
import { ICampaign } from './campaigns.model';

export const getCampaigns = createAction('[Get Campaigns] Get campaigns', props<{ campaigns: Map<string, ICampaign> }>());
export const setCampaigns = createAction('[Set Campaigns] Set campaigns', props<{ campaigns: Map<string, ICampaign>}>());
export const getCampaign = createAction('[Get Campaign] Get campaign', props<{id: string}>());