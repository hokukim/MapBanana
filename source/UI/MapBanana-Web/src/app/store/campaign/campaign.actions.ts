import { Action, createAction, props } from '@ngrx/store';
import { ICampaign } from './campaign.model';

export const getCampaign = createAction('[Get Campaign] Get campaign', props<{id: string}>());
export const setCampaigns = createAction('[Set Campaigns] Set campaigns', props<{campaigns: Map<string, ICampaign[]>}>());