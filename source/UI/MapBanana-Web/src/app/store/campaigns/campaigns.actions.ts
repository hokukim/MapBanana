import { createAction, props } from '@ngrx/store';
import { ICampaign } from './campaigns.model';

export const setCampaigns = createAction('[Set Campaigns] Set campaigns', props<{ campaigns: Map<string, ICampaign>}>());